using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Beats.Standard.Inputs;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;
using PBGame.Rulesets.Beats.Standard.UI.Components;

namespace PBGame.Rulesets.Beats.Standard
{
    public class LocalGameProcessor : BeatsStandardProcessor
    {
        private LocalPlayerInputter inputter;


        public override float CurrentTime => MusicController.CurrentTime;


        public override void JudgePassive(float curTime, HitObjectView hitObjectView)
        {
            foreach (var judgement in hitObjectView.JudgePassive(curTime))
                AddJudgement(judgement);
        }

        protected override IGameInputter CreateGameInputter()
        {
            inputter = new LocalPlayerInputter(
                PlayAreaContainer.HitBar,
                HitObjectHolder
            );
            Dependencies.Inject(inputter);
            return inputter;
        }
    }
}