using System;
using PBFramework.Graphics;
using UnityEngine;

namespace PBGame.UI.Components.Dialog
{
    public interface ISelectionHolder : IGraphicObject {

        /// <summary>
        /// Adds a new selection button using specified values.
        /// </summary>
        void AddSelection(string label, Color color, Action callback);

        /// <summary>
        /// Clears all selection buttons.
        /// </summary>
        void RemoveSelections();
    }
}