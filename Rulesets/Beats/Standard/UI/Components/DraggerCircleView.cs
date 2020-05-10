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
            }
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
            // TODO: Animation feedback

            // Held down
            if (holding && wasHolding)
            {
                isHolding = true;
            }
            // Released
            else if(!holding && wasHolding)
            {
                isHolding = false;
                releaseTime = curTime;
            }

            wasHolding = isHolding;
        }

        /// <summary>
        /// Returns whether the circle is currently being held down for specified time.
        /// </summary>
        public bool IsHolding(float curTime)
        {
            return isHolding && curTime < releaseTime + BonusReleaseTime;
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