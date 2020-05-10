using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Judgements;
using PBGame.Rulesets.Beats.Standard.Objects;
using PBFramework.UI;
using PBFramework.Inputs;
using PBFramework.Graphics;
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
        }

        public override JudgementResult JudgeInput(float curTime, IInput input)
        {
            if(IsJudged)
                return null;

            if (input.State.Value == InputState.Press)
            {
                float offset = GetHitOffset(curTime);
                var resultType = hitObject.Timing.GetHitResult(offset);
                if(resultType != HitResultType.None)
                    return SetResult(resultType, offset);
            }
            return null;
        }
    }
}