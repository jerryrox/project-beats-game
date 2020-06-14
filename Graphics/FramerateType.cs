namespace PBGame.Graphics
{
    /// <summary>
    /// Types of supported framerates in configuration.
    /// </summary>
    public enum FramerateType {
    
        _50fps = 50,
        _60fps = 60,
        _120fps = 120,
        _240fps = 240,
        _300fps = 300,
    }

    public static class FramerateTypeExtension
    {
        /// <summary>
        /// Returns the numeric value of this framerate type.
        /// </summary>
        public static int GetFrameRate(this FramerateType context)
        {
            return (int)context;
        }
    }
}