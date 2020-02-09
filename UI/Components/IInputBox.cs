using System;

using IPBInput = PBFramework.UI.IInputBox;

namespace PBGame.UI.Components
{
    public interface IInputBox : IPBInput {

        /// <summary>
        /// Event called on input focus.
        /// </summary>
        event Action OnFocus;

        /// <summary>
        /// Event called on input unfocus.
        /// </summary>
        event Action OnUnfocus;


        /// <summary>
        /// Sets the focus state of the input box.
        /// </summary>
        void SetFocus(bool isFocused);
    }
}