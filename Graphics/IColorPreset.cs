using UnityEngine;

namespace PBGame.Graphics
{
    public interface IColorPreset {
    
        Color PrimaryFocus { get; }

        Color SecondaryFocus { get; }

        Color Positive { get; }

        Color Negative { get; }

        Color Warning { get; }
    }
}