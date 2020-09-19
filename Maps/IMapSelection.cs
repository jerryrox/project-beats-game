using PBGame.Rulesets.Maps;
using PBGame.Configurations.Maps;
using PBFramework.Data.Bindables;
using PBFramework.Audio;

namespace PBGame.Maps
{
    public interface IMapSelection {

        /// <summary>
        /// Returns the mapset currently selected.
        /// </summary>
        IReadOnlyBindable<IMapset> Mapset { get; }

        /// <summary>
        /// Returns the map currently selected.
        /// </summary>
        IReadOnlyBindable<IPlayableMap> Map { get; }

        /// <summary>
        /// Returns the config for the currently selected mapset.
        /// </summary>
        Bindable<MapsetConfig> MapsetConfig { get; }

        /// <summary>
        /// Returns the config for the currently selected map.
        /// </summary>
        Bindable<MapConfig> MapConfig { get; }


        /// <summary>
        /// Returns the selected map's music clip.
        /// </summary>
        IReadOnlyBindable<IMusicAudio> Music { get; }

        /// <summary>
        /// Returns the selected map's background asset.
        /// </summary>
        IReadOnlyBindable<IMapBackground> Background { get; }

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