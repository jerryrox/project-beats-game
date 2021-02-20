using System;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using PBGame.Stores;
using PBGame.Rulesets.Maps;
using PBGame.Notifications;
using PBFramework.Threading;

using Random = UnityEngine.Random;
using Logger = PBFramework.Debugging.Logger;

namespace PBGame.Maps
{
    public class MapManager : IMapManager {

        public event Action<IMapset> OnImportMapset;

        public event Action<IOriginalMap> OnDeleteMap;

        public event Action<IMapset> OnDeleteMapset;

        private IMapsetStore store;
        private IMapsetList allMapsets = new MapsetList(false);
        private IMapsetList displayedMapsets = new MapsetList(true);
        private IMapSelection selection;

        private string lastSearch = "";

        private INotificationBox notificationBox;


        public IMapsetList AllMapsets => allMapsets;

        public IMapsetList DisplayedMapsets => displayedMapsets;

        public string LastSearch => lastSearch;


        public MapManager(IMapsetStore store, NotificationBox notificationBox, IMapSelection selection)
        {
            if(store == null) throw new ArgumentNullException(nameof(store));

            this.store = store;
            this.notificationBox = notificationBox;
            this.selection = selection;
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
                    Logger.LogError($"Error while importing mapset: {e.Message}\n{e.StackTrace}");
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
            if (map == null)
            {
                Logger.LogWarning("Attempted to delete a null map.");
                return;
            }

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
            mapset.Maps.Remove(map);
            OnDeleteMap?.Invoke(map);
        }

        public void DeleteMapset(IMapset mapset)
        {
            if (mapset == null)
            {
                Logger.LogWarning("Attempted to delete a null mapset.");
                return;
            }

            // If this mapset is currently selected, make it select the previous mapset.
            var selectedMapset = selection.Mapset.Value;
            if (selectedMapset == mapset)
            {
                // If there is only one mapset left in all mapsets, just remove selection.
                if(allMapsets.Count <= 1)
                    selection.SelectMapset(null);
                else
                {
                    // Determine which mapset list to choose the previous map from.
                    var mapsetsList = (
                        (displayedMapsets.Contains(mapset) && displayedMapsets.Count == 1) || displayedMapsets.Count == 0 ?
                        allMapsets :
                        displayedMapsets
                    );
                    selection.SelectMapset(mapsetsList.GetPrevious(mapset));
                }
            }

            store.Delete(mapset as Mapset);
            allMapsets.Remove(mapset);
            displayedMapsets.Remove(mapset);
            OnDeleteMapset?.Invoke(mapset);
        }

        public IMapset GetRandom()
        {
            if(displayedMapsets.Count == 0) return null;
            return displayedMapsets[Random.Range(0, displayedMapsets.Count)];
        }


    }
}