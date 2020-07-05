using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBGame.Networking.API;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download.Search
{
    public class ProviderButton : HighlightableTrigger {

        private ApiProviderType provider;


        [ReceivesDependency]
        private IApi Api { get; set; }

        [ReceivesDependency]
        private DownloadModel Model { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            OnTriggered += () =>
            {
                Model.Options.ApiProvider.Value = provider;
            };

            highlightSprite.Color = colorPreset.SecondaryFocus;

            CreateIconSprite();

            InvokeAfterTransformed(2, () =>
            {
                float size = Mathf.Min(this.Width, this.Height) - 16;
                iconSprite.Size = new Vector2(size, size);
                iconSprite.Position = Vector2.zero;
            });

            UseDefaultFocusAni();
            UseDefaultHoverAni();
            UseDefaultHighlightAni();

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();
            
            Model.Options.ApiProvider.BindAndTrigger(OnProviderChange);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            
            Model.Options.ApiProvider.OnNewValue -= OnProviderChange;
        }

        /// <summary>
        /// Sets the provider type to represent.
        /// </summary>
        public void SetProvider(ApiProviderType provider)
        {
            this.provider = provider;
            iconSprite.SpriteName = Model.GetProviderIcon(provider);
            
            RefreshFocus();
        }

        /// <summary>
        /// Refreshes the button's focus state.
        /// </summary>
        private void RefreshFocus()
        {
            IsFocused = (this.provider == Model.Options.ApiProvider.Value);
        }

        /// <summary>
        /// Event called on api provider change.
        /// </summary>
        private void OnProviderChange(ApiProviderType provider) => RefreshFocus();
    }
}