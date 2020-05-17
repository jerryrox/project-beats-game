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


        [InitWithDependency]
        private void Init()
        {
            tickSprite = CreateChild<UguiSprite>("tick", 0);
            {
                tickSprite.SpriteName = "circle-32";
                tickSprite.Size = new Vector2(32f, 32f);
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

        public override bool IsPastJudgeEnd(float curTime)
        {
            return curTime > hitObject.StartTime;
        }

        public override void HardDispose()
        {
            base.HardDispose();
            RemoveDragger();
        }

        protected override void EvalPassiveJudgement()
        {
            if (draggerView != null && draggerView.StartCircle != null)
            {
                SetResult(draggerView.StartCircle.IsHolding() ? HitResultType.Perfect : HitResultType.Miss, 0f);
                if(Result.IsHit)
                    hitAni.PlayFromStart();
                else
                    Active = false;
            }
            else
            {
                SetResult(HitResultType.Miss, 0f);
                Active = false;
            }
        }

        protected void Update()
        {
            this.Y = Mathf.Max(draggerView.StartCircle.Position.y, this.Y);
        }
    }
}