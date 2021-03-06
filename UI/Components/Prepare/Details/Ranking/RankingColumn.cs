using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets;
using PBGame.Rulesets.Judgements;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

using Logger = PBFramework.Debugging.Logger;

namespace PBGame.UI.Components.Prepare.Details.Ranking
{
    public class RankingColumn : UguiSprite {

        private const float JudgementLabelStartX = 380f;
        private const float JudgementLabelInterval = 70f;

        private List<ILabel> judgementLabels;


        /// <summary>
        /// Returns the rank # column label.
        /// </summary>
        public ILabel RankLabel { get; private set; }

        /// <summary>
        /// Returns the score column label.
        /// </summary>
        public ILabel ScoreLabel { get; private set; }

        /// <summary>
        /// Returns the accuracy column lable.
        /// </summary>
        public ILabel AccuracyLabel { get; private set; }

        /// <summary>
        /// Returns the username column label.
        /// </summary>
        public ILabel NameLabel { get; private set; }

        /// <summary>
        /// Returns the max combo column label.
        /// </summary>
        public ILabel MaxComboLabel { get; private set; }

        /// <summary>
        /// Returns the list of judgemnt type column labels.
        /// </summary>
        public IReadOnlyList<ILabel> JudgementLabels => judgementLabels;

        /// <summary>
        /// Returns the mod column label.
        /// </summary>
        public ILabel ModLabel { get; private set; }


        [InitWithDependency]
        private void Init()
        {
            Color = new Color(0f, 0f, 0f, 0.25f);

            RankLabel = CreateLabel(-550, "Rank");
            ScoreLabel = CreateLabel(-485, "Score");
            AccuracyLabel = CreateLabel(-345, "Accuracy");
            NameLabel = CreateLabel(-250, "Name");
            MaxComboLabel = CreateLabel(-80, "Max combo");
            ModLabel = CreateLabel(450, "Mods");

            judgementLabels = new List<ILabel>();
            foreach (var m in (HitResultType[])Enum.GetValues(typeof(HitResultType)))
            {
                judgementLabels.Add(CreateLabel(null, m.ToString()));
            }
        }

        /// <summary>
        /// Refreshes the types of columns displayed for judgemnt type hit count.
        /// </summary>
        public void RefreshColumns(IModeService service)
        {
            // In case of a null service, just display all labels.
            if (service == null)
            {
                Logger.LogWarning($"RankingColumn.RefreshColumn - The specified mode service is null.");
                judgementLabels.ForEach(l => l.Active = true);
                return;
            }

            // Only display judgement types that are used by this mode.
            var timing = service.CreateTiming();
            int activeCount = 0;
            judgementLabels.ForEach(l => l.Active = false);
            foreach (var result in timing.SupportedHitResults())
            {
                activeCount++;
                judgementLabels[(int)result].Active = true;
            }
            // Position these labels.
            for (int i = 0; i < judgementLabels.Count; i++)
            {
                if (judgementLabels[i].Active)
                {
                    activeCount--;
                    judgementLabels[i].X = JudgementLabelStartX - (activeCount * JudgementLabelInterval);
                }
            }
        }

        /// <summary>
        /// Creates and returns a new label with shared design properties.
        /// </summary>
        private ILabel CreateLabel(float? x, string text)
        {
            var label = CreateChild<Label>(text, transform.childCount);
            label.Anchor = AnchorType.CenterStretch;
            label.Pivot = PivotType.Left;
            label.RawHeight = 0f;
            label.Text = text;
            if(x.HasValue)
                label.X = x.Value;
            label.Alignment = TextAnchor.MiddleLeft;
            label.IsBold = true;
            label.FontSize = 16;
            return label;
        }
    }
}