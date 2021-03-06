using System.Linq;
using System.Collections.Generic;
using PBGame.UI.Models.Dialog;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using PBGame.Data.Records;
using PBGame.Data.Rankings;
using PBGame.Maps;
using PBGame.Stores;
using PBGame.Graphics;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Configurations;
using PBFramework.UI.Navigations;
using PBFramework.Data.Bindables;
using PBFramework.Threading;
using PBFramework.Dependencies;

using Logger = PBFramework.Debugging.Logger;

namespace PBGame.UI.Models
{
    public class PrepareModel : BaseModel {

        private TaskListener<List<IRecord>> recordsListener;

        private BindableBool isRetrievingRecords = new BindableBool(false);
        private BindableBool isDetailedMode = new BindableBool(false);
        private Bindable<List<IOriginalMap>> mapList = new Bindable<List<IOriginalMap>>();
        private Bindable<string> mapsetDescription = new Bindable<string>("");
        private Bindable<List<RankInfo>> rankList = new Bindable<List<RankInfo>>(new List<RankInfo>());


        /// <summary>
        /// Returns whether the records are currently being retrieved.
        /// </summary>
        public IReadOnlyBindable<bool> IsRetrievingRecords => isRetrievingRecords;

        /// <summary>
        /// Returns whether the map information should be displayed as detailed mode.
        /// </summary>
        public IReadOnlyBindable<bool> IsDetailedMode => isDetailedMode;

        /// <summary>
        /// Returns the currently selected map.
        /// </summary>
        public IReadOnlyBindable<IPlayableMap> SelectedMap => MapSelection.Map;

        /// <summary>
        /// Returns the currently selected mapset.
        /// </summary>
        public IReadOnlyBindable<IMapset> SelectedMapset => MapSelection.Mapset;

        /// <summary>
        /// Returns whether unicode is preferred.
        /// </summary>
        public IReadOnlyBindable<bool> PreferUnicode => GameConfiguration.PreferUnicode;

        /// <summary>
        /// Returns the currently selected game mode.
        /// </summary>
        public IReadOnlyBindable<GameModeType> GameMode => GameConfiguration.RulesetMode;

        /// <summary>
        /// Returns the currently selected rank display type.
        /// </summary>
        public IReadOnlyBindable<RankDisplayType> RankDisplay => GameConfiguration.RankDisplay;

        /// <summary>
        /// Returns the list of maps included in the selected mapset.
        /// </summary>
        public IReadOnlyBindable<List<IOriginalMap>> MapList => mapList;

        /// <summary>
        /// Returns the list of ranks loaded.
        /// </summary>
        public IReadOnlyBindable<List<RankInfo>> RankList => rankList;

        /// <summary>
        /// Returns the description of the mapset.
        /// </summary>
        public IReadOnlyBindable<string> MapsetDescription => mapsetDescription;

        /// <summary>
        /// Returns the game mode service matching the current mode.
        /// </summary>
        public IModeService CurModeService => ModeManager.GetService(GameMode.Value);

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private IModeManager ModeManager { get; set; }

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }

        [ReceivesDependency]
        private IMapManager MapManager { get; set; }

        [ReceivesDependency]
        private IRecordStore RecordStore { get; set; }


        /// <summary>
        /// Toggles between detailed/brief information display mode.
        /// </summary>
        public void ToggleDetailedMode() => SetDetailedMode(!isDetailedMode.Value);

        /// <summary>
        /// Shows the map actions dialog overlay.
        /// </summary>
        public void ShowMapActions(IOriginalMap map)
        {
            if (map == null)
            {
                Logger.LogWarning("Attempted to show map actions for a null map.");
                return;
            }

            var dialogModel = OverlayNavigator.Show<DialogOverlay>().Model;
            dialogModel.SetMessage($"Select an action for the version ({map.Detail.Version})");
            dialogModel.AddOption(new DialogOption()
            {
                Callback = () => OnMapActionDelete(map),
                Label = "Delete",
                Color = ColorPreset.Warning
            });
            dialogModel.AddOption(new DialogOption()
            {
                Label = "Cancel",
                Color = ColorPreset.Negative
            });
        }

        /// <summary>
        /// Shows the offset overlay for current selection.
        /// </summary>
        public void ShowOffset() => OverlayNavigator.Show<OffsetsOverlay>();

        /// <summary>
        /// Sets the detailed/brief display mode.
        /// </summary>
        public void SetDetailedMode(bool isDetailed) => isDetailedMode.Value = isDetailed;

        /// <summary>
        /// Selects the next map in the map list.
        /// </summary>
        public void SelectNextMap()
        {
            var maps = mapList.Value;
            int index = GetSelectedMapIndex() + 1;
            if(index >= maps.Count)
                MapSelection.SelectMap(maps[0]);
            else
                MapSelection.SelectMap(maps[index]);
        }

        /// <summary>
        /// Selects the previous map in the map list.
        /// </summary>
        public void SelectPrevMap()
        {
            var maps = mapList.Value;
            int index = GetSelectedMapIndex() - 1;
            if(index < 0)
                MapSelection.SelectMap(maps[maps.Count - 1]);
            else
                MapSelection.SelectMap(maps[index]);
        }

        /// <summary>
        /// Navigates back to songs screen.
        /// </summary>
        public void NavigateToSongs() => ScreenNavigator.Show<SongsScreen>();

        /// <summary>
        /// Navigates away toward game screen.
        /// </summary>
        public void NavigateToGame()
        {
            ScreenNavigator.Hide<PrepareScreen>();
            OverlayNavigator.Show<GameLoadOverlay>().Model.StartLoad(new GameParameter()
            {
                Map = SelectedMap.Value,
            });
        }

        /// <summary>
        /// Navigates away to the results screen to view the specified record.
        /// </summary>
        public void NavigateToResults(IRecord record)
        {
            var resultScreen = ScreenNavigator.Show<ResultScreen>();
            resultScreen.Model.Setup(SelectedMap.Value, record, allowRetry: false);
        }

        /// <summary>
        /// Sets the type of rank display source.
        /// </summary>
        public void SetRankDisplay(RankDisplayType type)
        {
            GameConfiguration.RankDisplay.Value = type;
            GameConfiguration.Save();
        }

        /// <summary>
        /// Returns the index of the selected map.
        /// </summary>
        public int GetSelectedMapIndex()
        {
            var maps = mapList.Value;
            var curMap = SelectedMap.Value.OriginalMap;
            return maps.IndexOf(curMap);
        }

        /// <summary>
        /// Returns the mode servicer for currently selected game mode.
        /// </summary>
        public IModeService GetSelectedModeService() => ModeManager.GetService(GameMode.Value);

        protected override void OnPreShow()
        {
            base.OnPreShow();

            SelectedMapset.OnNewValue += OnMapsetChange;
            SelectedMap.OnNewValue += OnMapChange;
            GameMode.OnNewValue += OnGameModeChange;
            RankDisplay.OnNewValue += OnRankDisplayChange;

            MapManager.OnDeleteMap += OnDeleteMap;

            SetDetailedMode(false);
            SetMapList();
            RequestMapsetDescription();
            RequestRankings();
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            SelectedMapset.OnNewValue -= OnMapsetChange;
            SelectedMap.OnNewValue -= OnMapChange;
            GameMode.OnNewValue -= OnGameModeChange;
            RankDisplay.OnNewValue -= OnRankDisplayChange;

            MapManager.OnDeleteMap -= OnDeleteMap;

            mapsetDescription.Value = "";

            // TODO: Stop mapset description api request

            StopRecordRetrieval();
        }

        /// <summary>
        /// Sets map list for specified mapset and sorts by current game mode.
        /// </summary>
        private void SetMapList()
        {
            var mapset = SelectedMapset.Value;
            if (mapset != null)
            {
                // Sort maps
                mapset.SortMapsByMode(GameMode.Value);
                mapList.Value = mapset.Maps;
            }
        }

        /// <summary>
        /// Starts requesting for selected mapset's description from API.
        /// </summary>
        private void RequestMapsetDescription()
        {
            mapsetDescription.Value = "";
            // TODO:
        }

        /// <summary>
        /// Requests for the ranking list.
        /// </summary>
        private void RequestRankings()
        {
            var curMap = SelectedMap.Value;
            if (curMap == null)
            {
                rankList.Value = new List<RankInfo>();
                return;
            }

            SetupRecordListener();

            var displayType = RankDisplay.Value;
            if (displayType == RankDisplayType.Local)
            {
                RecordStore.GetTopRecords(curMap, limit: 30, listener: recordsListener);
            }
            else
            {
                rankList.Value = new List<RankInfo>();
                // TODO:
            }
        }

        /// <summary>
        /// Initializes a new records list listener.
        /// </summary>
        private TaskListener<List<IRecord>> SetupRecordListener()
        {
            StopRecordRetrieval();

            isRetrievingRecords.Value = true;

            var listener = recordsListener = new TaskListener<List<IRecord>>();
            listener.OnFinished += OnRecordsRetrieved;
            return listener;
        }

        /// <summary>
        /// Stops listening to record retrieval task's completion.
        /// </summary>
        private void StopRecordRetrieval()
        {
            if (recordsListener != null)
            {
                recordsListener.OnFinished -= OnRecordsRetrieved;
                recordsListener = null;

                isRetrievingRecords.Value = false;
            }
        }

        /// <summary>
        /// Event called on current map change.
        /// </summary>
        private void OnMapChange(IPlayableMap map)
        {
            RequestRankings();
        }

        /// <summary>
        /// Event called on current mapset change.
        /// </summary>
        private void OnMapsetChange(IMapset mapset)
        {
            SetMapList();
            RequestMapsetDescription();
        }

        /// <summary>
        /// Event called on game mode change.
        /// </summary>
        private void OnGameModeChange(GameModeType mode)
        {
            SetMapList();
            RequestRankings();
        }

        /// <summary>
        /// Event clled on rank display change.
        /// </summary>
        private void OnRankDisplayChange(RankDisplayType type)
        {
            RequestRankings();
        }

        /// <summary>
        /// Event called on map deletion from map manager.
        /// </summary>
        private void OnDeleteMap(IOriginalMap map)
        {
            var mapset = SelectedMapset.Value;
            if(mapset == null)
                return;

            // Trigger change on the maps list.
            MapList.Trigger();
        }

        /// <summary>
        /// Event called on selecting delete in map actions dialog.
        /// </summary>
        private void OnMapActionDelete(IOriginalMap map)
        {
            MapManager.DeleteMap(map);
        }

        /// <summary>
        /// Event called when the records list have been retrieved.
        /// </summary>
        private void OnRecordsRetrieved(List<IRecord> records)
        {
            StopRecordRetrieval();
            
            records.Sort((x, y) => y.Score.CompareTo(x.Score));

            int rank = 1;
            rankList.Value = records.Select((r) =>
            {
                // Check whether the record's replay version is outdated. If true, delete the replay data.
                if (RecordStore.HasReplayData(r) && r.ReplayVersion < CurModeService.LatestReplayVersion)
                    RecordStore.DeleteReplayFile(r);
                return new RankInfo(rank++, r);
            }).ToList();
        }
    }
}