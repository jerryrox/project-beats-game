using PBFramework.UI;

namespace PBGame.UI.Components
{
    /// <summary>
    /// ITrigger with a highlighting line at the bottom of the button.
    /// </summary>
    public interface IHighlightTrigger : IButtonTrigger {

        /// <summary>
        /// Returns whether the trigger is highlighted.
        /// </summary>
        bool IsFocused { get; }

        /// <summary>
        /// Text displayed on the trigger label.
        /// </summary>
        string LabelText { get; set; }


        /// <summary>
        /// Visually represents the focused state.
        /// </summary>
        void SetFocus(bool isFocused, bool animate = true);
    }
}