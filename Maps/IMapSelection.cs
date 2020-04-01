using System;
using PBGame.Rulesets.Maps;
using PBFramework.Audio;

namespace PBGame.Maps
{
    public interface IMapSelection {

        /// <summary>
        /// Event called when the selected mapset has changed.
        /// </summary>
        event Action<IMapset> OnMapsetChange;

        /// <summary>
        /// Event called when the selected mapset has changed.
        /// </summary>
        event Action<IPlayableMap> OnMapChange;

        /// <summary>
        /// Event called when the music for current map has been loaded.
        /// </summary>
        event Action<IMusicAudio> OnMusicLoaded;

        /// <summary>
        /// Event called when the background for current map has been loaded.
        /// </summary>
        event Action<IMapBackground> OnBackgroundLoaded;

        /// <summary>
        /// Event called when the current music has been unloaded.
        /// </summary>
        event Action OnMusicUnloaded;

        /// <summary>
        /// Event called when the current background has been unloaded.
        /// </summary>
        event Action OnBackgroundUnloaded;


        /// <summary>
        /// Returns the mapset currently selected.
        /// </summary>
        IMapset Mapset { get; }

        /// <summary>
        /// Returns the map currently selected.
        /// </summary>
        IPlayableMap Map { get; }

        /// <summary>
        /// Returns the selected map's music clip.
        /// </summary>
        IMusicAudio Music { get; }

        /// <summary>
        /// Returns the selected map's background asset.
        /// </summary>
        /// <value></value>
        IMapBackground Background { get; }

        /// <summary>
        /// Returns whether there is currently a valid mapset & map selection.
        /// </summary>
        bool HasSelection { get; }


        /// <summary>
        /// Selects the specified mapset and map.
        /// If map is null, a default map will be selected.
        /// </summary>
        void SelectMapset(IMapset mapset, IPlayableMap map = null);

        /// <summary>
        /// Selects the specified map and if required, changes the selected mapset.
        /// </summary>
        void SelectMap(IPlayableMap map);

        /// <summary>
        /// Selects the playable map automatically from the specified original map.
        /// </summary>
        void SelectMap(IOriginalMap map);
    }
}