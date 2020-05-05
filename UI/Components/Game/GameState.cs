using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework;

namespace PBGame.UI.Components.Game
{
    /// <summary>
    /// Represents the state of the game screen itself.
    /// Note that gameplay-specific states should be managed by the corresponding game session objects.
    /// </summary>
    public class GameState {

        /// <summary>
        /// List of processes expected to finish while the game is initially loading.
        /// </summary>
        private List<IExplicitPromise> initialLoaders = new List<IExplicitPromise>();


        /// <summary>
        /// Returns a promise which waits for all the load processes to finish.
        /// </summary>
        public IExplicitPromise GetInitialLoadPromise() => new MultiPromise(initialLoaders.ToArray());

        /// <summary>
        /// Adds the specified loader to initial loading processes list.
        /// </summary>
        public void AddInitialLoader(IExplicitPromise promise)
        {
            if(promise != null)
                initialLoaders.Add(promise);
        }

        /// <summary>
        /// Resets the state for next time.
        /// </summary>
        public void Reset()
        {
            initialLoaders.ForEach(p => p.Revoke());
            initialLoaders.Clear();
        }
    }
}