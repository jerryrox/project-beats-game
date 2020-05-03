using PBGame.Rulesets.Difficulty;
using PBFramework.Graphics;
using UnityEngine;

namespace PBGame.Graphics
{
    public interface IColorPreset {
    
        ColorPalette PrimaryFocus { get; }

        ColorPalette SecondaryFocus { get; }

        ColorPalette Positive { get; }

        ColorPalette Negative { get; }

        ColorPalette Warning { get; }

        ColorPalette Passive { get; }

        Color DarkBackground { get; }


        /// <summary>
        /// Returns the color resembling the specified difficulty categorization type.
        /// </summary>
        ColorPalette GetDifficultyColor(DifficultyType type);
    }
}