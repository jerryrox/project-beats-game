using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBGame.Configurations.Settings;
using PBFramework.UI;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.SettingsMenu.Navbars
{
    public class NavTab : HighlightableTrigger {

        private ISprite iconSprite;
        private ISprite glowSprite;

        private SettingsTab tabData;


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            iconSprite = CreateChild<UguiSprite>("icon", 10);
            {
                iconSprite.Size = new Vector2(36f, 36f);
                iconSprite.Alpha = 0.65f;
            }
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
            iconSprite.SpriteName = tabData.IconName;
        }
    }
}