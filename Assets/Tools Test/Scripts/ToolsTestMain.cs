using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid.Path;
using SmartFormat;
using UniRx;
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

            Observable.Start(() => {
                int Factorial(int stage)
                {
                    if(stage > 0)
                        return stage * Factorial(stage - 1);
                    else
                        return 1;
                };
                int n = 1000;
                int res = Factorial(n);
                return res;
            }).ObserveOnMainThread()
              .Subscribe(xs => {
                  Debug.Log("res: " + xs);
              }).AddTo(this);
        }

        private void OnSliderClick()
        {
            sliderFormatedLable.text = Smart.Format("{0} {0:plural(ru):€блоко|€блока|€блок}", Model.Get<float>("SliderValue"));
        }
    }
}
