using PBFramework.UI;
using UnityEngine;

namespace PBGame.UI.Components.ProfileMenu
{
    public interface IMenuButton : ITrigger {
    
        /// <summary>
        /// Base color of the button.
        /// </summary>
        Color Tint { get; set; }

        /// <summary>
        /// Text displayed on the menu button label.
        /// </summary>
        string LabelText { get; set; }
    }
}