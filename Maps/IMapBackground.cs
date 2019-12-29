using System;
using UnityEngine;

namespace PBGame.Maps
{
    /// <summary>
    /// Represents a map's background image.
    /// </summary>
    public interface IMapBackground : IDisposable {
    
        /// <summary>
        /// Returns the actual background image as texture.
        /// </summary>
        Texture2D Image { get; }

        /// <summary>
        /// Returns the size of the background image.
        /// </summary>
        Vector2 Size { get; }

        /// <summary>
        /// Returns the gradient top color sampled from the image.
        /// </summary>
        Color GradientTop { get; }

        /// <summary>
        /// Returns the gradient bottom color sampled from the image.
        /// </summary>
        Color GradientBottom { get; }

        /// <summary>
        /// Returns the highlight color sampled from the image.
        /// </summary>
        Color Highlight { get; }
    }
}