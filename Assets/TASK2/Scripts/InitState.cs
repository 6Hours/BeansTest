using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;

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
            Model.Set("FirstList", new DynamicList<Card>());
            Model.Set("SecondList", new DynamicList<Card>());

            Parent.Change("Idle");
        }

        [Exit]
        private void ExitThis()
        {
            Log.Debug($"{Parent.CurrentStateName} EXIT");
        }

        [Bind]
        private void OnToggle(string toggleName)
        {
            Model.Set("RedSquare", Model.GetBool("AgreePrivacy"));
        }
    }
}
