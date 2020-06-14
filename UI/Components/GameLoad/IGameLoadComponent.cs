namespace PBGame.UI.Components.GameLoad
{
    public interface IGameLoadComponent {
    
        /// <summary>
        /// Returns the duration of the show animation.
        /// </summary>
        float ShowAniDuration { get; }

        /// <summary>
        /// Returns the duration of the hide animation.
        /// </summary>
        float HideAniDuration { get; }


        /// <summary>
        /// Starts showing the component with animation.
        /// </summary>
        void Show();

        /// <summary>
        /// Starts hiding the component with animation.
        /// </summary>
        void Hide();
    }
}