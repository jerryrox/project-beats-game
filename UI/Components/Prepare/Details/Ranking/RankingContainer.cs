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
    public class RankingContainer : UguiSprite, IRankingContainer {

        private IRankingTabDisplay tabDisplay;
        private IRankingColumn column;
        private IRankingList rankingList;


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
                tabDisplay.Anchor = Anchors.TopStretch;
                tabDisplay.Pivot = Pivots.Top;
                tabDisplay.RawWidth = 0f;
                tabDisplay.Height = 0f;
                tabDisplay.Y = 0f;
            }
            column = CreateChild<RankingColumn>("column", 1);
            {
                column.Anchor = Anchors.TopStretch;
                column.Pivot = Pivots.Top;
                column.Height = 36f;
                column.Y = -52f;
            }
            rankingList = CreateChild<RankingList>("list", 2);
            {
                rankingList.Anchor = Anchors.Fill;
                rankingList.OffsetLeft = rankingList.OffsetRight = rankingList.OffsetBottom = 0;
                rankingList.OffsetTop = 88f;
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
        private void OnGameModeChange(GameModes newMode, GameModes oldMode = GameModes.BeatsStandard)
        {
            column.RefreshColumns(ModeManager.GetService(newMode));
            ReloadRankInfos();
        }

        private void OnRankDisplayChange(RankDisplayTypes newType, RankDisplayTypes oldType = RankDisplayTypes.Local)
        {
            ReloadRankInfos();
        }
    }
}