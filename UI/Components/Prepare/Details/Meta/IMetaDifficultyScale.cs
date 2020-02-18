using PBFramework.Graphics;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details.Meta
{
    public interface IMetaDifficultyScale : IGraphicObject {
    
        /// <summary>
        /// Color theme applied to the display.
        /// </summary>
        Color Tint { get; set; }


        /// <summary>
        /// Sets specified values for display.
        /// </summary>
        void Setup(string label, float value, float maxValue);
    }
}