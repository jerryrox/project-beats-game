using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Data.Rankings;
using PBGame.Skins;
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
        private ITexture rankIcon;
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
        private ISkinManager SkinManager { get; set; }


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
                scoreHolder.Pivot = PivotType.Left;

                rankIcon = scoreHolder.CreateChild<UguiTexture>("icon", 0);
                {
                    rankIcon.Pivot = PivotType.Left;
                    rankIcon.Scale = new Vector3(0.3f, 0.3f, 1f);
                }
                score = scoreHolder.CreateChild<Label>("score", 1);
                {
                    SetupLabelStyle(score);
                    score.X = 30f;
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
                judgementGrid.Pivot = PivotType.Right;
                judgementGrid.Width = 1000f;
                judgementGrid.CellSize = new Vector2(70f, 36f);

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
        public void SetRank(IRankInfo info)
        {
            var record = info.Record;

            rank.Text = $"#{info.Rank}";
            rankIcon.Texture = SkinManager.CurrentSkin.GetTexture($"ranking-{record.Rank}-small").Element;
            score.Text = record.Score.ToString("N0");
            accuracy.Text = record.Accuracy.ToString("P2");
            username.Text = record.Username;
            maxCombo.Text = record.MaxCombo.ToString("N0");
            foreach (var resultPair in record.HitResultCounts)
            {
                judgementLabels[(int)resultPair.Key].Text = resultPair.Value.ToString("N0");
            }
            // TODO: Come back when mods are implemented.
            // mods.Text = "";
        }

        /// <summary>
        /// Initializes the label's style properties.
        /// </summary>
        private void SetupLabelStyle(ILabel label)
        {
            label.Pivot = PivotType.Left;
            label.FontSize = 17;
            label.Alignment = TextAnchor.MiddleLeft;
        }
    }
}