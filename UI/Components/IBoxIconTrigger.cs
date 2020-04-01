namespace PBGame.UI.Components
{
    public interface IBoxIconTrigger : IButtonTrigger {
    
        /// <summary>
        /// Spritename of the icon sprite.
        /// </summary>
        string IconName { get; set; }
    }
}