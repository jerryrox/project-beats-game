using PBFramework.UI;

namespace PBGame.UI.Components.MusicMenu
{
    public interface IControlButton : ITrigger {
        
        /// <summary>
        /// The spritename of the button icon.
        /// </summary>
        string IconName { get; set; }
        
        /// <summary>
        /// The size of the button icon.
        /// </summary>
        float IconSize { get; set; }
    }
}