using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.ProfileMenu;
using PBGame.Animations;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class ProfileMenuOverlay : BaseSubMenuOverlay, IProfileMenuOverlay {

        private IContentHolder content;


        protected override int OverlayDepth => ViewDepths.ProfileMenuOverlay;


        [InitWithDependency]
        private void Init()
        {
            container.Anchor = Anchors.TopLeft;
            container.Pivot = Pivots.TopLeft;
            container.X = 16f;
            container.Y = -16f;

            content = container.CreateChild<ContentHolder>("content", 0);
            {
                content.Anchor = Anchors.TopLeft;
                content.Pivot = Pivots.TopLeft;
                content.Position = Vector2.zero;
                content.Width = 320f;
            }

            hoverAni = new Anime();
            hoverAni.AnimateColor(color => content.GlowSprite.Color = color)
                .AddTime(0f, () => content.GlowSprite.Color)
                .AddTime(0.25f, Color.gray)
                .Build();

            outAni = new Anime();
            outAni.AnimateColor(color => content.GlowSprite.Color = color)
                .AddTime(0f, () => content.GlowSprite.Color)
                .AddTime(0.25f, Color.black)
                .Build();
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