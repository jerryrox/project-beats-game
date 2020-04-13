using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Graphics;
using PBGame.Configurations.Settings;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.SettingsMenu.Navbars
{
    public class NavTab : BoxIconTrigger {

        private ISprite highlightSprite;
        private ISprite glowSprite;
        private ISprite focusSprite;

        private SettingsTab tabData;


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            iconSprite.Size = new Vector2(36f, 36f);

            highlightSprite = CreateChild<UguiSprite>("highlight", 1);
            {
                highlightSprite.Anchor = Anchors.Fill;
                highlightSprite.RawSize = Vector2.zero;
                highlightSprite.Alpha = 0f;
            }
            glowSprite = CreateChild<UguiSprite>("glow", 3);
            {
                glowSprite.Size = new Vector2(96f, 96f);
                glowSprite.Color = colorPreset.PrimaryFocus;
                glowSprite.SpriteName = "glow-128";
                glowSprite.Alpha = 0;
            }
            focusSprite = CreateChild<UguiSprite>("focus", 4);
            {
                focusSprite.Anchor = Anchors.RightStretch;
                focusSprite.Pivot = Pivots.Right;
                focusSprite.Width = 3f;
                focusSprite.RawHeight = 0f;
                focusSprite.Position = Vector3.zero;
                focusSprite.Color = colorPreset.PrimaryFocus;
                focusSprite.Alpha = 0.25f;
            }
        }

        /// <summary>
        /// Sets the tab data this object will represent.
        /// </summary>
        public void SetTabData(SettingsTab tabData)
        {
            this.tabData = tabData;
            iconSprite.SpriteName = tabData.IconName;
        }
    }
}