namespace PBGame.Audio
{
    /// <summary>
    /// Interface of an object which assists with music offsetting.
    /// </summary>
    public interface IMusicOffset {
    
        /// <summary>
        /// The offset applied to the music.
        /// </summary>
        int Offset { get; set; }
    }
}