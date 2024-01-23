using AxGrid.Base;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AxGrid.Tools.Binders
{

	/// <summary>
	/// Бинд toggle-а в модель
	/// </summary>
	[RequireComponent(typeof(Toggle))]
	public class UIToggleDataBind : Binder
	{
		private Toggle toggle;
		/// <summary>
		/// Имя кнопки (если пустое берется из имени объекта)
		/// </summary>
		public string toggleName = "";

		public string enableField = "";

		/// <summary>
		/// Включена по умолчанию
		/// </summary>
		public bool defaultEnable = true;

		/// <summary>
		/// Поле из модели значения Toggle.IsOn
		/// </summary>
		public string activeField = "";

		/// <summary>
		/// Toggle.isOn по умолчанию
		/// </summary>
		public bool defaultActive = true;

		/// <summary>
		/// Поле из модели где взять настройку клавиатуры
		/// </summary>
		public string keyField = "";

		/// <summary>
		/// Кнопка клавиатуры (заполнится из модели если там есть)
		/// </summary>
		public string key = "";

		/// <summary>
		/// Срабатывать на нажатие
		/// </summary>
		public bool onKeyPress = false;

		/// <summary>
		/// Отправляет события во вспомогательную UI fsm
		/// </summary>
		public bool isFsmUI = false;

		private bool down = false;
		private float downTime = 0.0f;
		private bool cancel = false;

		private EventTrigger et;

		[OnAwake]
		public void awake()
		{
			toggle = GetComponent<Toggle>();
			if (string.IsNullOrEmpty(toggleName))
				toggleName = name;

			enableField = enableField == "" ? $"Toggle{toggleName}Enable" : enableField;
			activeField = activeField == "" ? $"Toggle{toggleName}Active" : activeField;
			if (!onKeyPress)
				toggle.onValueChanged.AddListener(OnClick);
			else
			{
				et = gameObject.AddComponent<EventTrigger>();
				var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
				entry.callback.AddListener(OnClick);
				et.triggers.Add(entry);
			}
		}

		[OnStart]
		public void start()
		{
			Model.EventManager.AddAction($"On{enableField}Changed", OnItemEnable);
			Model.EventManager.AddAction($"On{activeField}Changed", OnItemActiveChange);
			if (keyField == "")
				keyField = $"{name}Key";
			if (keyField != "")
			{
				key = Model.GetString(keyField, key);
				Model.EventManager.AddAction($"On{keyField}Changed", OnKeyChanged);
			}
			OnItemEnable();
			OnItemActiveChange();
		}

		public void OnKeyChanged()
		{
			key = Model.GetString(keyField);
		}

		public void CancelClick()
		{
			cancel = !onKeyPress;
		}


		public void OnItemEnable()
		{
			if (toggle.interactable != Model.GetBool(enableField, defaultEnable))
				toggle.interactable = Model.GetBool(enableField, defaultEnable);
		}

		public void OnItemActiveChange()
		{
			if (toggle.isOn != Model.GetBool(activeField, defaultActive))
				toggle.SetIsOnWithoutNotify(Model.GetBool(activeField, defaultActive));
		}

		[OnDestroy]
		public void onDestroy()
		{
			toggle.onValueChanged.RemoveAllListeners();
			if (et != null)
			{
				et.triggers.ForEach(t => t.callback.RemoveAllListeners());
				et.triggers.Clear();
			}
			Model.EventManager.RemoveAction($"On{enableField}Changed", OnItemEnable);
			Model.EventManager.RemoveAction($"On{activeField}Changed", OnItemActiveChange);
			Model.EventManager.RemoveAction($"On{keyField}Changed", OnKeyChanged);
		}

		private void OnClick(BaseEventData bd)
		{
			Log.Debug("CLICK!");
			if (!cancel)
				OnClick();
			cancel = false;
		}

		public void OnClick(bool active) => OnClick();

		public void OnClick()
		{
			if (!toggle.interactable || !isActiveAndEnabled)
				return;

			if (!cancel)
			{
				Model?.EventManager.Invoke("SoundPlay", "Click");

				Model?.Set(activeField, toggle.isOn);

				Settings.Fsm?.Invoke("OnToggle", toggleName);

				Model?.EventManager.Invoke($"On{toggleName}Click");
			}
			cancel = false;
		}


		[OnUpdate]
		protected void update()
		{
			if (!toggle.interactable || key == "")
				return;

			if (onKeyPress && !down && Input.GetKeyDown(key))
			{
				if (onKeyPress)
					OnClick();

				if (!down)
				{
					down = true;
					downTime = 0;
				}
			}
			if (!onKeyPress && Input.GetKeyUp(key)) Log.Info($"Key:{key} / D:{down} / C:{cancel}");
			if (!onKeyPress && Input.GetKeyUp(key))
			{
				OnClick();
				down = true;
			}

			if (Input.GetKeyUp(key))
			{
				cancel = false;
				down = false;
			}

			if (down)
				downTime += Time.deltaTime;
			if (downTime >= 2f)
			{
				down = false;
				cancel = false;
			}
		}
	}
}