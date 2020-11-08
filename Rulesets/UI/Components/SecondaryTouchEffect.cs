using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Shaders;
using PBFramework.Allocation.Recyclers;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.UI.Components
{
    public class SecondaryTouchEffects : TouchEffect, IRecyclable<SecondaryTouchEffects> {

        private const float GlowSizeOffset = 28f;
        private const float BaseSize = 150;
        private const float ShowShrinkSize = 80;

        private ISprite glowSprite;
        private ISprite fillSprite;

        private IAnime repeatAni;


        public IRecycler<SecondaryTouchEffects> Recycler { get; set; }

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init()
        {
            glowSprite = CreateChild<UguiSprite>("glow");
            {
                glowSprite.Size = new Vector2(BaseSize + GlowSizeOffset, BaseSize + GlowSizeOffset);
                glowSprite.SpriteName = "glow-square-32";
                glowSprite.ImageType = Image.Type.Sliced;
                glowSprite.Color = ColorPreset.SecondaryFocus;
                glowSprite.RotationZ = 45f;
                glowSprite.IsRaycastTarget = false;
            }
            fillSprite = CreateChild<UguiSprite>("fill");
            {
                fillSprite.Anchor = AnchorType.Fill;
                fillSprite.Offset = Offset.Zero;
                fillSprite.Color = ColorPreset.SecondaryFocus;
                fillSprite.RotationZ = 45f;
                fillSprite.IsRaycastTarget = false;

                fillSprite.AddEffect(new AdditiveShaderEffect());
            }

            showAni = new Anime();
            showAni.AnimateFloat((alpha) => glowSprite.Alpha = alpha)
                .AddTime(0f, 0f)
                .AddTime(0.05f, 0f, EaseType.QuadEaseOut)
                .AddTime(0.15f, 1f)
                .Build();
            showAni.AnimateFloat((alpha) => fillSprite.Alpha = alpha)
                .AddTime(0f, 0.85f, EaseType.QuadEaseOut)
                .AddTime(0.15f, 0.25f)
                .Build();
            showAni.AnimateVector2((size) => fillSprite.Size = size)
                .AddTime(0f, new Vector2(BaseSize + ShowShrinkSize, BaseSize + ShowShrinkSize), EaseType.CubicEaseOut)
                .AddTime(0.15f, new Vector2(BaseSize, BaseSize))
                .Build();
            showAni.AddEvent(showAni.Duration, () => repeatAni.PlayFromStart());

            hideAni = new Anime();
            hideAni.AddEvent(0f, () => repeatAni.Pause());
            hideAni.AnimateFloat((alpha) => glowSprite.Alpha = alpha)
                .AddTime(0f, () => glowSprite.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
            hideAni.AnimateFloat((alpha) => fillSprite.Alpha = alpha)
                .AddTime(0f, () => fillSprite.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
            hideAni.AddEvent(hideAni.Duration, () => Recycler.Return(this));

            repeatAni = new Anime()
            {
                WrapMode = WrapModeType.Loop,
            };
            repeatAni.AnimateFloat((alpha) => fillSprite.Alpha = alpha)
                .AddTime(0f, 0.25f, EaseType.QuadEaseOut)
                .AddTime(0.4f, 0.75f, EaseType.QuadEaseIn)
                .AddTime(0.8f, 0.25f)
                .Build();
        }
    }
}