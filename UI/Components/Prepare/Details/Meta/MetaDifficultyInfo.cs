using PBGame.UI.Models;
using PBGame.Rulesets.Maps;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details.Meta
{
    public class MetaDifficultyInfo : UguiSprite {

        private IGrid grid;
        private MetaDifficultyInfoCell timeInfo;
        private MetaDifficultyInfoCell objectsInfo;


        [ReceivesDependency]
        private PrepareModel Model { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            Color = new Color(1f, 1f, 1f, 0.0625f);

            grid = CreateChild<UguiGrid>("grid");
            {
                grid.Anchor = AnchorType.Fill;
                grid.Offset = Offset.Zero;
                grid.CellSize = new Vector2(118f, 36f);

                timeInfo = grid.CreateChild<MetaDifficultyInfoCell>("time", 0);
                {
                    timeInfo.IconName = "icon-time";
                    timeInfo.Tint = colorPreset.SecondaryFocus;
                }
                objectsInfo = grid.CreateChild<MetaDifficultyInfoCell>("objects", 1);
                {
                    objectsInfo.IconName = "icon-mode-osu-32";
                    objectsInfo.Tint = colorPreset.SecondaryFocus;
                }
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
        /// Event called on map selection change.
        /// </summary>
        private void OnMapChange(IPlayableMap map)
        {
            if (map == null)
            {
                timeInfo.LabelText = "0:00";
                objectsInfo.LabelText = "0";
            }
            else
            {
                int duration = map.Duration / 1000;
                int minutes = duration / 60;
                int seconds = duration % 60;
                timeInfo.LabelText = $"{minutes}:{seconds.ToString("00")}";
                objectsInfo.LabelText = map.ObjectCount.ToString("N0");
            }
        }
    }
}