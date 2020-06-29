using System;
using System.Collections;
using System.Collections.Generic;
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
        private DownloadState State { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            OnTriggered += () =>
            {
                State.ApiProvider.Value = provider;
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
            BindEvents();
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            UnbindEvents();
        }

        /// <summary>
        /// Sets the provider type to represent.
        /// </summary>
        public void SetProvider(ApiProviderType provider)
        {
            this.provider = provider;

            var api = Api.GetProvider(provider);
            iconSprite.SpriteName = api.IconName;
            
            RefreshFocus();
        }

        /// <summary>
        /// Binds to external dependency events.
        /// </summary>
        private void BindEvents()
        {
            State.ApiProvider.BindAndTrigger(OnProviderChange);
        }
        
        /// <summary>
        /// Unbinds from external dependency events.
        /// </summary>
        private void UnbindEvents()
        {
            State.ApiProvider.OnNewValue -= OnProviderChange;
        }

        /// <summary>
        /// Refreshes the button's focus state.
        /// </summary>
        private void RefreshFocus()
        {
            IsFocused = this.provider == State.ApiProvider.Value;
        }

        /// <summary>
        /// Event called on api provider change.
        /// </summary>
        private void OnProviderChange(ApiProviderType provider) => RefreshFocus();
    }
}