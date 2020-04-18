using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details
{
    public class VersionDisplay : UguiSprite {

        private IBoxIconTrigger backButton;
        private IBoxIconTrigger nextButton;
        private IVersionButton versionIcon;
        private ILabel nameLabel;
        private ILabel scaleLabel;


        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Alpha = 0.125f;

            backButton = CreateChild<BoxIconTrigger>("back", 0);
            {
                backButton.Anchor = Anchors.LeftStretch;
                backButton.Pivot = Pivots.Left;
                backButton.RawHeight = 0f;
                backButton.Width = 80f;
                backButton.X = 0f;
                backButton.IconName = "icon-left";

                backButton.OnTriggered += () => SelectMap(-1);
            }
            nextButton = CreateChild<BoxIconTrigger>("next", 1);
            {
                nextButton.Anchor = Anchors.RightStretch;
                nextButton.Pivot = Pivots.Right;
                nextButton.RawHeight = 0f;
                nextButton.Width = 80f;
                nextButton.X = 0f;
                nextButton.IconName = "icon-right";

                nextButton.OnTriggered += () => SelectMap(1);
            }
            versionIcon = CreateChild<VersionButton>("version", 2);
            {
                versionIcon.Anchor = Anchors.Left;
                versionIcon.Pivot = Pivots.Left;
                versionIcon.X = 88f;
                versionIcon.Size = new Vector2(72, 72);

                versionIcon.IsInteractible = false;
            }
            nameLabel = CreateChild<Label>("title", 3);
            {
                nameLabel.Anchor = Anchors.TopStretch;
                nameLabel.OffsetLeft = 176f;
                nameLabel.OffsetRight = 80f;
                nameLabel.Y = -26f;
                nameLabel.Height = 30f;
                nameLabel.Alignment = TextAnchor.UpperLeft;
            }
            scaleLabel = CreateChild<Label>("scale", 4);
            {
                scaleLabel.Anchor = Anchors.BottomStretch;
                scaleLabel.OffsetLeft = 176f;
                scaleLabel.OffsetRight = 80f;
                scaleLabel.Y = 26f;
                scaleLabel.Height = 30f;
                scaleLabel.IsBold = true;
                scaleLabel.FontSize = 22;
                scaleLabel.Alignment = TextAnchor.LowerLeft;
            }

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
        /// Binds to external dependency events.
        /// </summary>
        private void BindEvents()
        {
            MapSelection.OnMapChange += OnMapChange;

            RefreshDisplays();
        }
        
        /// <summary>
        /// Unbinds from external dependency events.
        /// </summary>
        private void UnbindEvents()
        {
            MapSelection.OnMapChange -= OnMapChange;
        }

        /// <summary>
        /// Selects the map within the selected mapset after or before current selection by specified offset.
        /// </summary>
        private void SelectMap(int offset)
        {
            var maps = MapSelection.Mapset.Maps;
            var curMap = MapSelection.Map.OriginalMap;

            // Determin the index
            int index = maps.IndexOf(curMap) + offset;
            // Select it only if valid.
            if(index >= 0 && index < maps.Count)
                MapSelection.SelectMap(maps[index]);
        }

        /// <summary>
        /// Refreshes version icon and labels.
        /// </summary>
        private void RefreshDisplays()
        {
            var map = MapSelection.Map;

            versionIcon.Setup(map);
            nameLabel.Text = map.Detail.Version;
            scaleLabel.Text = map.Difficulty.Scale.ToString("N2");
        }

        /// <summary>
        /// Event called on map selection change.
        /// </summary>
        private void OnMapChange(IPlayableMap map) => RefreshDisplays();

        /// <summary>
        /// Event called on unicode preference change.
        /// </summary>
        private void OnUnicodeChange(bool preferUnicode, bool _) => RefreshDisplays();
    }
}