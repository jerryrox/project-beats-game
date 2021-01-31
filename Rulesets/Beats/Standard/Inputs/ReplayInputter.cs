using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using PBGame.IO;
using PBGame.UI;
using PBGame.Rulesets.Beats.Standard.UI;
using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBFramework.Inputs;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.Beats.Standard.Inputs
{
    public class ReplayInputter : BaseInputter, IInputReceiver
    {
        private StreamReader replayStream;
        private DataStreamReader<ReplayableInput> replayReader;


        public int InputLayer => InputLayers.GameScreenComponents;

        [ReceivesDependency]
        private IInputManager InputManager { get; set; }


        public ReplayInputter(FileInfo replayFile, HitBarDisplay hitBar, HitObjectHolder hitObjectHolder) : base(hitBar, hitObjectHolder)
        {
            base.replayFile = replayFile;

            replayReader = new DataStreamReader<ReplayableInput>(InputManager.MaxTouchCount * 60, readInterval: 500);
        }

        public bool ProcessInput()
        {
            float curTime = hitObjectHolder.CurrentTime;

            UpdateInputs(curTime);

            if (!GameSession.IsPaused)
            {
                while (true)
                {
                    var input = replayReader.PeekData();
                    if (input == null || input.Time > curTime)
                        break;

                    if (input.State.Value == InputState.Press)
                    {
                        if (!hitBarCursor.IsActive && IsOnHitBar(input, out float pos))
                            TriggerCursorPress(input, pos);
                        else
                            TriggerKeyPress(input, input);
                    }
                    replayReader.AdvanceIndex();
                }
            }

            return true;
        }

        protected override void OnSoftInit()
        {
            base.OnSoftInit();

            InputManager.AddReceiver(this);

            replayStream = new StreamReader(replayFile.OpenRead());
            replayReader.StartStream(replayStream);
        }

        protected override void OnSoftDispose()
        {
            base.OnSoftDispose();

            replayReader.StopStream();
            replayStream = null;

            InputManager.RemoveReceiver(this);
        }

        int IComparable<IInputReceiver>.CompareTo(IInputReceiver other) => other.InputLayer.CompareTo(InputLayer);

        void IInputReceiver.PrepareInputSort() { }

    }
}