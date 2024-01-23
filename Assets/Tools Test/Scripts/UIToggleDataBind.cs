using AxGrid.Base;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AxGrid.Tools.Binders
{

	/// <summary>
	/// ���� toggle-� � ������
	/// </summary>
	[RequireComponent(typeof(Toggle))]
	public class UIToggleDataBind : Binder
	{
		private Toggle toggle;
		/// <summary>
		/// ��� ������ (���� ������ ������� �� ����� �������)
		/// </summary>
		public string toggleName = "";

		public string enableField = "";

		/// <summary>
		/// �������� �� ���������
		/// </summary>
		public bool defaultEnable = true;

		/// <summary>
		/// ���� �� ������ �������� Toggle.IsOn
		/// </summary>
		public string activeField = "";

		/// <summary>
		/// Toggle.isOn �� ���������
		/// </summary>
		public bool defaultActive = true;

		/// <summary>
		/// ���������� ������� �� ��������������� UI fsm
		/// </summary>
		public bool isFsmUI = false;

		[OnAwake]
		public void awake()
		{
			toggle = GetComponent<Toggle>();
			if (string.IsNullOrEmpty(toggleName))
				toggleName = name;

			enableField = enableField == "" ? $"Toggle{toggleName}Enable" : enableField;
			activeField = activeField == "" ? $"Toggle{toggleName}Active" : activeField;

			toggle.onValueChanged.AddListener(OnClick);
		}

		[OnStart]
		public void start()
		{
			Model.EventManager.AddAction($"On{enableField}Changed", OnItemEnable);
			Model.EventManager.AddAction($"On{activeField}Changed", OnItemActiveChange);

			OnItemEnable();
			OnItemActiveChange();
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

			Model.EventManager.RemoveAction($"On{enableField}Changed", OnItemEnable);
			Model.EventManager.RemoveAction($"On{activeField}Changed", OnItemActiveChange);
		}

		public void OnClick(bool active) => OnClick();

		public void OnClick()
		{
			if (!toggle.interactable || !isActiveAndEnabled)
				return;

			Model?.EventManager.Invoke("SoundPlay", "Click");

			Model?.Set(activeField, toggle.isOn);

			Settings.Fsm?.Invoke("OnToggle", toggleName);

			Model?.EventManager.Invoke($"On{toggleName}Click");
		}
	}
}