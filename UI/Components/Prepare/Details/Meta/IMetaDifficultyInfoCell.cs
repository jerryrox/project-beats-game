using PBFramework.Graphics;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details.Meta
{
    public interface IMetaDifficultyInfoCell : IGraphicObject {
    
        /// <summary>
        /// Spritename of the icon.
        /// </summary>
        string IconName { get; set; }

        /// <summary>
        /// Text displayed on the label.
        /// </summary>
        string LabelText { get; set; }

        /// <summary>
        /// Color of the icon sprite.
        /// </summary>
        Color IconTint { get; set; }
    }
}