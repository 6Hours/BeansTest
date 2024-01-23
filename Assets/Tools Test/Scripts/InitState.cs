using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;

namespace ToolsTest
{
    [State("Init")]
    public class InitState : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            Log.Debug($"{Parent.CurrentStateName} ENTER");

            Model.Set("AgreePrivacy", false);
            Model.Set("RedSquare", false);
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
