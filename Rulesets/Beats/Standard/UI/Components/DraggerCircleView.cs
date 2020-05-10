using System;
using System.Collections;
using System.Collections.Generic;
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

        private bool isHolding;
        private bool wasHolding;

        /// <summary>
        /// The time when the circle has been flagged release.
        /// </summary>
        private float releaseTime;

        private DraggerView dragger;


        public DraggerView Dragger => dragger;

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
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            holdSprite.Alpha = 0f;
            holdSprite.Scale = Vector3.one;
        }

        public void SetDragger(DraggerView dragger)
        {
            RemoveDragger();
            if(dragger == null)
                return;

            this.dragger = dragger;
            SetParent(dragger);
        }

        public void RemoveDragger()
        {
            if(dragger == null)
                return;

            SetParent(dragger.Parent);
            dragger = null;
        }

        /// <summary>
        /// Sets whether dragger is holding.
        /// </summary>
        public void SetHold(bool holding, float curTime)
        {
            // Held down
            if (holding && wasHolding)
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

        /// <summary>
        /// Returns whether the circle is currently being held down.
        /// Optionally provide curTime to check with a more generous release time.
        /// </summary>
        public bool IsHolding(float? curTime = null)
        {
            if(!curTime.HasValue)
                return isHolding;
            return isHolding && curTime.Value < releaseTime + BonusReleaseTime;
        }

        public override JudgementResult JudgeInput(float curTime, IInput input)
        {
            JudgementResult result = base.JudgeInput(curTime, input);

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

        public override void HardDispose()
        {
            base.HardDispose();
            RemoveDragger();
        }
    }
}