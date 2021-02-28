using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using PBGame.Maps;
using PBGame.Data.Users;
using PBGame.Data.Records;
using PBGame.Audio;
using PBGame.Stores;
using PBGame.Configurations;
using PBFramework.UI.Navigations;
using PBFramework.Data.Bindables;
using PBFramework.Threading;
using PBFramework.Dependencies;

namespace PBGame.UI.Models
{
    public class InitializeModel : BaseModel {

        private Bindable<string> state = new Bindable<string>("");
        private BindableFloat progress = new BindableFloat(0f);
        private BindableBool isComplete = new BindableBool(false);


        /// <summary>
        /// Returns whether the loading is finished.
        /// </summary>
        public IReadOnlyBindable<bool> IsComplete => isComplete;

        /// <summary>
        /// Returns the current state of the loader.
        /// </summary>
        public IReadOnlyBindable<string> State => state;

        /// <summary>
        /// Returns the current loader progress.
        /// </summary>
        public IReadOnlyBindable<float> Progress => progress;

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }

        [ReceivesDependency]
        private IMapManager MapManager { get; set; }

        [ReceivesDependency]
        private ISoundTable SkinManager { get; set; }

        [ReceivesDependency]
        private ISoundPool SoundPooler { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IMapConfiguration MapConfiguration { get; set; }

        [ReceivesDependency]
        private IMapsetConfiguration MapsetConfiguration { get; set; }

        [ReceivesDependency]
        private IUserManager UserManager { get; set; }

        [ReceivesDependency]
        private IRecordStore RecordStore { get; set; }

        [ReceivesDependency]
        private IDownloadStore DownloadStore { get; set; }

        [ReceivesDependency]
        private ITemporaryStore TemporaryStore { get; set; }


        /// <summary>
        /// Starts the game loading process.
        /// </summary>
        public void StartLoad()
        {
            LoadConfigurations();
        }

        /// <summary>
        /// Navigates away to the next view.
        /// </summary>
        public void NavigateToNext()
        {
            OverlayNavigator.Show<SystemOverlay>();
            OverlayNavigator.Show<BackgroundOverlay>();
            ScreenNavigator.Show<HomeScreen>();
        }

        /// <summary>
        /// Starts loading the configurations.
        /// </summary>
        private void LoadConfigurations()
        {
            SetState("Loading configurations");

            GameConfiguration.Load();
            MapConfiguration.Load();
            MapsetConfiguration.Load();

            // Trigger options which must be applied on entering the game.
            GameConfiguration.MasterVolume.Trigger();
            GameConfiguration.UseParallax.Trigger();
            GameConfiguration.ResolutionQuality.Trigger();
            GameConfiguration.GlobalOffset.Trigger();
            GameConfiguration.PersistNotificationLevel.Trigger();

            LoadMapManager();
        }

        /// <summary>
        /// Starts loading the map manager.
        /// </summary>
        private void LoadMapManager()
        {
            SetState("Loading maps");

            TaskListener listener = new TaskListener();
            listener.OnProgress += SetProgress;
            listener.OnFinished += () =>
            {
                // Load any downloaded mapset files that weren't imported concurrently.
                foreach(var archive in DownloadStore.MapStorage.GetAllFiles())
                    MapManager.Import(archive);
                LoadUserData();
            };
            MapManager.Reload(listener);
        }

        /// <summary>
        /// Starts loading the user data.
        /// </summary>
        private void LoadUserData()
        {
            SetState("Loading user data");

            TaskListener listener = new TaskListener();
            listener.OnProgress += SetProgress;
            listener.OnFinished += LoadRecordData;
            UserManager.Reload(listener);
        }

        /// <summary>
        /// Starts loading the record data.
        /// </summary>
        private void LoadRecordData()
        {
            SetState("Loading record data");

            TaskListener listener = new TaskListener();
            listener.OnProgress += SetProgress;
            listener.OnFinished += FinalizeLoad;
            RecordStore.Reload(listener);
        }

        /// <summary>
        /// Finishes the loading process.
        /// </summary>
        private void FinalizeLoad()
        {
            UnityThread.DispatchUnattended(() =>
            {
                TemporaryStore.Clear();

                isComplete.Value = true;
                return null;
            });
        }

        /// <summary>
        /// Assigns the current status and invokes changed event.
        /// </summary>
        private void SetState(string state)
        {
            UnityThread.DispatchUnattended(() => this.state.Value = state);
        }

        /// <summary>
        /// Sets the current progress and invokes changed event.
        /// </summary>
        private void SetProgress(float progress)
        {
            UnityThread.DispatchUnattended(() => this.progress.Value = progress);
        }
    }
}