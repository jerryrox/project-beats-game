using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Data.Rankings;
using PBGame.Audio;
using PBGame.Graphics;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Prepare.Details.Ranking
{
    public class RankingTabButton : HighlightTrigger, IRankingTabButton {

        private RankDisplayTypes rankDisplay;


        public RankDisplayTypes RankDisplay
        {
            get => rankDisplay;
            set
            {
                rankDisplay = value;
                SetFocus(GameConfiguration.RankDisplay.Value == value, false);
            }
        }

        protected override float HighlightWidth => 162f;

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }


        [InitWithDependency]
        private void Init() => OnEnableInited();

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

        protected override void OnClickTriggered()
        {
            base.OnClickTriggered();
            GameConfiguration.RankDisplay.Value = rankDisplay;
        }

        /// <summary>
        /// Binds to external dependency events.
        /// </summary>
        private void BindEvents()
        {
            GameConfiguration.RankDisplay.OnValueChanged += OnRankDisplayChange;
            SetFocus(rankDisplay == GameConfiguration.RankDisplay.Value, false);
        }
        
        /// <summary>
        /// Unbinds from external dependency events.
        /// </summary>
        private void UnbindEvents()
        {
            GameConfiguration.RankDisplay.OnValueChanged -= OnRankDisplayChange;
        }

        /// <summary>
        /// Event called on rank display type change from configuration.
        /// </summary>
        private void OnRankDisplayChange(RankDisplayTypes newType, RankDisplayTypes _ = RankDisplayTypes.Global)
        {
            SetFocus(newType == rankDisplay);
        }
    }
}