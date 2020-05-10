using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Beats.Standard.Objects;
using PBGame.Rulesets.Judgements;
using PBFramework.UI;
using PBFramework.Inputs;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.Beats.Standard.UI.Components
{
    public class DraggerView : HitObjectView<Dragger>, IRecyclable<DraggerView> {

        private DraggerCircleView startCircle;


        public DraggerCircleView StartCircle => startCircle;

        IRecycler<DraggerView> IRecyclable<DraggerView>.Recycler { get; set; }


        [InitWithDependency]
        private void Init()
        {
        }

        public override void SetHitObject(Dragger hitObject)
        {
        }

        public override JudgementResult JudgeInput(float curTime, IInput input)
        {
            // Direct judgements via input will only be done for the start circle.
            // For this and other nested objects, they must be handled through passive judgement.
            return startCircle.JudgeInput(curTime, input);
        }

        protected override void EvalPassiveJudgement()
        {
            var judgementsCount = BaseNestedObjects.Count;
            var judgementsHit = BaseNestedObjects.Count(o => o.Result.IsHit);
            var hitRatio = (float)judgementsHit / judgementsCount;

            HitResultType resultType = HitResultType.Miss;
            if (hitRatio == 1f && startCircle.Result.HitResult == HitResultType.Perfect)
                resultType = HitResultType.Perfect;
            else if (hitRatio >= 0.5f && startCircle.Result.HitResult <= HitResultType.Good)
                resultType = HitResultType.Great;
            else if (hitRatio > 0f)
                resultType = HitResultType.Good;

            SetResult(resultType, judgeEndTime);

            if(resultType != HitResultType.Miss)
                startCircle.PlayHit();
        }
    }
}