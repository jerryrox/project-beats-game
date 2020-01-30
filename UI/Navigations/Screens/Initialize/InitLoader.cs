using System;
using PBGame.Maps;
using PBGame.Skins;
using PBFramework.Services;
using PBFramework.Threading;

namespace PBGame.UI.Navigations.Screens.Initialize
{
    public class InitLoader : IInitLoader {

        public event Action<string> OnStatusChange;

        public event Action<float> OnProgress;

        public event Action OnComplete;

        private IMapManager mapManager;
        private ISkinManager skinManager;


        public bool IsComplete { get; private set; }

        public string State { get; private set; }


        public InitLoader(IMapManager mapManager, ISkinManager skinManager)
        {
            if(mapManager == null) throw new ArgumentNullException(nameof(mapManager));
            if(skinManager == null) throw new ArgumentNullException(nameof(skinManager));

            this.mapManager = mapManager;
            this.skinManager = skinManager;
        }

        public void Load()
        {
            LoadMapManager();
        }

        /// <summary>
        /// Starts loading the map manager.
        /// </summary>
        private void LoadMapManager()
        {
            SetState("Loading maps");

            IEventProgress progress = new EventProgress();
            progress.OnProgress += SetProgress;
            progress.OnFinished += LoadSkinManager;
            mapManager.Reload(progress);
        }

        /// <summary>
        /// Starts loading the skin manager.
        /// </summary>
        private void LoadSkinManager()
        {
            SetState("Loading skins");

            IEventProgress progress = new EventProgress();
            progress.OnProgress += SetProgress;
            progress.OnFinished += FinalizeLoad;
            skinManager.Reload(progress);
        }

        /// <summary>
        /// Finishes the loading process.
        /// </summary>
        private void FinalizeLoad()
        {
            UnityThreadService.DispatchUnattended(() =>
            {
                IsComplete = true;
                OnComplete?.Invoke();
                return null;
            });
        }

        /// <summary>
        /// Assigns the current status and invokes changed event.
        /// </summary>
        private void SetState(string state)
        {
            this.State = state;
            OnStatusChange?.Invoke(state);
        }

        /// <summary>
        /// Sets the current progress and invokes changed event.
        /// </summary>
        private void SetProgress(float progress)
        {
            UnityThreadService.DispatchUnattended(() =>
            {
                OnProgress?.Invoke(progress);
                return null;
            });
        }
    }
}