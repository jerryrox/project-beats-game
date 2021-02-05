using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Beats.Standard.UI;
using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBGame.Rulesets.Beats.Standard.Inputs;
using PBGame.Rulesets.Judgements;
using PBFramework.UI;
using PBFramework.Audio;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.Beats.Standard
{
    public abstract class BeatsStandardProcessor : GameProcessor
    {
        private IGameInputter inputter;


        [ReceivesDependency]
        protected HitObjectHolder HitObjectHolder { get; set; }

        [ReceivesDependency]
        protected PlayAreaContainer PlayAreaContainer { get; set; }

        [ReceivesDependency]
        protected TouchEffectDisplay TouchEffectDisplay { get; set; }


        [InitWithDependency]
        private void Init()
        {
            // Init inputter
            inputter = CreateGameInputter();
            TouchEffectDisplay.SetInputter(inputter);

            // Assign processor instance to other game modules.
            inputter.SetGameProcessor(this);
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

        /// <summary>
        /// Adds the specified judgement result to the score processor.
        /// </summary>
        public void AddJudgement(JudgementResult result)
        {
            if (result != null)
            {
                if(inputter.HitBarCursor.IsActive)
                    inputter.HitBarCursor.ReportNewResult(result);

                GameSession?.ScoreProcessor.ProcessJudgement(result);
            }
        }

        /// <summary>
        /// Creates a new game inputter instance.
        /// </summary>
        protected abstract IGameInputter CreateGameInputter();
    }
}