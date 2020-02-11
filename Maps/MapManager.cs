using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using PBGame.Stores;
using PBGame.Rulesets.Maps;
using PBFramework.Services;
using PBFramework.Threading;

using Random = UnityEngine.Random;

namespace PBGame.Maps
{
    public class MapManager : IMapManager {

        public event Action<IMapset> OnImportMapset;

        private IMapsetStore store;
        private IMapsetList allMapsets = new MapsetList(false);
        private IMapsetList displayedMapsets = new MapsetList(true);
        private IMapSelection selection;

        private string lastSearch = null;


        public IMapsetList AllMapsets => allMapsets;

        public IMapsetList DisplayedMapsets => displayedMapsets;

        public string LastSearch => lastSearch;


        public MapManager(IMapsetStore store)
        {
            if(store == null) throw new ArgumentNullException(nameof(store));

            this.store = store;
        }

        public async Task<bool> Import(FileInfo file)
        {
            var returnableProgress = new ReturnableProgress<Mapset>();
            // Start importing the file
            Mapset mapset = await store.Import(file, progress: returnableProgress);
            // Dispatch mapset imported event on main thread.
            if (mapset != null)
            {
                UnityThreadService.Dispatch(() =>
                {
                    OnImportMapset?.Invoke(mapset);
                    return null;
                });
                return true;
            }
            return false;
        }

        public Task Reload(IEventProgress progress)
        {
            return Task.Run(async () =>
            {
                // Wait for store reloading.
                await store.Reload(progress);

                // Run on the main thread
                UnityThreadService.DispatchUnattended(() =>
                {
                    // Refill the mapset list
                    allMapsets.Clear();
                    allMapsets.AddRange(store.Mapsets);

                    // TODO: Process for a case where the previously selected map no longer exists.

                    // Fill the displayed mapsets list using last search term.
                    Search(lastSearch);
                    // Finished
                    progress.InvokeFinished();
                    return null;
                });
            });
        }

        public void Search(string filter)
        {
            lastSearch = filter;
            displayedMapsets.Clear();
            displayedMapsets.AddRange(allMapsets.Search(filter));
        }

        public void Sort(MapsetSorts sort) => displayedMapsets.Sort(sort);

        public void DeleteMap(IMap map)
        {
            // TODO:
        }

        public void DeleteMapset(IMapset mapset)
        {
            // TODO:
        }

        public IMapset GetRandom()
        {
            if(displayedMapsets.Count == 0) return null;
            return displayedMapsets[Random.Range(0, displayedMapsets.Count)];
        }
    }
}