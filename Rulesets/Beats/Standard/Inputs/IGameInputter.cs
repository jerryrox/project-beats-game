using System;
using System.IO;
using System.Collections.Generic;
using PBGame.Rulesets.Beats.Standard.UI.Components;

namespace PBGame.Rulesets.Beats.Standard.Inputs
{
    public interface IGameInputter {

        /// <summary>
        /// Event called on new cursor press.
        /// </summary>
        event Action<BeatsCursor> OnCursorPress;

        /// <summary>
        /// Event called on existing cursor release.
        /// </summary>
        event Action<BeatsCursor> OnCursorRelease;

        /// <summary>
        /// Event called on new key press.
        /// </summary>
        event Action<BeatsKey> OnKeyPress;

        /// <summary>
        /// Event called on existing key release.
        /// </summary>
        event Action<BeatsKey> OnKeyRelease;


        /// <summary>
        /// List of cursors currently pressed down on the hit bar for aiming.
        /// </summary>
        BeatsCursor HitBarCursor { get; }

        /// <summary>
        /// List of inputs serving press/hold/release actions.
        /// </summary>
        List<BeatsKey> KeyInputs { get; }

        /// <summary>
        /// Returns the file containing new or existing replay data.
        /// </summary>
        FileInfo ReplayFile { get; }


        /// <summary>
        /// Handles passive judgements for the specified object.
        /// </summary>
        void JudgePassive(float curTime, HitObjectView view);
    }
}