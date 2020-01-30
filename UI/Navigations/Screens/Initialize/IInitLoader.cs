using System;

namespace PBGame.UI.Navigations.Screens.Initialize
{
    public interface IInitLoader {

        /// <summary>
        /// Event called when the loader status has changed.
        /// </summary>
        event Action<string> OnStatusChange;

        /// <summary>
        /// Event called when the loader progress has changed.
        /// </summary>
        event Action<float> OnProgress;

        /// <summary>
        /// Event called when the loading process has completed.
        /// </summary>
        event Action OnComplete;


        /// <summary>
        /// Returns whether the loading process is completed.
        /// </summary>
        bool IsComplete { get; }

        /// <summary>
        /// Returns the current state of the loader.
        /// </summary>
        string State { get; }


        /// <summary>
        /// Starts the loading process.
        /// </summary>
        void Load();
    }
}