using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBGame.Configurations;
using PBFramework.Data.Bindables;
using PBFramework.Audio;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Models
{
    public class MusicMenuModel : BaseModel {

        private BindableBool isPlaying = new BindableBool(false);


        /// <summary>
        /// Returns whether the music controller is currently playing.
        /// </summary>
        public IReadOnlyBindable<bool> IsPlaying => isPlaying;

        /// <summary>
        /// Returns the currently selected map.
        /// </summary>
        public IReadOnlyBindable<IPlayableMap> SelectedMap => MapSelection.Map;

        /// <summary>
        /// Returns the currently selected map's background.
        /// </summary>
        public IReadOnlyBindable<IMapBackground> Background => MapSelection.Background;

        /// <summary>
        /// Returns whether unicode text is preferred.
        /// </summary>
        public IReadOnlyBindable<bool> PreferUnicode => GameConfiguration.PreferUnicode;

        [ReceivesDependency]
        private IMapManager MapManager { get; set; }

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IMusicController MusicController { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IMusicPlaylist MusicPlaylist { get; set; }


        /// <summary>
        /// Toggles between music playing/paused state.
        /// </summary>
        public void TogglePlaying()
        {
            if(MusicController.IsPlaying)
                MusicController.Pause();
            else
                MusicController.Play();
        }

        /// <summary>
        /// Selects a random music from the playlist.
        /// </summary>
        public void RandomizeMusic()
        {
            var mapset = MapManager.AllMapsets.GetRandom();
            MapSelection.SelectMapset(mapset);
        }

        /// <summary>
        /// Selects the previous music from the playlist.
        /// </summary>
        public void PrevMusic() => MapSelection.SelectMapset(MusicPlaylist.GetPrevious());

        /// <summary>
        /// Selects the next music from the playlist.
        /// </summary>
        public void NextMusic() => MapSelection.SelectMapset(MusicPlaylist.GetNext());

        protected override void OnPreShow()
        {
            base.OnPreShow();

            MusicController.OnPlay += OnMusicPlay;
            MusicController.OnUnpause += OnMusicPlay;
            MusicController.OnPause += OnMusicPause;

            isPlaying.Value = MusicController.IsPlaying;
        }

        protected override void OnPostHide()
        {
            base.OnPostHide();

            MusicController.OnPlay -= OnMusicPlay;
            MusicController.OnUnpause -= OnMusicPlay;
            MusicController.OnPause -= OnMusicPause;
        }

        /// <summary>
        /// Event called when the music controller has started playing or is unpaused.
        /// </summary>
        private void OnMusicPlay(float time)
        {
            isPlaying.Value = true;
        }

        /// <summary>
        /// Event calld when the music controller has paused playing.
        /// </summary>
        private void OnMusicPause()
        {
            isPlaying.Value = false;
        }
    }
}