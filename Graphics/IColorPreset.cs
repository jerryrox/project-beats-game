using PBGame.Rulesets.Difficulty;
using UnityEngine;

namespace PBGame.Graphics
{
    public interface IColorPreset {
    
        Color PrimaryFocus { get; }

        Color SecondaryFocus { get; }

        Color Positive { get; }

        Color Negative { get; }

        Color Warning { get; }

        Color Passive { get; }


        /// <summary>
        /// Returns the color resembling the specified difficulty categorization type.
        /// </summary>
        Color GetDifficultyColor(DifficultyTypes type);
    }
}