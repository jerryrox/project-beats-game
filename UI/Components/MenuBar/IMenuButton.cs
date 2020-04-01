using System;
using PBFramework.UI;

namespace PBGame.UI.Components.MenuBar
{
    public interface IMenuButton : IBoxIconTrigger {

        /// <summary>
        /// Event called when the menu has been toggled on.
        /// </summary>
        event Action OnToggleOn;

        /// <summary>
        /// Event called when the menu has been toggled off.
        /// </summary>
        event Action OnToggleOff;


        /// <summary>
        /// Returns whether the menu is toggled on.
        /// </summary>
        bool IsToggled { get; }

        /// <summary>
        /// Returns the sprite used for menu toggle indication.
        /// </summary>
        ISprite ToggleSprite { get; }


        /// <summary>
        /// Sets the menu toggle state.
        /// </summary>
        void SetToggle(bool on);
    }
}