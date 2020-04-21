using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.SettingsMenu.Navbars;
using PBGame.UI.Components.SettingsMenu.Contents;
using PBGame.Animations;
using PBGame.Configurations.Settings;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Navigations.Overlays
{
    public class SettingsMenuOverlay : BaseSubMenuOverlay, ISettingsMenuOverlay {

        private NavBar navBar;
        private ContentHolder contentHolder;

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

            navBar = container.CreateChild<NavBar>("navBar", 1);
            {
                navBar.Anchor = Anchors.RightStretch;
                navBar.Pivot = Pivots.Right;
                navBar.Width = 72f;
                navBar.RawHeight = 0f;
                navBar.Position = Vector2.zero;

                navBar.OnTabFocused += (tabData) => contentHolder.MoveToTab(tabData);
            }
            contentHolder = container.CreateChild<ContentHolder>("content", 0);
            {
                contentHolder.Anchor = Anchors.Fill;
                contentHolder.Offset = new Offset(0f, 0f, 72f, 0f);

                contentHolder.OnTabFocus += (tabData) => navBar.ShowFocusOnTab(tabData);
            }
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

        public void SetSettingsData(ISettingsData data)
        {
            navBar.SetSettingsData(data);
            contentHolder.SetSettingsData(data);
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