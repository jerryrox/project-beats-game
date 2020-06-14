using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBGame.Configurations.Settings;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.SettingsMenu.Navbars
{
    public class NavTab : HighlightableTrigger {

        private ISprite glowSprite;

        private SettingsTab tabData;


        /// <summary>
        /// Returns the tab data represented by this object.
        /// </summary>
        public SettingsTab TabData => tabData;

        protected override AnchorType HighlightSpriteAnchor => AnchorType.RightStretch;


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            CreateIconSprite(depth: 10);
            
            glowSprite = CreateChild<UguiSprite>("glow", 3);
            {
                glowSprite.Size = new Vector2(96f, 96f);
                glowSprite.Color = colorPreset.PrimaryFocus;
                glowSprite.SpriteName = "glow-128";
                glowSprite.Alpha = 0;
            }

            UseDefaultFocusAni();
            UseDefaultHighlightAni();
            UseDefaultHoverAni();

            highlightAni.AnimateFloat(a => glowSprite.Alpha = a)
                .AddTime(0f, () => glowSprite.Alpha)
                .AddTime(0.25f, 1f)
                .Build();
            unhighlightAni.AnimateFloat(a => glowSprite.Alpha = a)
                .AddTime(0f, () => glowSprite.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
        }

        /// <summary>
        /// Sets the tab data this object will represent.
        /// </summary>
        public void SetTabData(SettingsTab tabData)
        {
            this.tabData = tabData;
            IconName = tabData.IconName;
        }
    }
}