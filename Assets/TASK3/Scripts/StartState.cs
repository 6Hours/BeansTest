using AxGrid;
using AxGrid.FSM;
using UnityEngine;

namespace SlotMachine
{
    [State("Start")]
    public class StartState : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            Log.Debug($"{Parent.CurrentStateName} ENTER");

            Model.EventManager.AddAction("OnStopClick", OnStopClick);

            Model.EventManager.Invoke("ChangeWheelRotate", true);
        }

        [One(3f)]
        private void EnableStopBtn()
        {
            Debug.Log("Enable Stop Button");

            Model.Add("StopEnabled", true);

            Model.EventManager.Invoke("OnStopEnabledChanged");

            Model.EventManager.Remove("OnStartClick");
        }

        [Exit]
        private void ExitThis()
        {
            Log.Debug($"{Parent.CurrentStateName} EXIT");
        }

        private void OnStopClick()
        {
            Parent.Change("Stop");
        }
    }
}
