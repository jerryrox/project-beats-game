using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Judgements;
using PBGame.Rulesets.Beats.Standard.Objects;
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
    public class HitCircleView : HitObjectView<HitCircle>, IRecyclable<HitCircleView> {

        protected ISprite outerGlowSprite;
        protected ISprite bgSprite;
        protected ISprite dotSprite;
        protected ISprite glowSprite;

        protected IAnime hitAni;


        IRecycler<HitCircleView> IRecyclable<HitCircleView>.Recycler { get; set; }


        [InitWithDependency]
        private void Init()
        {
            outerGlowSprite = CreateChild<UguiSprite>("outer-glow", 0);
            {
                outerGlowSprite.Anchor = AnchorType.Fill;
                outerGlowSprite.Offset = new Offset(-64f);
                outerGlowSprite.SpriteName = "glow-circle-64";
            }
            bgSprite = CreateChild<UguiSprite>("bg-sprite", 1);
            {
                bgSprite.Anchor = AnchorType.Fill;
                bgSprite.Offset = Offset.Zero;
                bgSprite.SpriteName = "circle-128";
                bgSprite.Color = Color.black;
            }
            dotSprite = CreateChild<UguiSprite>("dot", 2);
            {
                dotSprite.Size = new Vector2(64f, 64f);
                dotSprite.SpriteName = "circle-64";
            }
            glowSprite = CreateChild<UguiSprite>("glow", 3);
            {
                glowSprite.Anchor = AnchorType.Fill;
                glowSprite.Offset = new Offset(-30f);
                glowSprite.SpriteName = "glow-128";
            }

            hitAni = new Anime();
            hitAni.AnimateVector3(s => this.Scale = s)
                .AddTime(0f, Vector3.one, EaseType.CubicEaseOut)
                .AddTime(0.5f, new Vector3(2f, 2f, 2f))
                .Build();
            hitAni.AnimateFloat(a =>
            {
                bgSprite.Alpha = a * a;
                this.Alpha = a;
            })
                .AddTime(0f, 1f, EaseType.QuadEaseOut)
                .AddTime(0.5f, 0f)
                .Build();
            hitAni.AddEvent(hitAni.Duration, () => Active = false);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            hitAni.Stop();
        }

        public override JudgementResult JudgeInput(float curTime, IInput input)
        {
            if(IsJudged)
                return null;

            if (input.State.Value == InputState.Press)
            {
                float offset = GetHitOffset(curTime);
                var resultType = hitObject.Timing.GetHitResult(offset);
                if (resultType != HitResultType.None)
                {
                    hitAni.PlayFromStart();
                    return SetResult(resultType, offset);
                }
            }
            return null;
        }

        public override void SoftDispose()
        {
            base.SoftDispose();
            // TODO:
        }

        public override void HardDispose()
        {
            base.HardDispose();
            // TODO:
        }
    }
}