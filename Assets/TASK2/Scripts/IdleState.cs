using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;

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
        private void OnCardBtn(CardItem cardItem)
        {
            Model.EventManager.Invoke("MoveCard", cardItem);

            Model.Get<DynamicList<Card>>("FirstList").Remove(cardItem.Card);
            Model.Get<DynamicList<Card>>("SecondList").Add(cardItem.Card);
        }
    }
}
