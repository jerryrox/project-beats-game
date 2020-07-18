using PBGame.UI.Models;
using PBGame.UI.Components.SettingsMenu.Navbars;
using PBGame.UI.Components.SettingsMenu.Contents;
using PBGame.Configurations.Settings;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class SettingsMenuOverlay : BaseSubMenuOverlay<SettingsModel>, ISettingsMenuOverlay {

        private NavBar navBar;
        private ContentHolder contentHolder;


        protected override int ViewDepth => ViewDepths.SettingsMenuOverlay;


        [InitWithDependency]
        private void Init()
        {
            container.Anchor = AnchorType.RightStretch;
            container.Pivot = PivotType.TopRight;
            container.X = -16f;
            container.Y = -16f;
            container.RawHeight = -32f;
            container.Width = 480f;

            var bg = container.CreateChild<UguiSprite>("bg", -100);
            {
                bg.Anchor = AnchorType.Fill;
                bg.Offset = Offset.Zero;
                bg.Color = new Color(0f, 0f, 0f, 0.5f);
            }
            navBar = container.CreateChild<NavBar>("navBar", 1);
            {
                navBar.Anchor = AnchorType.RightStretch;
                navBar.Pivot = PivotType.Right;
                navBar.Width = 72f;
                navBar.RawHeight = 0f;
                navBar.Position = Vector2.zero;

                navBar.OnTabFocused += (tabData) => contentHolder.MoveToTab(tabData);
            }
            contentHolder = container.CreateChild<ContentHolder>("content", 0);
            {
                contentHolder.Anchor = AnchorType.Fill;
                contentHolder.Offset = new Offset(0f, 0f, 72f, 0f);

                contentHolder.OnTabFocus += (tabData) => navBar.ShowFocusOnTab(tabData);
            }

            OnEnableInited();
        }

        /// <summary>
        /// Displays the specified settings data visually.
        /// </summary>
        public void SetSettingsData(ISettingsData data)
        {
            navBar.SetSettingsData(data);
            contentHolder.SetSettingsData(data);
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();
            model.CurrentSettings.BindAndTrigger(OnSettingsDataChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            model.CurrentSettings.OnNewValue -= OnSettingsDataChange;
        }

        /// <summary>
        /// Event called when the settings data has changed.
        /// </summary>
        private void OnSettingsDataChange(ISettingsData data) => SetSettingsData(data);
    }
}