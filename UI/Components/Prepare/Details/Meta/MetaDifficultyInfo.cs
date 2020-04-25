using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Maps;
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
        private IMapSelection MapSelection { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            Color = new Color(1f, 1f, 1f, 0.0625f);

            grid = CreateChild<UguiGrid>("grid");
            {
                grid.Anchor = Anchors.Fill;
                grid.RawSize = Vector2.zero;
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
            OnMapChange(MapSelection.Map);
        }
        
        /// <summary>
        /// Unbinds from external dependency events.
        /// </summary>
        private void UnbindEvents()
        {
            MapSelection.OnMapChange -= OnMapChange;
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