using AxGrid;
using AxGrid.FSM;
using UnityEngine;

namespace SlotMachine
{
    [State("Ready")]
    public class ReadyState : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            Log.Debug($"{Parent.CurrentStateName} ENTER");

            Model.Set("StartEnabled", true);

            Model.EventManager.Invoke("OnStartEnabledChanged");

            Model.EventManager.AddAction("OnStartClick", OnStartClick);
        }

        [Exit]
        private void ExitThis()
        {
            Log.Debug($"{Parent.CurrentStateName} EXIT");

            Model.Set("StartEnabled", false);

            Model.EventManager.Invoke("OnStartEnabledChanged");
        }

        private void OnStartClick()
        {
            Parent.Change("Start");
        }
    }
}
