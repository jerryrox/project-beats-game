using System;
using System.Collections.Generic;
using PBGame.Data.Rankings;
using PBGame.Graphics;
using PBGame.Rulesets;
using PBGame.Rulesets.Scoring;
using PBGame.Rulesets.Judgements;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details.Ranking
{
    public class RankingCell : UguiSprite, IListItem {

        private ILabel rank;
        private IGraphicObject scoreHolder;
        private ILabel rankIcon;
        private ILabel score;
        private ILabel accuracy;
        private ILabel username;
        private ILabel maxCombo;
        private ILabel mods;

        private IGrid judgementGrid;
        private List<ILabel> judgementLabels = new List<ILabel>();


        public int ItemIndex { get; set; }

        /// <summary>
        /// Sets whether the cell order is the multiple of 2.
        /// </summary>
        public bool IsEvenCell
        {
            set => Alpha = value ? 0.0625f : 0f;
        }

        [ReceivesDependency]
        private IModeManager ModeManager { get; set; }

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Alpha = 0f;

            rank = CreateChild<Label>("rank", 0);
            {
                SetupLabelStyle(rank);
            }
            scoreHolder = CreateChild<UguiObject>("score-holder", 1);
            {
                scoreHolder.Anchor = AnchorType.CenterStretch;
                scoreHolder.Pivot = PivotType.Left;
                scoreHolder.SetOffsetVertical(0f);

                rankIcon = scoreHolder.CreateChild<Label>("icon", 0);
                {
                    rankIcon.Anchor = AnchorType.LeftStretch;
                    rankIcon.Pivot = PivotType.Left;
                    rankIcon.X = 0;
                    rankIcon.Width = 32f;
                    rankIcon.FontSize = 34;
                    rankIcon.IsBold = true;
                    rankIcon.SetOffsetVertical(0f);
                }
                score = scoreHolder.CreateChild<Label>("score", 1);
                {
                    SetupLabelStyle(score);
                    score.X = -5f;
                }
            }
            accuracy = CreateChild<Label>("acc", 2);
            {
                SetupLabelStyle(accuracy);
            }
            username = CreateChild<Label>("username", 3);
            {
                SetupLabelStyle(username);
            }
            maxCombo = CreateChild<Label>("max-combo", 4);
            {
                SetupLabelStyle(maxCombo);
            }
            judgementGrid = CreateChild<UguiGrid>("judgements", 5);
            {
                judgementGrid.Anchor = AnchorType.CenterStretch;
                judgementGrid.Pivot = PivotType.Right;
                judgementGrid.Width = 1000;
                judgementGrid.Alignment = TextAnchor.UpperRight;
                judgementGrid.CellSize = new Vector2(70f, 36f);
                judgementGrid.Offset = new Offset(26f, 0f, 126f, 0f);

                foreach (var rank in (HitResultType[])Enum.GetValues(typeof(HitResultType)))
                {
                    int index = (int)rank;
                    var label = judgementGrid.CreateChild<Label>($"label{index}", index);
                    SetupLabelStyle(label);
                    judgementLabels.Add(label);
                }
            }
            mods = CreateChild<Label>("mods", 6);
            {
                SetupLabelStyle(mods);
            }
        }

        /// <summary>
        /// Adjusts widget positions based on the specified column display.
        /// </summary>
        public void AdjustToColumn(RankingColumn rankingColumn)
        {
            rank.X = rankingColumn.RankLabel.X;
            scoreHolder.X = rankingColumn.ScoreLabel.X;
            accuracy.X = rankingColumn.AccuracyLabel.X;
            username.X = rankingColumn.NameLabel.X;
            maxCombo.X = rankingColumn.MaxComboLabel.X;
            judgementGrid.X = rankingColumn.ModLabel.X;
            mods.X = rankingColumn.ModLabel.X;

            for (int i = 0; i < rankingColumn.JudgementLabels.Count; i++)
            {
                var label = rankingColumn.JudgementLabels[i];
                judgementLabels[i].Active = label.Active;
            }
        }

        /// <summary>
        /// Sets ranking information to display.
        /// </summary>
        public void SetRank(RankInfo info)
        {
            var record = info.Record;

            rank.Text = $"#{info.Rank}";
            rankIcon.Text = record.Rank.ToDisplayedString();
            rankIcon.Color = ColorPreset.GetRankColor(record.Rank);
            score.Text = record.Score.ToString("N0");
            accuracy.Text = record.Accuracy.ToString("P2");
            username.Text = record.Username;
            maxCombo.Text = record.MaxCombo.ToString("N0");

            foreach (var label in judgementLabels)
                label.Text = "0";
            foreach (var resultPair in record.HitResultCounts)
            {
                var label = judgementLabels[(int)resultPair.Key];
                label.Text = resultPair.Value.ToString("N0");
            }

            // TODO: Come back when mods are implemented.
            // mods.Text = "";
        }

        /// <summary>
        /// Initializes the label's style properties.
        /// </summary>
        private void SetupLabelStyle(ILabel label)
        {
            label.Anchor = AnchorType.CenterStretch;
            label.Pivot = PivotType.Left;
            label.FontSize = 17;
            label.Alignment = TextAnchor.MiddleLeft;

            label.SetOffsetVertical(0f);
        }
    }
}