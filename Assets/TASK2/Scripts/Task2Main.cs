using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid.Path;
using UnityEngine;

namespace Task2
{
    public class Card
    {
        public int value;
        public string cardName;
        public string spriteName;

        public Card(int _value, string _cardName, string _spriteName)
        {
            value = _value;
            cardName = _cardName;
            spriteName = _spriteName;
        }
    }

    public class Task2Main : MonoBehaviourExtBind
    {
        [OnStart]
        private void StartThis()
        {
            Log.Debug("Start");
            Settings.Fsm = new FSM();
            Settings.Fsm.Add(new InitState());
            Settings.Fsm.Add(new IdleState());
            Settings.Fsm.Add(new CreateState());

            Settings.Fsm.Start("Init");
        }
    }
}
