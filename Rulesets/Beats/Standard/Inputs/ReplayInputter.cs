using System;
using System.IO;
using System.Collections.Generic;
using PBGame.IO;
using PBGame.UI;
using PBGame.Rulesets.Beats.Standard.UI;
using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBFramework.Inputs;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets.Beats.Standard.Inputs
{
    public class ReplayInputter : BaseInputter, IInputReceiver
    {
        private StreamReader replayStream;
        private DataStreamReader<ReplayableInput> replayReader;

        private Dictionary<KeyCode, ReplayableInput> playbackInputs = new Dictionary<KeyCode, ReplayableInput>();


        public int InputLayer => InputLayers.GameScreenComponents;

        [ReceivesDependency]
        private IInputManager InputManager { get; set; }


        public ReplayInputter(FileInfo replayFile, HitBarDisplay hitBar, HitObjectHolder hitObjectHolder) : base(hitBar, hitObjectHolder)
        {
            base.replayFile = replayFile;
        }

        [InitWithDependency]
        private void Init()
        {
            replayReader = new DataStreamReader<ReplayableInput>(InputManager.MaxCursorCount * 60, readInterval: 500);
        }

        public bool ProcessInput()
        {
            float curTime = hitObjectHolder.CurrentTime;

            UpdateInputs(curTime);

            if (!GameSession.IsPaused)
            {
                while (true)
                {
                    var rawInput = replayReader.PeekData();
                    if (rawInput == null || rawInput.Time > curTime)
                        break;

                    var playbackInput = GetPlaybackInput(rawInput);

                    if (playbackInput.State.Value == InputState.Press)
                    {
                        if (!hitBarCursor.IsActive && IsOnHitBar(playbackInput, out float pos))
                            TriggerCursorPress(playbackInput.Time, playbackInput, pos);
                        else
                            TriggerKeyPress(playbackInput.Time, playbackInput, playbackInput);
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

        /// <summary>
        /// Returns the ReplayableInput instance for playback.
        /// </summary>
        private ReplayableInput GetPlaybackInput(ReplayableInput rawInput)
        {
            ReplayableInput input = null;
            if (!playbackInputs.TryGetValue(rawInput.Key, out input))
                playbackInputs.Add(rawInput.Key, input = new ReplayableInput());

            input.SetFromCursor(rawInput.Time, rawInput);
            return input;
        }
    }
}