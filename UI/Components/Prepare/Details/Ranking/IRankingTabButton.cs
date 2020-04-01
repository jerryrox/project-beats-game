using PBGame.Data.Rankings;
using PBFramework.UI;

namespace PBGame.UI.Components.Prepare.Details.Ranking
{
    public interface IRankingTabButton : IHighlightTrigger {

        /// <summary>
        /// Type of rank data provision method.
        /// </summary>
        RankDisplayTypes RankDisplay { get; set; }
    }
}