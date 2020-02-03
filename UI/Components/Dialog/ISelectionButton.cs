using System;
using PBFramework.UI;
using UnityEngine;

namespace PBGame.UI.Components.Dialog
{
    public interface ISelectionButton : ITrigger, IDisposable {
    
        /// <summary>
        /// The text to be displayed on the label.
        /// </summary>
        string LabelText { get; set; }

        /// <summary>
        /// The color of the background sprite.
        /// </summary>
        Color BackgroundColor { get; set; }
    }
}