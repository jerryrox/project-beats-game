using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Configurations.Settings;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.SettingsMenu.Navbars
{
    public class NavBar : UguiGrid {

        [InitWithDependency]
        private void Init()
        {
            Corner = GridLayoutGroup.Corner.UpperLeft;
            Axis = GridLayoutGroup.Axis.Horizontal;
            Alignment = TextAnchor.UpperLeft;
        }

        /// <summary>
        /// Sets the settings data to build nav tabs based on.
        /// </summary>
        public void SetSettings(ISettingsData data)
        {
            InvokeAfterFrames(1, () =>
            {
                CellSize = new Vector2(72f, Height / data.TabCount);

                foreach (var tabData in data.GetTabs())
                {
                    var tabButton = CreateChild<NavTab>("tab-" + tabData.Name);
                    
                }
            });
        }
    }
}