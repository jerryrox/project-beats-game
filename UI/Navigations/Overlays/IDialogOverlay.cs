using System;
using PBFramework.UI.Navigations;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public interface IDialogOverlay : INavigationView {

        /// <summary>
        /// Sets the message to be displayed on the dialog.
        /// </summary>
        void SetMessage(string message);

        /// <summary>
        /// Adds confirm and cancel selections.
        /// </summary>
        void AddConfirmCancel(Action onConfirm = null, Action onCancel = null);

        /// <summary>
        /// Adds a new dialog selection using specified values.
        /// </summary>
        void AddSelection(string label, Color color, Action callback = null);
    }
}