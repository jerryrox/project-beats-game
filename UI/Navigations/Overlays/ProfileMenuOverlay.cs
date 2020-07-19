using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Components.ProfileMenu;
using PBGame.Animations;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class ProfileMenuOverlay : BaseSubMenuOverlay<ProfileMenuModel> {

        private ContentHolder content;


        protected override int ViewDepth => ViewDepths.ProfileMenuOverlay;


        [InitWithDependency]
        private void Init()
        {
            container.Anchor = AnchorType.TopLeft;
            container.Pivot = PivotType.TopLeft;
            container.X = 16f;
            container.Y = -16f;

            content = container.CreateChild<ContentHolder>("content", 0);
            {
                content.Anchor = AnchorType.TopLeft;
                content.Pivot = PivotType.TopLeft;
                content.Position = Vector2.zero;
                content.Width = 320f;

                glowSprite.SetParent(content);
                glowSprite.Offset = new Offset(-15f);
            }
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