using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Data.Rankings;
using PBGame.Rulesets;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details.Ranking
{
    public class RankingContainer : UguiSprite {

        private RankingTabDisplay tabDisplay;
        private RankingColumn column;
        private RankingList rankingList;


        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IModeManager ModeManager { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Alpha = 0.0625f;

            tabDisplay = CreateChild<RankingTabDisplay>("tab", 0);
            {
                tabDisplay.Anchor = AnchorType.TopStretch;
                tabDisplay.Pivot = PivotType.Top;
                tabDisplay.RawWidth = 0f;
                tabDisplay.Height = 0f;
                tabDisplay.Y = 0f;
            }
            column = CreateChild<RankingColumn>("column", 1);
            {
                column.Anchor = AnchorType.TopStretch;
                column.Pivot = PivotType.Top;
                column.Height = 36f;
                column.Y = -52f;
            }
            rankingList = CreateChild<RankingList>("list", 2);
            {
                rankingList.Anchor = AnchorType.Fill;
                rankingList.Offset = new Offset(0f, 88f, 0f, 0f);
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
        /// Starts reloading ranking info cells from appropriate sources.
        /// </summary>
        private void ReloadRankInfos()
        {
            rankingList.Clear();
            
            // TODO: Load rank infos from appropriate sources.
            // rankingList
        }

        /// <summary>
        /// Binds to external dependency events.
        /// </summary>
        private void BindEvents()
        {
            GameConfiguration.RulesetMode.OnValueChanged += OnGameModeChange;
            GameConfiguration.RankDisplay.OnValueChanged += OnRankDisplayChange;

            OnGameModeChange(GameConfiguration.RulesetMode.Value);
        }

        /// <summary>
        /// Unbinds from external dependency events.
        /// </summary>
        private void UnbindEvents()
        {
            GameConfiguration.RulesetMode.OnValueChanged -= OnGameModeChange;
            GameConfiguration.RankDisplay.OnValueChanged -= OnRankDisplayChange;
        }

        /// <summary>
        /// Event called on game mode configuration change.
        /// </summary>
        private void OnGameModeChange(GameModeType newMode, GameModeType oldMode = GameModeType.BeatsStandard)
        {
            column.RefreshColumns(ModeManager.GetService(newMode));
            ReloadRankInfos();
        }

        private void OnRankDisplayChange(RankDisplayType newType, RankDisplayType oldType = RankDisplayType.Local)
        {
            ReloadRankInfos();
        }
    }
}