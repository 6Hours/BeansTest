using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid.Path;
using UnityEngine;

namespace SlotMachine
{
    public class SlotMachine : MonoBehaviourExtBind
    {
        [SerializeField] private RectTransform[] lines = new RectTransform[5];

        private int[] linesNumbers = new int[5];
        private int rotateLineFrom = 5;
        private bool isStopWheel;

        [OnStart]
        private void StartThis()
        {
            Log.Debug("Start");
            Settings.Fsm = new FSM();
            Settings.Fsm.Add(new ReadyState());
            Settings.Fsm.Add(new StartState());
            Settings.Fsm.Add(new StopState());

            Settings.Fsm.Start("Ready");

            Model.EventManager.AddParameterAction<bool>(ChangeWheelRotate);
        }

        [OnUpdate]
        private void UpdateThis()
        {
            //Log.Debug("Task3 Update");

            Settings.Fsm.Update(Time.deltaTime);

            for(int i = rotateLineFrom; i < 5; i++)
            {
                lines[i].anchoredPosition += Vector2.down * Time.deltaTime * 1000f;

                if (isStopWheel && i == rotateLineFrom)
                {
                    if(lines[i].anchoredPosition.y < linesNumbers[i] * lines[i].sizeDelta.y - 100f)
                    {
                        var lineInd = rotateLineFrom;
                        Path.Action(() => { }).EasingLinear(
                            0.5f,
                            lines[lineInd].anchoredPosition.y,
                            linesNumbers[lineInd] * lines[lineInd].sizeDelta.y,
                            (f) => lines[lineInd].anchoredPosition = Vector2.up * f).Action(() => {
                                if(rotateLineFrom < lines.Length)
                                {
                                    isStopWheel = true;
                                }
                                else
                                {
                                    Settings.Fsm.Change("Ready");
                                }
                            });

                        isStopWheel = false;
                        rotateLineFrom++;
                    }
                }
                else if (lines[i].anchoredPosition.y < 0f)
                {
                    lines[i].anchoredPosition += Vector2.up * lines[i].sizeDelta.y * 3f;
                }
            }
        }

        private void ChangeWheelRotate(bool _isRotate)
        {
            if(_isRotate)
            {
                rotateLineFrom = 0;
                for (var i = 0; i < linesNumbers.Length; i++)
                {
                    linesNumbers[i] = Random.Range(0, 3);
                }
            }

            isStopWheel = !_isRotate;
        }
    }
}
