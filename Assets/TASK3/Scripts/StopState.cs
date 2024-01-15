using AxGrid;
using AxGrid.FSM;
using UnityEngine;

namespace SlotMachine
{
    [State("Stop")]
    public class StopState : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            Log.Debug($"{Parent.CurrentStateName} ENTER");

            Model.Add("StopEnabled", false);

            Model.EventManager.Invoke("OnStopEnabledChanged");

            Model.EventManager.Invoke("ChangeWheelRotate", false);
        }
        
        [Exit]
        private void ExitThis()
        {
            Log.Debug($"{Parent.CurrentStateName} EXIT");
        }
    }
}
