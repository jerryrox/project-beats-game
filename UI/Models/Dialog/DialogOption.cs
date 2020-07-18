using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBGame.UI.Models.Dialog
{
    /// <summary>
    /// Representation of a single option of a dialog overlay.
    /// </summary>
    public class DialogOption {

        /// <summary>
        /// The displayed label text.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Tint color of the dialog button.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Action to be invoked on selection.
        /// </summary>
        public Action Callback { get; set; }


        /// <summary>
        /// Invokes the callback registered on the option.
        /// </summary>
        public void Invoke() => Callback?.Invoke();
    }
}