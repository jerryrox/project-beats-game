using PBGame.UI.Components.Common.Dropdown;
using UnityEngine;

namespace PBGame.UI
{
    public interface IDropdownProvider {

        /// <summary>
        /// Opens a dropdown menu using specified context.
        /// </summary>
        DropdownMenu Open(DropdownContext context);

        /// <summary>
        /// Opens a dropdown menu at specified world position.
        /// </summary>
        DropdownMenu OpenAt(DropdownContext context, Vector2 worldPos);
    }
}