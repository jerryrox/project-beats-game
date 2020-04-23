using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.SettingsMenu.Navbars;
using PBGame.UI.Components.SettingsMenu.Contents;
using PBGame.Animations;
using PBGame.Configurations;
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


        protected override int OverlayDepth => ViewDepths.SettingsMenuOverlay;

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }


        [InitWithDependency]
        private void Init()
        {
            container.Anchor = Anchors.RightStretch;
            container.Pivot = Pivots.TopRight;
            container.X = -16f;
            container.Y = -16f;
            container.RawHeight = -32f;
            container.Width = 480f;

            var bg = container.CreateChild<UguiSprite>("bg", -100);
            {
                bg.Anchor = Anchors.Fill;
                bg.Offset = Offset.Zero;
                bg.Color = new Color(0f, 0f, 0f, 0.5f);
            }
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

            OnEnableInited();
        }

        public void SetSettingsData(ISettingsData data)
        {
            navBar.SetSettingsData(data);
            contentHolder.SetSettingsData(data);
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();
            GameConfiguration.Save();
        }
    }
}