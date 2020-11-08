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

        private ISprite glowSprite;
        private ISprite fillSprite;

        private IAnime repeatAni;


        IRecycler<SecondaryTouchEffects> IRecyclable<SecondaryTouchEffects>.Recycler { get; set; }

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init()
        {
            glowSprite = CreateChild<UguiSprite>("glow");
            {
                glowSprite.Size = new Vector2(108f, 108f);
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
                .AddTime(0f, new Vector2(140f, 140f), EaseType.CubicEaseOut)
                .AddTime(0.15f, new Vector2(80f, 80f))
                .Build();

            hideAni = new Anime();
            hideAni.AnimateFloat((alpha) => glowSprite.Alpha = alpha)
                .AddTime(0f, () => glowSprite.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
            hideAni.AnimateFloat((alpha) => fillSprite.Alpha = alpha)
                .AddTime(0f, () => fillSprite.Alpha)
                .AddTime(0.25f, 0f)
                .Build();

            repeatAni = new Anime();
            repeatAni.AnimateFloat((alpha) => fillSprite.Alpha = alpha)
                .AddTime(0f, 0.25f, EaseType.QuadEaseOut)
                .AddTime(0.4f, 0.75f, EaseType.QuadEaseIn)
                .AddTime(0.8f, 0.25f)
                .Build();
        }
    }
}