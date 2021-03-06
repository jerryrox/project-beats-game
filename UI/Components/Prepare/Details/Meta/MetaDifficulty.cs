using System;
using System.Linq;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Difficulty;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Prepare.Details.Meta
{
    public class MetaDifficulty : UguiObject {

        private ILabel label;
        private IGrid grid;
        private MetaDifficultyBlock bpmInfo;
        private MetaDifficultyInfo difficultyInfo;
        private List<MetaDifficultyScale> scales = new List<MetaDifficultyScale>();
        private MetaDifficultyScale difficultyScale;


        [ReceivesDependency]
        private PrepareModel Model { get; set; }

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init()
        {
            label = CreateChild<Label>("label", 0);
            {
                label.Anchor = AnchorType.TopLeft;
                label.Pivot = PivotType.TopLeft;
                label.X = 32f;
                label.Y = -32f;
                label.FontSize = 18;
                label.Alignment = TextAnchor.UpperLeft;
                label.IsBold = true;

                label.Text = "Difficulty";
            }
            grid = CreateChild<UguiGrid>("grid", 1);
            {
                grid.Anchor = AnchorType.Fill;
                grid.Offset = new Offset(32f, 64f, 32f, 32f);
                grid.Axis = GridLayoutGroup.Axis.Vertical;
                grid.CellSize = new Vector2(236f, 36f);

                bpmInfo = grid.CreateChild<MetaDifficultyBlock>("bpm", -1);
                {
                    bpmInfo.Tint = ColorPreset.SecondaryFocus;
                }
                difficultyInfo = grid.CreateChild<MetaDifficultyInfo>("diff", 0);
                difficultyScale = grid.CreateChild<MetaDifficultyScale>("scale", 100);
                {
                    difficultyScale.Tint = ColorPreset.PrimaryFocus;
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
        /// Displays difficulty info scales.
        /// </summary>
        private void SetupDifficulty(IPlayableMap map, DifficultyInfo difficulty)
        {
            var mapDifficulty = map.Detail.Difficulty;

            switch (map.PlayableMode)
            {
                // TODO: Handle other modes that should display different label values.
                
                default:
                    GetCell().Setup("AR", mapDifficulty.ApproachRate, 10f);
                    GetCell().Setup("CS", mapDifficulty.CircleSize, 10f);
                    GetCell().Setup("HP", mapDifficulty.HpDrainRate, 10f);
                    GetCell().Setup("OD", mapDifficulty.OverallDifficulty, 10f);
                    break;
            }

            // Display BPM value
            bpmInfo.Setup("BPM", map.ControlPoints.CommonBpm.ToString("N0"));

            // Display overall scale
            difficultyScale.Setup("Diff. scale", difficulty.Scale, 10f);
            difficultyScale.Active = true;
        }

        /// <summary>
        /// Returns the next available info cell, or creates a new one.
        /// </summary>
        private MetaDifficultyScale GetCell()
        {
            var cell = scales.FirstOrDefault(c => !c.Active);
            if (cell != null)
            {
                cell.Active = true;
                return cell;
            }

            cell = grid.CreateChild<MetaDifficultyScale>($"cell{scales.Count}", scales.Count + 1);
            cell.Tint = ColorPreset.SecondaryFocus;
            scales.Add(cell);
            return cell;
        }

        /// <summary>
        /// Deactivates all dynamic info cells.
        /// </summary>
        private void ClearCells()
        {
            foreach(var s in scales)
                s.Active = false;
        }

        /// <summary>
        /// Event called on map selection change.
        /// </summary>
        private void OnMapChange(IPlayableMap map)
        {
            ClearCells();
            if (map == null)
            {
                difficultyScale.Active = false;
            }
            else
            {
                var difficulty = map.Difficulty;
                if (difficulty == null)
                    throw new ArgumentException($"Missing DifficultyInfo for specified map ({map.ToString()}). Perhaps it is not a playable map?");

                SetupDifficulty(map, difficulty);
            }
        }
    }
}