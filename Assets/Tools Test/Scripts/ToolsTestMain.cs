using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid.Path;
using UnityEngine;

namespace ToolsTest
{
    public class ToolsTestMain : MonoBehaviourExtBind
    {
        [OnStart]
        private void StartThis()
        {
            Log.Debug("Start");
            Settings.Fsm = new FSM();
            Settings.Fsm.Add(new InitState());

            Settings.Fsm.Start("Init");
        }
    }
}
