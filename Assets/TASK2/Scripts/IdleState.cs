using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;
using System.Collections.Generic;

namespace Task2
{
    [State("Idle")]
    public class IdleState : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            Log.Debug($"{Parent.CurrentStateName} ENTER");
        }

        [Exit]
        private void ExitThis()
        {
            Log.Debug($"{Parent.CurrentStateName} EXIT");
        }

        [Bind]
        private void OnBtn(string btnName)
        {
            if(btnName == "CreateButton")
            {
                Parent.Change("Create");
            }
        }

        [Bind]
        private void OnCardBtn(Card card)
        {
            Model.Get<List<Card>>("FirstList").Remove(card);

            Model.EventManager.Invoke("OnFirstListChanged");

            Model.Get<List<Card>>("SecondList").Add(card);

            Model.EventManager.Invoke("OnSecondListChanged");
        }
    }
}
