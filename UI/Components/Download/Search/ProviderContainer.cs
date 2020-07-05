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
    public class ProviderContainer : UguiSprite {

        private const float ProviderButtonWidth = 100f;

        private ILabel titleLabel;
        private ILabel statusLabel;

        private IGrid grid;


        [ReceivesDependency]
        private DownloadModel Model { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            Color = colorPreset.Passive;

            titleLabel = CreateChild<Label>("title", 0);
            {
                titleLabel.Anchor = AnchorType.LeftStretch;
                titleLabel.Pivot = PivotType.Left;
                titleLabel.SetOffsetVertical(0f);
                titleLabel.X = 16f;
                titleLabel.IsBold = true;
                titleLabel.FontSize = 16;
                titleLabel.Alignment = TextAnchor.MiddleLeft;
                titleLabel.Text = "Providers";
            }
            statusLabel = CreateChild<Label>("status", 1);
            {
                statusLabel.Anchor = AnchorType.RightStretch;
                statusLabel.Pivot = PivotType.Right;
                statusLabel.SetOffsetVertical(0f);
                statusLabel.X = -16f;
                statusLabel.IsBold = true;
                statusLabel.FontSize = 16;
                statusLabel.Alignment = TextAnchor.MiddleRight;
            }
            grid = CreateChild<UguiGrid>("grid", 2);
            {
                grid.Anchor = AnchorType.Fill;
                grid.Offset = new Offset(100f, 0f, 200f, 0f);

                foreach (var provider in (ApiProviderType[])Enum.GetValues(typeof(ApiProviderType)))
                {
                    var button = grid.CreateChild<ProviderButton>(provider.ToString(), grid.ChildCount);
                    button.SetProvider(provider);
                }
            }

            InvokeAfterTransformed(1, () =>
            {
                grid.CellSize = new Vector2(ProviderButtonWidth, this.Height);
            });

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
        /// Refreshes status label text.
        /// </summary>
        private void RefreshStatus()
        {
            statusLabel.Text = $"using {Model.SelectedProvider.Name}";
        }

        /// <summary>
        /// Event called on api provider change.
        /// </summary>
        private void OnProviderChange(ApiProviderType provider) => RefreshStatus();
    }
}