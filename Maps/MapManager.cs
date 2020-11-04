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


        public MapManager(IMapsetStore store, NotificationBox notificationBox)
        {
            if(store == null) throw new ArgumentNullException(nameof(store));

            this.store = store;
            this.notificationBox = notificationBox;
        }

        public Task Reload(TaskListener listener = null)
        {
            return Task.Run(async () =>
            {
                if(listener != null)
                    listener.HasOwnProgress = false;

                // Wait for store reloading.
                await store.Reload(listener?.CreateSubListener());

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
                    listener?.SetFinished();
                    return null;
                });
            });
        }

        public Task<IMapset> Load(Guid id, TaskListener<IMapset> listener = null)
        {
            return Task.Run(() =>
            {
                IMapset mapset = store.Load(id);

                UnityThread.Dispatch(() =>
                {
                    // If already loaded within all mapsets, replace it.
                    allMapsets.AddOrReplace(mapset);
                    // Search again.
                    Search(lastSearch);
                    // Finished.
                    listener?.SetFinished(mapset);
                    return null;
                });
                return mapset;
            });
        }

        public Task<IMapset> Import(FileInfo file, TaskListener<IMapset> listener = null)
        {
            return Task.Run<IMapset>(async () =>
            {
                try
                {
                    // Start importing the file
                    Mapset mapset = await store.Import(file, listener: listener?.CreateSubListener<Mapset>());
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
                    listener?.SetFinished(mapset);
                    return mapset;
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError(e);
                    notificationBox?.Add(new Notification()
                    {
                        Message = $"Error while importing mapset: ({e.Message})\n{e.StackTrace}",
                        Type = NotificationType.Negative
                    });
                    listener?.SetFinished();
                    return null;
                }
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
            if(map == null)
                throw new ArgumentNullException(nameof(map));

            var mapset = map.Detail.Mapset;
            if (mapset == null)
                throw new Exception($"Could not retrieve the mapset of specified map: {map.Metadata.Artist} - {map.Metadata.Title}");

            // Delete the mapset itself if there is only one map.
            if (mapset.Maps.Count == 1)
            {
                DeleteMapset(mapset);
                return;
            }

            // If this map is currently selected, make it select the previous map.
            var selectedOriginal = selection.Map.Value?.OriginalMap;
            if (selectedOriginal == map)
                selection.SelectMap(mapset.GetMapBefore(selectedOriginal));

            store.DeleteMap(map);
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