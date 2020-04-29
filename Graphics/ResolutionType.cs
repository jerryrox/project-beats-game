using System;

namespace PBGame.Graphics
{
    /// <summary>
    /// Types of resolution quality settings.
    /// </summary>
    public enum ResolutionType {
    
        Best = 0,
        Good,
        Medium,
        Low
    }

    public static class ResolutionTypeExtension
    {
        /// <summary>
        /// Returns the scale value of the resolution for this type.
        /// </summary>
        public static float GetResolutionScale(this ResolutionType context)
        {
            switch (context)
            {
                case ResolutionType.Best: return 1f;
                case ResolutionType.Good: return 0.833334f;
                case ResolutionType.Medium: return 0.666667f;
                case ResolutionType.Low: return 0.5f;
            }
            throw new Exception("Unsupported resolution type: " + context);
        }
    }
}