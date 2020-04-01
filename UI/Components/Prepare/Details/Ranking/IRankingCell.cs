using PBGame.Data.Rankings;
using PBFramework.UI;

namespace PBGame.UI.Components.Prepare.Details.Ranking
{
    public interface IRankingCell : ISprite, IListItem {

        /// <summary>
        /// Whether the cell order is the multiple of 2.
        /// </summary>
        bool IsEvenCell { set; }


        /// <summary>
        /// Adjusts widget positions based on the specified column display.
        /// </summary>
        void AdjustToColumn(IRankingColumn rankingColumn);

        /// <summary>
        /// Sets ranking information to display.
        /// </summary>
        void SetRank(IRankInfo rank);
    }
}