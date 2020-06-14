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

        /// <summary>
        /// Amount of padding to apply on scrolling to a specified tab on request.
        /// </summary>
        private const float ScrollPadding = 50f;

        /// <summary>
        /// Event called when the group representing a tab is within focus of the view.
        /// </summary>
        public event Action<SettingsTab> OnTabFocus;

        private List<ContentGroup> groups = new List<ContentGroup>();
        private IScrollBar scrollBar;

        private ISettingsData settingsData;
        private SettingsTab focusedTab;

        private float scrollviewHeight;
        private float containerHeight;
        private float maxScrollPos;


        [InitWithDependency]
        private void Init()
        {
            background.Alpha = 0f;

            scrollBar = CreateChild<UguiScrollBar>("scrollbar", 1);
            {
                scrollBar.Anchor = AnchorType.RightStretch;
                scrollBar.Pivot = PivotType.Right;
                scrollBar.SetOffsetVertical(0f);
                scrollBar.X = 0f;
                scrollBar.Width = 2f;
                scrollBar.Direction = Scrollbar.Direction.BottomToTop;
            }

            VerticalScrollbar = scrollBar;
        }

        /// <summary>
        /// Sets the settings data to display content for.
        /// </summary>
        public void SetSettingsData(ISettingsData settingsData)
        {
            if (settingsData == null)
            {
                Cleanup();
                return;
            }
            else if (this.settingsData == settingsData)
                return;

            Cleanup();
            this.settingsData = settingsData;

            InvokeAfterTransformed(1, () =>
            {
                scrollviewHeight = Height;

                // Create content groups.
                ResetPosition();
                container.Height = containerHeight = 0f;
                foreach (var tab in settingsData.GetTabs())
                {
                    var group = container.CreateChild<ContentGroup>(tab.Name);
                    {
                        group.Anchor = AnchorType.TopStretch;
                        group.Pivot = PivotType.Top;
                        group.Y = -container.Height;
                        group.SetOffsetHorizontal(0f);
                    }
                    group.SetTabData(tab);
                    groups.Add(group);

                    container.Height = (containerHeight += group.Height);
                }

                maxScrollPos = Math.Max(containerHeight - scrollviewHeight, 0f);

                // Cache position progress of each group for tab focus feature.
                if (maxScrollPos > 0f)
                {
                    foreach (var group in groups)
                        group.PositionProgress = -group.Y / containerHeight;
                }
            });
        }

        /// <summary>
        /// Makes the scrollview view on the specified tab.
        /// </summary>
        public void MoveToTab(SettingsTab tabData)
        {
            foreach (var group in groups)
            {
                if (group.TabData == tabData)
                {
                    float padding = (ScrollPadding) * group.PositionProgress;
                    ScrollTo(new Vector2(0f, Math.Min(maxScrollPos, group.PositionProgress * maxScrollPos + padding)));
                    return;
                }
            }
        }

        /// <summary>
        /// Removes previous states and child components for a new settings data.
        /// </summary>
        public void Cleanup()
        {
            for (int i = 0; i < groups.Count; i++)
                groups[i].Destroy();
            groups.Clear();

            settingsData = null;
            focusedTab = null;

            scrollviewHeight = 0f;
            containerHeight = 0f;
            maxScrollPos = 0f;
        }

        /// <summary>
        /// Marks the specified tab dat as focused.
        /// </summary>
        private void FocusOnTab(SettingsTab tabData)
        {
            if (focusedTab != tabData)
            {
                focusedTab = tabData;
                OnTabFocus?.Invoke(tabData);
            }
        }

        protected void Update()
        {
            if (maxScrollPos == 0f)
                return;

            // Find focused tab.
            float curProgress = container.Y / maxScrollPos;
            for (int i = groups.Count - 1; i >= 0; i--)
            {
                var group = groups[i];
                if (curProgress >= group.PositionProgress)
                {
                    FocusOnTab(group.TabData);
                    return;
                }
            }
        }
    }
}