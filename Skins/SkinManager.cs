using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using PBGame.Audio;
using PBGame.Stores;
using PBFramework;
using PBFramework.Services;
using PBFramework.Threading;
using PBFramework.Debugging;

namespace PBGame.Skins
{
    public class SkinManager : ISkinManager {

        public event Action<ISkin> OnImportSkin;

        /// <summary>
        /// List of skins loaded from store.
        /// </summary>
        private List<ISkin> skins = new List<ISkin>();

        /// <summary>
        /// Skin store provider instance.
        /// </summary>
        private ISkinStore store = new SkinStore();


        public ISkin DefaultSkin { get; private set; } = new DefaultSkin();

        public ISkin CurrentSkin { get; private set; }

        public IReadOnlyList<ISkin> Skins => skins;


        public async Task<bool> Import(FileInfo file)
        {
            var progress = new ReturnableProgress<Skin>();
            // Import the file
            Skin skin = await store.Import(file, progress: progress);
            // Dispatch mapset imported event on main thread.
            if (skin != null)
            {
                UnityThreadService.Dispatch(() =>
                {
                    OnImportSkin?.Invoke(skin);
                    return null;
                });
                return true;
            }
            return false;
        }

        public Task Reload(IEventProgress progress)
        {
            // Load the default skin.
            DefaultSkin.AssetStore.Load();
            
            return Task.Run(async () =>
            {
                // Wait for store reload
                await store.Reload(progress);

                // Run on main thread
                UnityThreadService.DispatchUnattended(() =>
                {
                    // Refill skins list
                    skins.Clear();
                    skins.Add(DefaultSkin);
                    skins.AddRange(store.Skins);

                    // TODO: Process for a case where the currently selected skin no longer exists in the store.

                    // Finished event
                    progress.InvokeFinished();
                    return null;
                });
            });
        }

        public IPromise SelectSkin(ISkin skin, ISoundPooler soundPooler)
        {
            if (skin == CurrentSkin)
            {
                Logger.Log($"SkinManager.SelectSkin - Already selected this skin!");
                return null;
            }
            if (skin == null)
            {
                Logger.LogWarning($"SkinManager.SelectSkin - The specified skin is null.");
                return null;
            }
            if (soundPooler == null)
            {
                Logger.LogWarning($"SkinManager.SelectSkin - The sound pooler is not specified.");
                return null;
            }

            Action skinLoadAction = () =>
            {
                CurrentSkin?.AssetStore.Unload();
                CurrentSkin = skin;

                // Setup sound effects.
                soundPooler.Prepare(skin);
            };

            // Load the new skin.
            var loadPromise = skin.AssetStore.Load();
            if (loadPromise != null && !loadPromise.IsFinished)
            {
                loadPromise.OnFinished += skinLoadAction;
                return loadPromise;
            }
            else if (loadPromise == null || loadPromise.IsFinished)
            {
                skinLoadAction.Invoke();
            }
            return null;
        }
    }
}