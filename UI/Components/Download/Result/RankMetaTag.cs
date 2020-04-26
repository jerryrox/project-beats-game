using PBGame.Networking.Maps;

namespace PBGame.UI.Components.Download.Result
{
    public class RankMetaTag : BaseMetaTag {

        /// <summary>
        /// Sets the map status (rank) to display.
        /// </summary>
        public void SetRank(MapStatus mapRank) => label.Text = mapRank.ToString();
    }
}