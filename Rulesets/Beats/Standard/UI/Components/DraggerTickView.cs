using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Beats.Standard.Objects;
using PBGame.Rulesets.Judgements;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Inputs;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.Beats.Standard.UI.Components
{
    public class DraggerTickView : HitObjectView<DraggerTick>, IRecyclable<DraggerTickView>, IDraggerComponent {

        protected ISprite tickSprite;

        protected IAnime hitAni;

        private DraggerView draggerView;


        public DraggerView DraggerView => draggerView;

        IRecycler<DraggerTickView> IRecyclable<DraggerTickView>.Recycler { get; set; }

        [ReceivesDependency]
        private PlayAreaContainer PlayArea { get; set; }


        [InitWithDependency]
        private void Init()
        {
            tickSprite = CreateChild<UguiSprite>("tick", 0);
            {
                tickSprite.Anchor = AnchorType.Fill;
                tickSprite.Offset = Offset.Zero;
                tickSprite.SpriteName = "circle-32";
            }

            hitAni = new Anime();
            hitAni.AnimateFloat(a => this.Alpha = a)
                .AddTime(0f, 1f, EaseType.QuadEaseOut)
                .AddTime(0.5f, 0f)
                .Build();
            hitAni.AnimateVector3(s => this.Scale = s)
                .AddTime(0f, Vector3.one, EaseType.CubicEaseOut)
                .AddTime(0.5f, new Vector3(2.5f, 2.5f, 1f))
                .Build();
            hitAni.AddEvent(hitAni.Duration, () => Active = false);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (hitAni != null)
            {
                hitAni.Stop();
            }
        }

        // Ticks shouldn't be judged by direct input.
        public override JudgementResult JudgeInput(float curTime, IInput input) => null;

        /// <summary>
        /// Sets the parent dragger view.
        /// Specifying a null dragger should be equivalent to calling RemoveDragger.
        /// </summary>
        public void SetDragger(DraggerView draggerView)
        {
            RemoveDragger();
            if(draggerView == null)
                return;

            this.draggerView = draggerView;
            SetParent(draggerView);

            // Reset to initial position
            SetInitialPosition();
        }

        /// <summary>
        /// Disassociates this object from parent dragger.
        /// </summary>
        public void RemoveDragger()
        {
            if(draggerView == null)
                return;

            SetParent(draggerView.Parent);
            draggerView = null;
        }

        public override void SetHitObject(DraggerTick hitObject)
        {
            base.SetHitObject(hitObject);

            this.Size = new Vector2(32f, 32f);
        }

        public override bool IsPastJudgeEnd(float curTime)
        {
            return curTime > hitObject.StartTime;
        }

        public override void SoftInit()
        {
            base.SoftInit();

            Active = true;
            SetInitialPosition();
        }

        public override void HardDispose()
        {
            base.HardDispose();
            RemoveDragger();
        }

        public override JudgementResult SetResult(HitResultType hitResult, float offset)
        {
            var judgement = base.SetResult(hitResult, offset);
            if (judgement.IsHit)
                hitAni.PlayFromStart();
            else
                Active = false;
            return judgement;
        }

        public void UpdatePosition()
        {
            this.Y = Mathf.Max(draggerView.StartCircle.Position.y, this.Y);
        }

        protected override void EvalPassiveJudgement()
        {
            if (draggerView != null && draggerView.StartCircle != null)
                SetResult(draggerView.StartCircle.IsHolding() ? HitResultType.Perfect : HitResultType.Miss, 0f);
            else
                SetResult(HitResultType.Miss, 0f);
        }

        /// <summary>
        /// Sets the position of the tick to be at its initial position within the dragger.
        /// </summary>
        private void SetInitialPosition()
        {
            if(hitObject == null || draggerView == null)
                return;
            Position = new Vector3(
                hitObject.X,
                (hitObject.StartTime - draggerView.HitObject.StartTime) * PlayArea.DistancePerTime
            );
        }
    }
}