using System;
using PBFramework.UI;

namespace PBGame.UI.Components
{
    /// <summary>
    /// Provides the basic implementation of a trigger used as a button.
    /// </summary>
    public interface IButtonTrigger : ITrigger {

        /// <summary>
        /// Event called on button trigger using click or a pointer-down.
        /// </summary>
        event Action OnTriggered;
    }
}