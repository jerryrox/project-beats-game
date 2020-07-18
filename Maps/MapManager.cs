using System;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PBGame.Stores;
using PBGame.Rulesets.Maps;
using PBGame.Notifications;
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

        private string lastSearch = "";

        private INotificationBox notificationBox;


        public IMapsetList AllMapsets => allMapsets;

        public IMapsetList DisplayedMapsets => displayedMapsets;

        public string LastSearch => lastSearch;


        public MapManager(IMapsetStore store, INotificationBox notificationBox)
        {
            if(store == null) throw new ArgumentNullException(nameof(store));

            this.store = store;
            this.notificationBox = notificationBox;
        }

        public Task<bool> Import(FileInfo file)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var returnableProgress = new ReturnableProgress<Mapset>();
                    // Start importing the file
                    Mapset mapset = await store.Import(file, progress: returnableProgress);
                    if (mapset != null)
                    {
                        // Mapset must be fully loaded.
                        Mapset loadedMapset = store.LoadData(mapset);
                        if (loadedMapset != null)
                        {
                            // Dispatch mapset imported event on main thread.
                            UnityThread.Dispatch(() =>
                            {
                                // Add to all mapsets
                                allMapsets.AddOrReplace(loadedMapset);
                                // Reapply filter
                                Search(lastSearch);
                                OnImportMapset?.Invoke(loadedMapset);
                                return null;
                            });
                            return true;
                        }
                        else
                        {
                            notificationBox?.Add(new Notification()
                            {
                                Message = $"Failed to load imported mapset ({mapset.Metadata.Artist} - {mapset.Metadata.Title})",
                                Type = NotificationType.Negative
                            });
                        }
                    }
                    else
                    {
                        notificationBox?.Add(new Notification()
                        {
                            Message = $"Failed to import mapset at ({file.FullName})",
                            Type = NotificationType.Negative
                        });
                    }
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError(e);
                    notificationBox?.Add(new Notification()
                    {
                        Message = $"Error while importing mapset: ({e.Message})\n{e.StackTrace}",
                        Type = NotificationType.Negative
                    });
                }
                return false;
            });
        }

        public Task Reload(IEventProgress progress)
        {
            return Task.Run(async () =>
            {
                // Wait for store reloading.
                await store.Reload(progress);

                // Run on the main thread
                UnityThread.DispatchUnattended(() =>
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

        public Task Load(Guid id, IReturnableProgress<IMapset> progress)
        {
            return Task.Run(() =>
            {
                progress?.Report(0f);
                IMapset mapset = store.Load(id);

                UnityThread.DispatchUnattended(() =>
                {
                    // If already loaded within all mapsets, replace it.
                    allMapsets.AddOrReplace(mapset);
                    // Search again.
                    Search(lastSearch);
                    // Finished.
                    progress?.Report(1f);
                    progress.InvokeFinished(mapset);
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

        public void Sort(MapsetSortType sort) => displayedMapsets.Sort(sort);

        public void DeleteMap(IOriginalMap map)
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