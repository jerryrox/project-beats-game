using UnityEngine;

namespace PBGame.UI.Components.ProfileMenu
{
    public interface IMenuButton : IButtonTrigger {
    
        /// <summary>
        /// Text displayed on the button label.
        /// </summary>
        string LabelText { get; set; }

        /// <summary>
        /// The color applied to the background box.
        /// </summary>
        Color Tint { get; set; }
    }
}