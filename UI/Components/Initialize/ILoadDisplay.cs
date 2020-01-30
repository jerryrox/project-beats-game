using PBFramework.UI;
using PBFramework.Graphics;

namespace PBGame.UI.Components.Initialize
{
    public interface ILoadDisplay : IGraphicObject {
        
        /// <summary>
        /// Returns the status displayer label.
        /// </summary>
        ILabel Status { get; }

        /// <summary>
        /// Returns the progress displayer bar.
        /// </summary>
        IProgressBar Progress { get; }


        /// <summary>
        /// Sets the status to display.
        /// </summary>
        void SetStatus(string status);

        /// <summary>
        /// Sets the progress to display.
        /// </summary>
        void SetProgress(float progress);
    }
}