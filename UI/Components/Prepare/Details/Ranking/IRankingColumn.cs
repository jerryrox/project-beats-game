using System.Collections.Generic;
using PBGame.Rulesets;
using PBFramework.UI;

namespace PBGame.UI.Components.Prepare.Details.Ranking
{
    public interface IRankingColumn : ISprite {
    
        /// <summary>
        /// Returns the rank # column label.
        /// </summary>
        ILabel RankLabel { get; }

        /// <summary>
        /// Returns the score column label.
        /// </summary>
        ILabel ScoreLabel { get; }

        /// <summary>
        /// Returns the accuracy column lable.
        /// </summary>
        ILabel AccuracyLabel { get; }

        /// <summary>
        /// Returns the username column label.
        /// </summary>
        ILabel NameLabel { get; }

        /// <summary>
        /// Returns the max combo column label.
        /// </summary>
        ILabel MaxComboLabel { get; }

        /// <summary>
        /// Returns the list of judgemnt type column labels.
        /// </summary>
        IReadOnlyList<ILabel> JudgementLabels { get; }

        /// <summary>
        /// Returns the mod column label.
        /// </summary>
        ILabel ModLabel { get; }


        /// <summary>
        /// Refreshes the types of columns displayed for judgemnt type hit count.
        /// </summary>
        void RefreshColumns(IModeService service);
    }
}