using PBGame.UI.Models;
using PBGame.UI.Components.Offsets;
using PBGame.Configurations.Maps;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class OffsetsOverlay : BaseOverlay<OffsetsModel> {

        private BackgroundDisplay backgroundDisplay;
        private MetronomeContainer metronomeContainer;
        private OffsetSlider mapsetSlider;
        private OffsetSlider mapSlider;


        protected override int ViewDepth => ViewDepths.OffsetOverlay;


        [InitWithDependency]
        private void Init()
        {
            backgroundDisplay = CreateChild<BackgroundDisplay>("background", 0);
            {
                backgroundDisplay.Anchor = AnchorType.Fill;
                backgroundDisplay.Offset = Offset.Zero;
            }
            metronomeContainer = CreateChild<MetronomeContainer>("metronome-container", 1);
            {
                metronomeContainer.Anchor = AnchorType.BottomStretch;
                metronomeContainer.SetOffsetHorizontal(0f);
                metronomeContainer.Y = 140f;
                metronomeContainer.Height = 64f;
            }
            mapsetSlider = CreateChild<OffsetSlider>("mapset", 2);
            {
                mapsetSlider.Anchor = AnchorType.Bottom;
                mapsetSlider.Position = new Vector3(-240f, 56f);
                mapsetSlider.Size = new Vector2(400f, 100f);
                mapsetSlider.LabelText = "Mapset offset";
            }
            mapSlider = CreateChild<OffsetSlider>("map", 3);
            {
                mapSlider.Anchor = AnchorType.Bottom;
                mapSlider.Position = new Vector3(240f, 56f);
                mapSlider.Size = new Vector2(400f, 100f);
                mapSlider.LabelText = "Map offset";
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            model.MapsetConfig.BindAndTrigger(OnMapsetConfigChange);
            model.MapConfig.BindAndTrigger(OnMapConfigChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            model.MapsetConfig.OnNewValue -= OnMapsetConfigChange;
            model.MapConfig.OnNewValue -= OnMapConfigChange;

            mapsetSlider.SetSource(null);
            mapSlider.SetSource(null);
        }

        /// <summary>
        /// Event called when the selected mapset configuration has changed.
        /// </summary>
        private void OnMapsetConfigChange(MapsetConfig config)
        {
            mapsetSlider.SetSource(config);
        }

        /// <summary>
        /// Event called when the selected map configuration has changed.
        /// </summary>
        private void OnMapConfigChange(MapConfig config)
        {
            mapSlider.SetSource(config);
        }
    }
}