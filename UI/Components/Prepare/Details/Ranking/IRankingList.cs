using System.Collections.Generic;
using PBGame.Data.Rankings;
using PBFramework.UI;

namespace PBGame.UI.Components.Prepare.Details.Ranking
{
    public interface IRankingList : IListView {

        /// <summary>
        /// Displays the specified list of rank information.
        /// </summary>
        void Setup(IEnumerable<IRankInfo> ranks);

        /// <summary>
        /// Clears all cells from the list.
        /// </summary>
        void Clear();
    }
}