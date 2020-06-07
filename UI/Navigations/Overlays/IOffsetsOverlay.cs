using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBFramework.UI.Navigations;

namespace PBGame.UI.Navigations.Overlays
{
    public interface IOffsetsOverlay : INavigationView {

        /// <summary>
        /// Initializes offset configuration for currently selected map and mapset.
        /// </summary>
        void Setup();

        /// <summary>
        /// Initializes offset configuration for specified mapset and map.
        /// </summary>
        void Setup(IMapset mapset, IMap map);
    }
}