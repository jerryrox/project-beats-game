using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Shaders;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Common
{
    public class BlurDisplay : UguiObject {

        private UguiSprite blurSprite;
        private UguiSprite fallbackSprite;


        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }


        [InitWithDependency]
        private void Init()
        {
            blurSprite = CreateChild<UguiSprite>("blur", 0);
            {
                blurSprite.Anchor = AnchorType.Fill;
                blurSprite.Offset = Offset.Zero;
                blurSprite.SpriteName = "null";
                blurSprite.AddEffect(new BlurShaderEffect());
            }
            fallbackSprite = CreateChild<UguiSprite>("fallback", 1);
            {
                fallbackSprite.Anchor = AnchorType.Fill;
                fallbackSprite.Offset = Offset.Zero;
                fallbackSprite.Color = new Color(0f, 0f, 0f, 0.4f);
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();
            GameConfiguration.UseBlurShader.BindAndTrigger(OnBlurChange);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            GameConfiguration.UseBlurShader.OnValueChanged -= OnBlurChange;
        }

        /// <summary>
        /// Sets up blur sprite or fallback sprite depending on specified flag.
        /// </summary>
        private void SetBlur(bool useBlur)
        {
            blurSprite.Active = useBlur;
            fallbackSprite.Active = !useBlur;
        }

        /// <summary>
        /// Event called when blur settings have changed.
        /// </summary>
        private void OnBlurChange(bool useBlur, bool _) => SetBlur(useBlur);
    }
}