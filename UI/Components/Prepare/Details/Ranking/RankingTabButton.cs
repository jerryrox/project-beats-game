using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
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
    public class RankingTabButton : HighlightableTrigger, IHasLabel {

        private ILabel label;
        private RankDisplayType rankDisplay;


        /// <summary>
        /// Type of rank data provision method.
        /// </summary>
        public RankDisplayType RankDisplay
        {
            get => rankDisplay;
            set
            {
                rankDisplay = value;
                SetFocused(GameConfiguration.RankDisplay.Value == value, false);
            }
        }

        public string LabelText
        {
            get => label.Text;
            set => label.Text = value;
        }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }


        [InitWithDependency]
        private void Init()
        {
            label = CreateChild<Label>("label", 0);
            {
                label.Anchor = AnchorType.Fill;
                label.RawSize = Vector2.zero;
                label.IsBold = true;
                label.FontSize = 18;
            }

            UseDefaultFocusAni();
            UseDefaultHighlightAni();
            UseDefaultHoverAni();

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
            SetFocused(rankDisplay == GameConfiguration.RankDisplay.Value, false);
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
        private void OnRankDisplayChange(RankDisplayType newType, RankDisplayType _ = RankDisplayType.Global)
        {
            IsFocused = newType == rankDisplay;
        }
    }
}