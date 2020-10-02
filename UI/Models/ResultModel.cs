using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using PBGame.Maps;
using PBGame.Data.Records;
using PBGame.Assets.Caching;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Scoring;
using PBGame.Configurations;
using PBFramework.UI.Navigations;
using PBFramework.Data.Bindables;
using PBFramework.Allocation.Caching;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Models
{
    public class ResultModel : BaseModel
    {

        private CacherAgent<string, Texture2D> avatarAgent;

        private Bindable<IPlayableMap> map = new Bindable<IPlayableMap>();
        private Bindable<IRecord> record = new Bindable<IRecord>();
        private Bindable<Texture2D> avatarImage = new Bindable<Texture2D>();


        /// <summary>
        /// Returns the current map instance being displayed for.
        /// </summary>
        public IReadOnlyBindable<IPlayableMap> Map => map;

        /// <summary>
        /// Returns the map background currently loaded.
        /// </summary>
        public IReadOnlyBindable<IMapBackground> MapBackground => MapSelection.Background;

        /// <summary>
        /// Returns the current record instance being displayed for.
        /// </summary>
        public IReadOnlyBindable<IRecord> Record => record;

        /// <summary>
        /// Returns the avatar image currently loaded.
        /// </summary>
        public IReadOnlyBindable<Texture2D> AvatarImage => avatarImage;

        /// <summary>
        /// Returns whether unicode is preferred.
        /// </summary>
        public IReadOnlyBindable<bool> PreferUnicode => GameConfiguration.PreferUnicode;

        /// <summary>
        /// Returns the mode service instance suitable for the current map.
        /// </summary>
        private IModeService ModeService => ModeManager.GetService(map.Value.PlayableMode);

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IModeManager ModeManager { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IWebImageCacher WebImageCacher { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }


        [InitWithDependency]
        private void Init()
        {
            avatarAgent = new CacherAgent<string, Texture2D>(WebImageCacher);
            avatarAgent.OnFinished += OnAvatarLoaded;
        }

        protected override void OnPostHide()
        {
            base.OnPostHide();

            SetMap(null);
            SetRecord(null);
        }

        /// <summary>
        /// Navigates back to prepare screen.
        /// </summary>
        public void ToPrepare() => ScreenNavigator.Show<PrepareScreen>();

        /// <summary>
        /// Shares the current result via system's share feature.
        /// </summary>
        public void Share()
        {
            // TODO:
        }

        /// <summary>
        /// Starts replay of this result.
        /// </summary>
        public void Replay()
        {
            // TODO:
        }

        /// <summary>
        /// Re-enters the game to retry.
        /// </summary>
        public void Retry()
        {
            // TODO:
        }

        /// <summary>
        /// Initializes states for specified map and record.
        /// </summary>
        public void Setup(IPlayableMap map, IRecord record)
        {
            SetMap(map);
            SetRecord(record);
        }

        /// <summary>
        /// Returns a new score processor instance that would've been used for the current map.
        /// </summary>
        public IScoreProcessor GetScoreProcessor()
        {
            if(map.Value == null)
                return null;
            return ModeService.CreateScoreProcessor();
        }

        /// <summary>
        /// Returns the types of ranks to display the ranges for.
        /// </summary>
        public IEnumerable<RankType> GetRankRangeTypes()
        {
            yield return RankType.D;
            yield return RankType.C;
            yield return RankType.B;
            yield return RankType.A;
            // TODO: SH if used harder mods.
            yield return RankType.S;
        }

        /// <summary>
        /// Sets the map to display.
        /// </summary>
        private void SetMap(IPlayableMap map)
        {
            this.map.Value = map;
        }

        /// <summary>
        /// Sets the record to display.
        /// </summary>
        private void SetRecord(IRecord record)
        {
            RemoveAvatar();

            this.record.Value = record;

            if (record != null)
            {
                avatarAgent.Request(record.AvatarUrl);
            }
        }

        /// <summary>
        /// Removes currently loaded avatar image and stops on-going request if applicable.
        /// </summary>
        private void RemoveAvatar()
        {
            avatarImage.Value = null;
            avatarAgent.Remove();
        }

        /// <summary>
        /// Event called when avatar agent has loaded the avatar image.
        /// </summary>
        private void OnAvatarLoaded(Texture2D image)
        {
            avatarImage.Value = image;
        }
    }
}