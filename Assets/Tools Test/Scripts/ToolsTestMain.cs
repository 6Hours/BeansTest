using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid.Path;
using SmartFormat;
using UnityEngine;

namespace ToolsTest
{
    public class ToolsTestMain : MonoBehaviourExtBind
    {
        [SerializeField] private TMPro.TMP_Text sliderFormatedLable;

        [OnStart]
        private void StartThis()
        {
            Log.Debug("Start");
            Settings.Fsm = new FSM();
            Settings.Fsm.Add(new InitState());

            Model.EventManager.AddAction("OnTestClick", OnSliderClick);

            Settings.Fsm.Start("Init");
        }

        private void OnSliderClick()
        {
            sliderFormatedLable.text = Smart.Format("{0} {0:plural(ru):€блоко|€блока|€блок}", Model.Get<float>("SliderValue"));
        }
    }
}
