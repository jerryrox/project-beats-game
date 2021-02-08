using PBGame.Rulesets.Objects;
using PBGame.Rulesets.Beats.Standard.Objects;
using PBGame.Rulesets.Judgements;
using PBFramework.UI;
using PBFramework.Inputs;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets.Beats.Standard.UI.Components
{
    public class DraggerCircleView : HitCircleView, IDraggerComponent, IRecyclable<DraggerCircleView> {

        /// <summary>
        /// Because of unity input's limitations, it is quite challenging to receive a max judgement when releasing the dragger
        /// at a seemingly perfect timing.
        /// By applying some extra milliseconds before the input is considered release, such frustrating gameplay experience due
        /// to the above case could be resolved.
        /// </summary>
        private const float BonusReleaseTime = 50;

        protected ISprite holdSprite;

        protected IAnime holdAni;
        protected IAnime releaseAni;

        private DraggerView draggerView;
        private Dragger dragger;

        private bool isHolding;
        private bool wasHolding;
        private Vector3 myPosition = new Vector3();

        /// <summary>
        /// The time when the circle has been flagged release.
        /// </summary>
        private float releaseTime;


        public DraggerView DraggerView => draggerView;

        IRecycler<DraggerCircleView> IRecyclable<DraggerCircleView>.Recycler { get; set; }


        [InitWithDependency]
        private void Init()
        {
            holdSprite = CreateChild<UguiSprite>("hold", 4);
            {
                holdSprite.Anchor = AnchorType.Fill;
                holdSprite.Offset = Offset.Zero;
                holdSprite.SpriteName = "circle-320";
                holdSprite.Alpha = 0f;
            }

            holdAni = new Anime();
            holdAni.AnimateFloat(a => holdSprite.Alpha = a)
                .AddTime(0f, () => holdSprite.Alpha)
                .AddTime(0.35f, 0.25f)
                .Build();
            holdAni.AnimateVector3(s => holdSprite.Scale = s)
                .AddTime(0f, () => holdSprite.Scale)
                .AddTime(0.35f, new Vector3(2f, 2f, 2f))
                .Build();

            releaseAni = new Anime();
            releaseAni.AnimateFloat(a => holdSprite.Alpha = a)
                .AddTime(0f, () => holdSprite.Alpha)
                .AddTime(0.35f, 0f)
                .Build();
            releaseAni.AnimateVector3(s => holdSprite.Scale = s)
                .AddTime(0f, () => holdSprite.Scale)
                .AddTime(0.35f, Vector3.one)
                .Build();

            hitAni.AddEvent(hitAni.Duration, () =>
            {
                if(draggerView != null)
                    draggerView.Active = false;
            });
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (holdSprite != null)
            {
                holdSprite.Alpha = 0f;
                holdSprite.Scale = Vector3.one;
            }
        }

        public void SetDragger(DraggerView draggerView)
        {
            RemoveDragger();
            if(draggerView == null)
                return;

            this.draggerView = draggerView;
            this.dragger = draggerView.HitObject;
            SetParent(draggerView);

            // Override judgement end time so it doesn't go over any other nested object's start time
            // nor the dragger's end time.
            judgeEndTime = Mathf.Min(judgeEndTime, dragger.EndTime);
            foreach (var o in dragger.NestedObjects)
            {
                if(o != hitObject)
                    judgeEndTime = Mathf.Min(judgeEndTime, o.StartTime);
            }

            // Reset to initial position
            Position = Vector3.zero;
        }

        public void RemoveDragger()
        {
            if(draggerView == null)
                return;

            SetParent(draggerView.Parent);
            draggerView = null;
            dragger = null;
        }

        /// <summary>
        /// Plays the hit animation.
        /// </summary>
        public void PlayHit() => hitAni.PlayFromStart();

        /// <summary>
        /// Sets whether dragger is holding.
        /// </summary>
        public void SetHold(bool holding, float curTime)
        {
            if(this.isHolding == holding)
                return;
            // Show visual change only if there is currently a valid result for this hit object.
            if (!IsJudged)
                return;

            // Held down
            if (holding && !wasHolding)
            {
                isHolding = true;

                releaseAni.Stop();
                holdAni.PlayFromStart();
            }
            // Released
            else if(!holding && wasHolding)
            {
                isHolding = false;
                releaseTime = curTime;

                holdAni.Stop();
                releaseAni.PlayFromStart();
            }

            wasHolding = isHolding;
        }

        public override bool IsHolding(float? curTime = null)
        {
            if(!curTime.HasValue)
                return isHolding;
            return isHolding || curTime.Value < releaseTime + BonusReleaseTime;
        }

        public override JudgementResult SetResult(HitResultType hitResult, float offset)
        {
            var judgement = base.SetResult(hitResult, offset);
            if (hitAni.IsPlaying)
                hitAni.Stop();
            return judgement;
        }

        public override JudgementResult JudgeInput(float curTime, IInput input)
        {
            JudgementResult result = base.JudgeInput(curTime, input);
            if (result != null)
            {
                // Don't play hit animation just yet.
                hitAni.Stop();
            }

            switch (input.State.Value)
            {
                case InputState.Press:
                case InputState.Hold:
                    SetHold(true, curTime);
                    break;

                case InputState.Release:
                case InputState.Idle:
                    SetHold(false, curTime);
                    break;
            }
            return result;
        }

        public override void SoftInit()
        {
            base.SoftInit();

            Active = true;
            Position = Vector3.zero;
        }

        public override void SoftDispose()
        {
            base.SoftDispose();

            isHolding = false;
            wasHolding = false;
            myPosition = Vector3.zero;
            releaseTime = 0f;
        }

        public override void HardDispose()
        {
            base.HardDispose();
            RemoveDragger();
        }

        public void UpdatePosition()
        {
            if(draggerView == null)
                return;

            float progress = draggerView.GetHitProgress(GameSession.GameProcessor.CurrentTime);
            if(progress < 0f)
                return;
            else if(progress > 1f)
                progress = 1f;
            myPosition.x = dragger.GetPosition(progress).x;
            myPosition.y = draggerView.DistUnderHitPos;
            this.Position = myPosition;
        }

        protected override void EvalPassiveJudgement()
        {
            SetResult(HitResultType.Miss, judgeEndTime);
        }
    }
}