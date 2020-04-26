using System;
using PBGame.Maps;
using PBGame.Data.Users;
using PBGame.Data.Records;
using PBGame.Skins;
using PBGame.Audio;
using PBGame.Stores;
using PBGame.Configurations;
using PBFramework.Services;
using PBFramework.Threading;
using PBFramework.Dependencies;

namespace PBGame.UI.Navigations.Screens.Initialize
{
    public class InitLoader : IInitLoader {

        public event Action<string> OnStatusChange;

        public event Action<float> OnProgress;

        public event Action OnComplete;


        public bool IsComplete { get; private set; }

        public string State { get; private set; }

        [ReceivesDependency]
        private IMapManager MapManager { get; set; }

        [ReceivesDependency]
        private ISkinManager SkinManager { get; set; }

        [ReceivesDependency]
        private ISoundPooler SoundPooler { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IMapConfiguration MapConfiguration { get; set; }

        [ReceivesDependency]
        private IMapsetConfiguration MapsetConfiguration { get; set; }

        [ReceivesDependency]
        private IUserManager UserManager { get; set; }

        [ReceivesDependency]
        private IRecordManager RecordManager { get; set; }

        [ReceivesDependency]
        private IDownloadStore DownloadStore { get; set; }


        public InitLoader(IDependencyContainer dependencies)
        {
            if(dependencies == null) throw new ArgumentNullException(nameof(dependencies));

            dependencies.Inject(this);
        }

        public void Load()
        {
            LoadConfigurations();
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

            // Apply volume changes.
            GameConfiguration.MasterVolume.Trigger();

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
            progress.OnFinished += () =>
            {
                // Load any downloaded mapset files that weren't imported concurrently.
                foreach(var archive in DownloadStore.MapStorage.GetAllFiles())
                    MapManager.Import(archive);
                LoadSkinManager();
            };
            MapManager.Reload(progress);
        }

        /// <summary>
        /// Starts loading the skin manager.
        /// </summary>
        private void LoadSkinManager()
        {
            SetState("Loading skins");

            IEventProgress progress = new EventProgress();
            progress.OnProgress += SetProgress;
            progress.OnFinished += LoadSkin;
            SkinManager.Reload(progress);
        }

        /// <summary>
        /// Starts loading the player's current skin.
        /// </summary>
        private void LoadSkin()
        {
            SetState("Loading player skin");

            // TODO: Check configuration for the selected skin.
            var promise = SkinManager.SelectSkin(SkinManager.DefaultSkin, SoundPooler);
            // Null promise would mean default skin load.
            if (promise == null)
            {
                SetProgress(1f);
                LoadUserData();
            }
            else
            {
                promise.OnProgress += SetProgress;
                promise.OnFinished += LoadUserData;
                promise.Start();
            }
        }

        /// <summary>
        /// Starts loading the user data.
        /// </summary>
        private void LoadUserData()
        {
            SetState("Loading user data");

            IEventProgress progress = new EventProgress();
            progress.OnProgress += SetProgress;
            progress.OnFinished += LoadRecordData;
            UserManager.Reload(progress);
        }

        /// <summary>
        /// Starts loading the record data.
        /// </summary>
        private void LoadRecordData()
        {
            SetState("Loading record data");

            IEventProgress progress = new EventProgress();
            progress.OnProgress += SetProgress;
            progress.OnFinished += FinalizeLoad;
            RecordManager.Reload(progress);
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