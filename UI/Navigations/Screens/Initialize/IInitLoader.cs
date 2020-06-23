using System;
using PBFramework.Data.Bindables;

namespace PBGame.UI.Navigations.Screens.Initialize
{
    public interface IInitLoader {

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
        IReadOnlyBindable<string> State { get; }

        /// <summary>
        /// Returns the current loader progress.
        /// </summary>
        IReadOnlyBindable<float> Progress { get; }


        /// <summary>
        /// Starts the loading process.
        /// </summary>
        void Load();
    }
}