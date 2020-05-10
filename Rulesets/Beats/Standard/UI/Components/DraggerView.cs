using System;
using System.Linq;
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

        private DraggerBody draggerBody;
        private DraggerCircleView startCircle;
        private List<DraggerTickView> tickViews = new List<DraggerTickView>();


        public DraggerCircleView StartCircle => startCircle;

        /// <summary>
        /// Current y distance under the hit position.
        /// </summary>
        /// <value></value>
        public float DistUnderHitPos => PlayArea.HitPosition - this.Y;
        
        IRecycler<DraggerView> IRecyclable<DraggerView>.Recycler { get; set; }

        [ReceivesDependency]
        private IRecycler<DraggerCircleView> DragCircleRecycler { get; set; }

        [ReceivesDependency]
        private IRecycler<DraggerTickView> TickRecyler { get; set; }

        [ReceivesDependency]
        private PlayAreaContainer PlayArea { get; set; }


        [InitWithDependency]
        private void Init()
        {
            draggerBody = CreateChild<DraggerBody>(depth: -10);
        }

        public override void SetHitObject(Dragger hitObject)
        {
            base.SetHitObject(hitObject);

            draggerBody.SetDragger(hitObject);
            draggerBody.RenderPath();

            startCircle = DragCircleRecycler.GetNext();
            {
                startCircle.Depth = 1000;
                startCircle.Position = Vector3.zero;
                startCircle.SetDragger(this);
                startCircle.SetHitObject(hitObject.NestedObjects[0] as DraggerStartCircle);
            }

            // Skip first index because it's a dragger circle.
            float distPerTime = PlayArea.DistancePerTime;
            for (int i = 1; i < hitObject.NestedObjects.Count; i++)
            {
                var tick = hitObject.NestedObjects[i] as DraggerTick;
                var tickView = TickRecyler.GetNext();
                {
                    tickView.SetDragger(this);
                    tickView.SetHitObject(tick);
                    tickView.Position = new Vector3(
                        tick.X,
                        (tick.StartTime - hitObject.StartTime) * distPerTime
                    );
                }
            }
        }

        public override JudgementResult JudgeInput(float curTime, IInput input)
        {
            // Direct judgements via input will only be done for the start circle.
            // For this and other nested objects, they must be handled through passive judgement.
            return startCircle.JudgeInput(curTime, input);
        }

        public override void HardDispose()
        {
            base.HardDispose();
            startCircle = null;
            tickViews.Clear();

            draggerBody.ClearPath();
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