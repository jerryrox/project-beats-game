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

        private DraggerView dragger;


        public DraggerView Dragger => dragger;

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
                .AddTime(0.5f, new Vector3(2f, 2f, 2f))
                .Build();
            hitAni.AddEvent(hitAni.Duration, () => Active = false);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            hitAni.Stop();
        }

        // Ticks shouldn't be judged by direct input.
        public override JudgementResult JudgeInput(float curTime, IInput input) => null;

        /// <summary>
        /// Sets the parent dragger view.
        /// Specifying a null dragger should be equivalent to calling RemoveDragger.
        /// </summary>
        public void SetDragger(DraggerView dragger)
        {
            RemoveDragger();
            if(dragger == null)
                return;

            this.dragger = dragger;
            SetParent(dragger);
        }

        /// <summary>
        /// Disassociates this object from parent dragger.
        /// </summary>
        public void RemoveDragger()
        {
            if(dragger == null)
                return;

            SetParent(dragger.Parent);
            dragger = null;
        }

        protected override void EvalPassiveJudgement()
        {
            if (dragger != null && dragger.StartCircle != null)
                SetResult(dragger.StartCircle.IsHolding() ? HitResultType.Perfect : HitResultType.Miss, 0f);
            else
                base.EvalPassiveJudgement();
        }
    }
}