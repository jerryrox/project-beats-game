using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Data.Rankings;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Prepare.Details.Ranking
{
    public class RankingList : UguiListView, IListView {

        private List<IRankInfo> ranks = new List<IRankInfo>();


        [InitWithDependency]
        private void Init()
        {
            background.Alpha = 0f;

            Initialize(CreateCell, UpdateCell);
            CellSize = new Vector2(1152f, 36f);
            Corner = GridLayoutGroup.Corner.UpperLeft;
            Axis = GridLayoutGroup.Axis.Vertical;
        }

        /// <summary>
        /// Displays the specified list of rank information.
        /// </summary>
        public void Setup(IEnumerable<IRankInfo> ranks)
        {
            foreach(var rank in ranks)
                this.ranks.Add(rank);

            this.TotalItems = this.ranks.Count;
            ResetPosition();
        }

        /// <summary>
        /// Clears all cells from the list.
        /// </summary>
        public void Clear()
        {
            ranks.Clear();
            this.TotalItems = 0;
        }

        private IListItem CreateCell()
        {
            int index = transform.childCount;
            var cell = container.CreateChild<RankingCell>($"cell{index}", index);
            return cell;
        }

        private void UpdateCell(IListItem item)
        {
            var cell = item as RankingCell;
            if(cell == null)
                return;

            cell.IsEvenCell = item.ItemIndex % 2 == 1;
            cell.SetRank(ranks[item.ItemIndex]);
        }
    }
}