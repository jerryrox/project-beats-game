namespace PBGame.UI
{
    /// <summary>
    /// Indicates that an icon sprite exists.
    /// </summary>
    public interface IHasIcon {
    
        /// <summary>
        /// The spritename of the icon sprite.
        /// </summary>
        string IconName { get; set; }
    }
}