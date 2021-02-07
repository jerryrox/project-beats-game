using System.IO;
using System.Collections.Generic;
using PBGame.Rulesets.Beats.Standard.UI;
using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBFramework.Inputs;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets.Beats.Standard.Inputs
{
    public class ReplayInputter : BaseInputter
    {
        private Dictionary<KeyCode, ReplayableInput> playbackInputs = new Dictionary<KeyCode, ReplayableInput>();


        [ReceivesDependency]
        private IRecycler<ReplayableInput> replayInputRecycler { get; set; }


        public ReplayInputter(FileInfo replayFile, HitBarDisplay hitBar, HitObjectHolder hitObjectHolder) : base(hitBar, hitObjectHolder)
        {
            base.replayFile = replayFile;
        }

        public void UpdateInputs(float curTime, List<ReplayableInput> inputs)
        {
            if (!GameSession.IsPaused)
            {
                foreach (var rawInput in inputs)
                {
                    var playbackInput = GetPlaybackInput(rawInput);
                    if (playbackInput.State.Value == InputState.Press)
                    {
                        if (!hitBarCursor.IsActive && IsOnHitBar(playbackInput, out float pos))
                            TriggerCursorPress(curTime, playbackInput, pos);
                        else
                            TriggerKeyPress(curTime, playbackInput, playbackInput);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the ReplayableInput instance for playback.
        /// </summary>
        private ReplayableInput GetPlaybackInput(ReplayableInput rawInput)
        {
            ReplayableInput input = null;
            if (!playbackInputs.TryGetValue(rawInput.Key, out input))
                playbackInputs.Add(rawInput.Key, input = replayInputRecycler.GetNext());

            input.SetFromCursor(rawInput);
            return input;
        }
    }
}