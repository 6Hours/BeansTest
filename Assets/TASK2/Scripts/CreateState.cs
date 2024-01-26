using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;

namespace Task2
{
    [State("Create")]
    public class CreateState : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            Log.Debug($"{Parent.CurrentStateName} ENTER");

            int currentId = Model.Get("CardCount", 0);

            Model.Get<DynamicList<Card>>("FirstList").Add(new Card(currentId, $"cardName{currentId}", $"sprite{currentId}"));
            
            Model.Inc("CardCount", 1);

            Parent.Change("Idle");
        }

        [Exit]
        private void ExitThis()
        {
            Log.Debug($"{Parent.CurrentStateName} EXIT");
        }
    }
}
