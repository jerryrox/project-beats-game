using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Configurations.Settings;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.SettingsMenu.Contents
{
    public class ContentHolder : UguiScrollView {

        private List<ContentGroup> groups = new List<ContentGroup>();
        private IScrollBar scrollBar;

        private ISettingsData settingsData;


        [InitWithDependency]
        private void Init()
        {
            background.Alpha = 0f;

            scrollBar = CreateChild<UguiScrollBar>("scrollbar", 1);
            {
                scrollBar.Anchor = Anchors.RightStretch;
                scrollBar.Pivot = Pivots.Right;
                scrollBar.SetOffsetVertical(0f);
                scrollBar.X = 0f;
                scrollBar.Width = 2f;
                scrollBar.Direction = Scrollbar.Direction.BottomToTop;
            }

            VerticalScrollbar = scrollBar;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            for (int i = 0; i < groups.Count; i++)
                groups[i].Destroy();
            groups = null;
            settingsData = null;
        }

        /// <summary>
        /// Sets the settings data to display content for.
        /// </summary>
        public void SetSettingsData(ISettingsData settingsData)
        {
            this.settingsData = settingsData;

            // Create content groups.
            container.Height = 0f;
            foreach (var tab in settingsData.GetTabs())
            {
                var group = container.CreateChild<ContentGroup>(tab.Name);
                {
                    group.Anchor = Anchors.TopStretch;
                    group.Pivot = Pivots.Top;
                    group.Y = -container.Height;
                    group.SetOffsetHorizontal(0f);
                }
                group.SetTabData(tab);
                groups.Add(group);

                container.Height += group.Height;
            }
        }
    }
}