using PBFramework.UI;

namespace PBGame.UI.Components.Songs
{
    public interface IMenuButton : ITrigger {
    
        /// <summary>
        /// The icon spritename.
        /// </summary>
        string IconName { get; set; }
    }
}