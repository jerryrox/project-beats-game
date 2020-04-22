using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Configurations.Settings;
using PBFramework.UI;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.SettingsMenu.Navbars
{
    public class NavBar : UguiGrid {

        /// <summary>
        /// Event called on settings tab button press.
        /// </summary>
        public event Action<SettingsTab> OnTabFocused;

        private List<NavTab> tabs = new List<NavTab>();


        [InitWithDependency]
        private void Init()
        {
            Corner = GridLayoutGroup.Corner.UpperLeft;
            Axis = GridLayoutGroup.Axis.Vertical;
            Alignment = TextAnchor.UpperLeft;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            for (int i = 0; i < tabs.Count; i++)
                tabs[i].Destroy();
            tabs.Clear();
        }

        /// <summary>
        /// Sets the settings data to build nav tabs based on.
        /// </summary>
        public void SetSettingsData(ISettingsData data)
        {
            InvokeAfterFrames(1, () =>
            {
                CellSize = new Vector2(Width, Height / data.TabCount);

                foreach (var tabData in data.GetTabs())
                {
                    var tabButton = CreateChild<NavTab>("tab-" + tabData.Name, tabs.Count);
                    tabButton.SetTabData(tabData);
                    tabs.Add(tabButton);

                    tabButton.OnTriggered += () =>
                    {
                        OnTabFocused?.Invoke(tabData);
                    };
                }
            });
        }

        /// <summary>
        /// Shows focus state on the nav tab of specified data.
        /// </summary>
        public void ShowFocusOnTab(SettingsTab tab)
        {
            for (int i = 0; i < tabs.Count; i++)
                tabs[i].IsFocused = tabs[i].TabData == tab;
        }
    }
}