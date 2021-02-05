using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Beats.Standard.UI;
using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBFramework.UI;
using PBFramework.Audio;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.Beats.Standard
{
    public abstract class GameProcessor : UguiObject {

        /// <summary>
        /// Returns the current time of the music.
        /// </summary>
        public abstract float CurrentTime { get; }

        [ReceivesDependency]
        private IGameSession GameSession { get; set; }

        [ReceivesDependency]
        protected IMusicController MusicController { get; set; }

        [ReceivesDependency]
        protected HitObjectHolder HitObjectHolder { get; set; }


        [InitWithDependency]
        private void Init()
        {
            // Assign processor instance to other game modules.
            // TODO: Inputter
            HitObjectHolder.SetGameProcessor(this);
        }

        /// <summary>
        /// Handles passive judgement of the specified hit object view.
        /// </summary>
        public abstract void JudgePassive(float curTime, HitObjectView hitObjectView);

        protected void Update()
        {
            if (!GameSession.IsPlaying)
                return;

            float curTime = CurrentTime;
            HitObjectHolder.UpdateObjects(curTime);
        }
    }
}