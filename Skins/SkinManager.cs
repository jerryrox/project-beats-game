using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using PBGame.Stores;
using PBFramework;
using PBFramework.Services;
using PBFramework.Threading;

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

        public async Task Reload(IEventProgress progress)
        {
            // Load the default skin.
            DefaultSkin.AssetStore.Load();

            // Wait for store reload
            await store.Reload(progress);
            // Run on main thread
            UnityThreadService.Dispatch(() =>
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
        }

        public IPromise SelectSkin(ISkin skin)
        {
            if(skin == CurrentSkin || skin == null) return null;

            // Load the new skin.
            var loadPromise = skin.AssetStore.Load();
            if (loadPromise != null && !loadPromise.IsFinished)
            {
                loadPromise.OnFinished += () =>
                {
                    CurrentSkin?.AssetStore.Unload();
                    CurrentSkin = skin;
                };
                return loadPromise;
            }
            else if (loadPromise.IsFinished)
            {
                CurrentSkin?.AssetStore.Unload();
                CurrentSkin = skin;
            }
            return null;
        }
    }
}