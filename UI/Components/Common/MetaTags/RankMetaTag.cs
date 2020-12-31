namespace PBGame.UI.Components.Common.MetaTags
{
    public class RankMetaTag : BaseMetaTag {

        /// <summary>
        /// Sets the map status (rank) to display.
        /// </summary>
        public void SetRank(string mapRank) => label.Text = mapRank;
    }
}