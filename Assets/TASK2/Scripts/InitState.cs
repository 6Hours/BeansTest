using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;
using System.Collections.Generic;

namespace Task2
{
    [State("Init")]
    public class InitState : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            Log.Debug($"{Parent.CurrentStateName} ENTER");

            Model.Set("CardCount", 0);

            Model.Set("FirstList", new List<Card>());
            Model.Set("SecondList", new List<Card>());

            Parent.Change("Idle");
        }

        [Exit]
        private void ExitThis()
        {
            Log.Debug($"{Parent.CurrentStateName} EXIT");
        }
    }
}
