using AxGrid.Base;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AxGrid.Tools.Binders
{

	/// <summary>
	/// Бинд slider-а в модель
	/// </summary>
	[RequireComponent(typeof(Slider))]
	public class UISliderDataBind : Binder
	{
		private Slider slider;
		/// <summary>
		/// Имя компонента (если пустое берется из имени объекта)
		/// </summary>
		public string sliderName = "";

		public string enableField = "";

		/// <summary>
		/// Включена по умолчанию
		/// </summary>
		public bool defaultEnable = true;

		/// <summary>
		/// Поле из модели значения Slider.value
		/// </summary>
		public string valueField = "";

		/// <summary>
		/// Slider.value по умолчанию
		/// </summary>
		public float defaultValue = 0.5f;

		/// <summary>
		/// Поле из модели значения Slider.minValue
		/// </summary>
		public string minValueField = "";

		/// <summary>
		/// Slider.minValue по умолчанию
		/// </summary>
		public float defaultMinValue = 0f;

		/// <summary>
		/// Поле из модели значения Slider.maxValue
		/// </summary>
		public string maxValueField = "";

		/// <summary>
		/// Slider.maxValue по умолчанию
		/// </summary>
		public float defaultMaxValue = 1f;

		/// <summary>
		/// Поле из модели значения Slider.wholeNumbers
		/// </summary>
		public string wholeNumbersActiveField = "";

		/// <summary>
		/// Slider.wholeNumbers по умолчанию
		/// </summary>
		public bool defaultWholeNumbersActive = false;

		/// <summary>
		/// Отправляет события во вспомогательную UI fsm
		/// </summary>
		public bool isFsmUI = false;

		[OnAwake]
		public void awake()
		{
			slider = GetComponent<Slider>();
			if (string.IsNullOrEmpty(sliderName))
				sliderName = name;

			enableField = enableField == "" ? $"Slider{sliderName}Enable" : enableField;
			valueField = valueField == "" ? $"Slider{sliderName}Value" : valueField;
			minValueField = minValueField == "" ? $"Slider{sliderName}MinValue" : minValueField;
			maxValueField = maxValueField == "" ? $"Slider{sliderName}MaxValue" : maxValueField;
			wholeNumbersActiveField = wholeNumbersActiveField == "" ? $"Slider{sliderName}WholeNumbersActive" : wholeNumbersActiveField;

			slider.onValueChanged.AddListener(OnClick);
		}

		[OnStart]
		public void start()
		{
			Model.EventManager.AddAction($"On{enableField}Changed", OnItemEnable);
			Model.EventManager.AddAction($"On{valueField}Changed", OnValueChange);
			Model.EventManager.AddAction($"On{minValueField}Changed", OnMinValueChange);
			Model.EventManager.AddAction($"On{maxValueField}Changed", OnMaxValueChange);
			Model.EventManager.AddAction($"On{wholeNumbersActiveField}Changed", OnWholeNumbersActiveChange);

			OnItemEnable();
			OnValueChange();
			OnMinValueChange();
			OnMaxValueChange();
			OnWholeNumbersActiveChange();
		}

		public void OnItemEnable()
		{
			if (slider.interactable != Model.GetBool(enableField, defaultEnable))
				slider.interactable = Model.GetBool(enableField, defaultEnable);
		}

		public void OnValueChange()
		{
			if (slider.value != Model.GetFloat(valueField, defaultValue))
				slider.SetValueWithoutNotify(Model.GetFloat(valueField, defaultValue));
		}

		public void OnMinValueChange()
		{
			if (slider.minValue != Model.GetFloat(minValueField, defaultMinValue))
				slider.minValue = Model.GetFloat(minValueField, defaultMinValue);
		}

		public void OnMaxValueChange()
		{
			if (slider.maxValue != Model.GetFloat(maxValueField, defaultMaxValue))
				slider.maxValue = Model.GetFloat(maxValueField, defaultMaxValue);
		}

		public void OnWholeNumbersActiveChange()
		{
			if (slider.wholeNumbers != Model.GetBool(wholeNumbersActiveField, defaultWholeNumbersActive))
				slider.wholeNumbers = Model.GetBool(wholeNumbersActiveField, defaultWholeNumbersActive);
		}

		[OnDestroy]
		public void onDestroy()
		{
			slider.onValueChanged.RemoveAllListeners();

			Model.EventManager.RemoveAction($"On{enableField}Changed", OnItemEnable);
			Model.EventManager.RemoveAction($"On{valueField}Changed", OnValueChange);
			Model.EventManager.RemoveAction($"On{minValueField}Changed", OnMinValueChange);
			Model.EventManager.RemoveAction($"On{maxValueField}Changed", OnMaxValueChange);
			Model.EventManager.RemoveAction($"On{wholeNumbersActiveField}Changed", OnWholeNumbersActiveChange);
		}

		public void OnClick(float value)
		{
			if (!slider.interactable || !isActiveAndEnabled)
				return;

			Model?.EventManager.Invoke("SoundPlay", "Click");

			Model?.Set(valueField, value);

			Settings.Fsm?.Invoke("OnToggle", sliderName);

			Model?.EventManager.Invoke($"On{sliderName}Click");
		}
	}
}