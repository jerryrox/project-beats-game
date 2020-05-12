using System.Collections.Generic;
using PBGame.Rulesets.Difficulty;
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


        /// <summary>
        /// Returns the color resembling the specified difficulty categorization type.
        /// </summary>
        ColorPalette GetDifficultyColor(DifficultyType type);
    }
}