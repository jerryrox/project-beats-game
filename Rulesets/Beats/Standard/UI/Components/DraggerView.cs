using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PBGame.Audio;
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
        public float DistUnderHitPos => PlayArea.HitPosition - this.Y;

        public override bool IsHoldable => true;

        public override Color Tint
        {
            get => startCircle.Tint;
            set => startCircle.Tint = value;
        }

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

            draggerBody.Active = true;
            draggerBody.SetDragger(hitObject);
            draggerBody.RenderPath();

            startCircle = DragCircleRecycler.GetNext();
            {
                startCircle.Active = true;
                startCircle.SetDragger(this);
                startCircle.Depth = -1;
                startCircle.Position = Vector3.zero;
                startCircle.SetHitObject(hitObject.NestedObjects[0] as DraggerStartCircle);

                AddNestedObject(startCircle);
            }

            // Skip first index because it's a dragger circle.
            float distPerTime = PlayArea.DistancePerTime;
            for (int i = 1; i < hitObject.NestedObjects.Count; i++)
            {
                var tick = hitObject.NestedObjects[i] as DraggerTick;
                var tickView = TickRecyler.GetNext();
                {
                    tickView.Active = true;
                    tickView.SetDragger(this);
                    tickView.SetHitObject(tick);
                    AddNestedObject(tickView);

                    tickViews.Add(tickView);
                }
            }
        }

        public override JudgementResult JudgeInput(float curTime, IInput input)
        {
            // Direct judgements via input will only be done for the start circle.
            // For this and other nested objects, they must be handled through passive judgement.
            return startCircle.JudgeInput(curTime, input);
        }

        public override bool IsHolding(float? curTime)
        {
            return startCircle.IsHolding(curTime);
        }

        public override bool IsCursorInRange(float x)
        {
            float curPos = xPos + startCircle.X;
            return x > curPos - radius && x < curPos + radius;
        }

        public override void SoftDispose()
        {
            base.SoftDispose();

            draggerBody.Active = true;
        }

        public override void HardDispose()
        {
            base.HardDispose();
            startCircle = null;
            tickViews.Clear();

            draggerBody.ClearPath();
            draggerBody.Active = false;
        }

        public override JudgementResult SetResult(HitResultType hitResult, float offset)
        {
            if (!startCircle.IsJudged)
                return startCircle.SetResult(hitResult, offset);

            var judgement = base.SetResult(hitResult, offset);
            if (judgement.HitResult == HitResultType.Miss)
                Active = false;
            else
            {
                draggerBody.Active = false;
                startCircle.PlayHit();
                startCircle.SetHold(true, judgeEndTime);
            }
            return judgement;
        }

        protected override float GetPosOnJudgement() => hitObject.X + hitObject.EndX;

        protected override void EvalPassiveJudgement()
        {
            var judgementsCount = BaseNestedObjects.Count + 1;
            var judgementsHit = BaseNestedObjects.Count(o => o.Result.IsHit) + (startCircle.IsHolding(judgeEndTime) ? 1 : 0);
            var hitRatio = (float)judgementsHit / judgementsCount;

            HitResultType resultType = HitResultType.Miss;
            if (hitRatio == 1f && startCircle.Result.HitResult == HitResultType.Perfect)
                resultType = HitResultType.Perfect;
            else if (hitRatio >= 0.5f && startCircle.Result.HitResult <= HitResultType.Good)
                resultType = HitResultType.Great;
            else if (hitRatio > 0f)
                resultType = HitResultType.Good;

            SetResult(resultType, judgeEndTime);
        }

        protected override PlayableHitsound CreatePlayableHitsound()
        {
            if (hitObject.SamplePoint == null)
                throw new Exception("There must be a valid sample point in order to process hit sounds!");
            return new PlayableHitsound(GameSession.MapAssetStore, hitObject.SamplePoint, hitObject.EndSamples, SoundPool);
        }

        protected void Update()
        {
            startCircle?.UpdatePosition();
            for (int i = 0; i < tickViews.Count; i++)
                tickViews[i].UpdatePosition();
        }
    }
}