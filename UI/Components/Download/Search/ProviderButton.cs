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
        private IApiManager ApiManager { get; set; }

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

            var api = ApiManager.GetApi(provider);
            iconSprite.SpriteName = api.IconName;
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
            State.ApiProvider.OnValueChanged -= OnProviderChange;
        }

        /// <summary>
        /// Event called on api provider change.
        /// </summary>
        private void OnProviderChange(ApiProviderType provider, ApiProviderType _)
        {
            IsFocused = provider == this.provider;
        }
    }
}