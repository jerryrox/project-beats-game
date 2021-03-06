using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.Rulesets.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details
{
    public class VersionDisplay : UguiSprite {

        private IconButton backButton;
        private IconButton nextButton;
        private VersionButton versionIcon;
        private ILabel nameLabel;
        private ILabel scaleLabel;


        [ReceivesDependency]
        private PrepareModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Alpha = 0.125f;

            backButton = CreateChild<IconButton>("back", 0);
            {
                backButton.Anchor = AnchorType.LeftStretch;
                backButton.Pivot = PivotType.Left;
                backButton.RawHeight = 0f;
                backButton.Width = 80f;
                backButton.X = 0f;
                backButton.IconName = "icon-left";
                
                backButton.OnTriggered += Model.SelectPrevMap;
            }
            nextButton = CreateChild<IconButton>("next", 1);
            {
                nextButton.Anchor = AnchorType.RightStretch;
                nextButton.Pivot = PivotType.Right;
                nextButton.RawHeight = 0f;
                nextButton.Width = 80f;
                nextButton.X = 0f;
                nextButton.IconName = "icon-right";
                
                nextButton.OnTriggered += Model.SelectNextMap;
            }
            versionIcon = CreateChild<VersionButton>("version", 2);
            {
                versionIcon.Anchor = AnchorType.Left;
                versionIcon.Pivot = PivotType.Left;
                versionIcon.X = 88f;
                versionIcon.Size = new Vector2(72, 72);

                versionIcon.IsInteractible = false;
            }
            nameLabel = CreateChild<Label>("title", 3);
            {
                nameLabel.Anchor = AnchorType.TopStretch;
                nameLabel.SetOffsetHorizontal(176f, 80f);
                nameLabel.Y = -26f;
                nameLabel.Height = 30f;
                nameLabel.Alignment = TextAnchor.UpperLeft;
            }
            scaleLabel = CreateChild<Label>("scale", 4);
            {
                scaleLabel.Anchor = AnchorType.BottomStretch;
                scaleLabel.SetOffsetHorizontal(176f, 80f);
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

            Model.SelectedMap.BindAndTrigger(OnMapChange);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            Model.SelectedMap.OnNewValue -= OnMapChange;
        }

        /// <summary>
        /// Refreshes version icon and labels.
        /// </summary>
        private void RefreshDisplays()
        {
            var map = Model.SelectedMap.Value;
            versionIcon.Setup(map);
            nameLabel.Text = map.Detail.Version;
            scaleLabel.Text = map.Difficulty.Scale.ToString("N2");
        }

        /// <summary>
        /// Event called on map selection change.
        /// </summary>
        private void OnMapChange(IPlayableMap map) => RefreshDisplays();
    }
}