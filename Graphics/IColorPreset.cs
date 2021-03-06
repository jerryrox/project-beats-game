using System.Collections.Generic;
using PBGame.Rulesets.Scoring;
using PBGame.Rulesets.Difficulty;
using PBGame.Rulesets.Judgements;
using PBFramework.Graphics;
using UnityEngine;

namespace PBGame.Graphics
{
    public interface IColorPreset {

        List<Color> DefaultComboColors { get; }

        ColorPalette PrimaryFocus { get; }

        ColorPalette SecondaryFocus { get; }

        ColorPalette Positive { get; }

        ColorPalette Negative { get; }

        ColorPalette Warning { get; }

        ColorPalette Passive { get; }

        Color DarkBackground { get; }

        Color Background { get; }


        /// <summary>
        /// Returns the color resembling the specified difficulty categorization type.
        /// </summary>
        ColorPalette GetDifficultyColor(DifficultyType type);

        /// <summary>
        /// Returns the color resembling the specified hit result type.
        /// </summary>
        ColorPalette GetHitResultColor(HitResultType type);

        /// <summary>
        /// Returns the color resembling the specified rank type.
        /// </summary>
        ColorPalette GetRankColor(RankType type);

        /// <summary>
        /// Returns the outline color for the specified rank type.
        /// </summary>
        Color GetRankOutlineColor(RankType type);
    }
}