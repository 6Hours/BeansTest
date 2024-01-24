using AxGrid.Base;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AxGrid.Tools.Binders
{

	/// <summary>
	/// ���� slider-� � ������
	/// </summary>
	[RequireComponent(typeof(Slider))]
	public class UISliderDataBind : Binder
	{
		private Slider slider;
		/// <summary>
		/// ��� ���������� (���� ������ ������� �� ����� �������)
		/// </summary>
		public string sliderName = "";

		public string enableField = "";

		/// <summary>
		/// �������� �� ���������
		/// </summary>
		public bool defaultEnable = true;

		/// <summary>
		/// ���� �� ������ �������� Slider.value
		/// </summary>
		public string valueField = "";

		/// <summary>
		/// Slider.value �� ���������
		/// </summary>
		public float defaultValue = 0.5f;

		/// <summary>
		/// ���� �� ������ �������� Slider.minValue
		/// </summary>
		public string minValueField = "";

		/// <summary>
		/// Slider.minValue �� ���������
		/// </summary>
		public float defaultMinValue = 0f;

		/// <summary>
		/// ���� �� ������ �������� Slider.maxValue
		/// </summary>
		public string maxValueField = "";

		/// <summary>
		/// Slider.maxValue �� ���������
		/// </summary>
		public float defaultMaxValue = 1f;

		/// <summary>
		/// ���� �� ������ �������� Slider.wholeNumbers
		/// </summary>
		public string wholeNumbersActiveField = "";

		/// <summary>
		/// Slider.wholeNumbers �� ���������
		/// </summary>
		public bool defaultWholeNumbersActive = false;

		/// <summary>
		/// ���������� ������� �� ��������������� UI fsm
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