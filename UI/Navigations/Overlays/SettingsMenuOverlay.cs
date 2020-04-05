using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Animations;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Navigations.Overlays
{
    public class SettingsMenuOverlay : BaseSubMenuOverlay, ISettingsMenuOverlay {

        private ISprite glow;

        protected override int OverlayDepth => ViewDepths.SettingsMenuOverlay;

        [InitWithDependency]
        private void Init()
        {
            container.Anchor = Anchors.RightStretch;
            container.Pivot = Pivots.TopRight;
            container.X = -16f;
            container.Y = -16f;
            container.RawHeight = -32f;
            container.Width = 480f;

            glow = container.CreateChild<UguiSprite>("glow", -1);
            {
                glow.Anchor = Anchors.Fill;
                glow.RawSize = new Vector3(30f, 30f);
                glow.Position = Vector2.zero;
                glow.SpriteName = "square-32-glow";
                glow.ImageType = Image.Type.Sliced;
                glow.Color = Color.black;
            }

            hoverAni = new Anime();
            hoverAni.AnimateColor(color => glow.Color = color)
                .AddTime(0f, () => glow.Color)
                .AddTime(0.25f, Color.gray)
                .Build();

            outAni = new Anime();
            outAni.AnimateColor(color => glow.Color = color)
                .AddTime(0f, () => glow.Color)
                .AddTime(0.25f, Color.black)
                .Build();

            OnEnableInited();
        }

        protected override IAnime CreateShowAnime(IDependencyContainer dependencies)
        {
            return dependencies.Get<IAnimePreset>().GetSubMenuOverlayPopupShow(this, () => container);
        }

        protected override IAnime CreateHideAnime(IDependencyContainer dependencies)
        {
            return dependencies.Get<IAnimePreset>().GetSubMenuOverlayPopupHide(this, () => container);
        }
    }
}