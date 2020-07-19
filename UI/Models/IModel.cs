using PBGame.UI.Navigations;

namespace PBGame.UI.Models
{
    public interface IModel
    {
        /// <summary>
        /// Event called when the host view is about to show.
        /// </summary>
        void OnPreShow();

        /// <summary>
        /// Event called when the host view has been shown.
        /// </summary>
        void OnPostShow();

        /// <summary>
        /// Event called when the host view is about to hide.
        /// </summary>
        void OnPreHide();

        /// <summary>
        /// Event called when the host view has been hidden.
        /// </summary>
        void OnPostHide();
    }
}