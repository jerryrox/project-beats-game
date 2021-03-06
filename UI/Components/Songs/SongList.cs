using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.Graphics;
using PBGame.Rulesets.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Songs
{
    public class SongList : UguiListView, IListView {

        [ReceivesDependency]
        private SongsModel Model { get; set; }


        [InitWithDependency]
        private void Init(IRootMain rootMain)
        {
            // Init the list view.
            Initialize(OnCreateListItem, OnUpdateListItem);
            CellSize = new Vector2(rootMain.Resolution.x, 82f);
            Corner = GridLayoutGroup.Corner.UpperLeft;
            Axis = GridLayoutGroup.Axis.Vertical;

            background.SpriteName = "null";

            OnEnableInited();

            // Recalibrate after a frame due to a limitation where a rect transform's size doesn't update immediately when using anchors.
            InvokeAfterTransformed(1, Recalibrate);
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.SelectedMapset.OnNewValue += OnMapsetChange;
            Model.Mapsets.BindAndTrigger(OnMapsetListChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Model.Mapsets.OnNewValue -= OnMapsetListChange;
            Model.SelectedMapset.OnNewValue -= OnMapsetChange;
        }

        /// <summary>
        /// Event called from song cell when hold action is invoked.
        /// </summary>
        private void OnItemHold(SongListItem item)
        {
            if(!item.Active)
                return;
            Model.TriggerDropdown(item.Mapset, item.RawTransform.position);
        }

        /// <summary>
        /// Event called from listview when another item should be created.
        /// </summary>
        private IListItem OnCreateListItem()
        {
            var item = container.CreateChild<SongListItem>("item");
            item.Anchor = AnchorType.MiddleStretch;
            item.Size = CellSize;
            item.SetOffsetHorizontal(0f);
            item.OnHold += () => OnItemHold(item);
            return item;
        }

        /// <summary>
        /// Event called from listview when the specified item needs to be refreshed.
        /// </summary>
        private void OnUpdateListItem(IListItem item)
        {
            var mapsets = Model.Mapsets.Value;
            if(mapsets == null || item.ItemIndex >= mapsets.Count || item.ItemIndex < 0)
                return;

            // Cast the item into SongListItem and refresh it.
            var songListItem = (SongListItem)item;
            songListItem.SetMapset(mapsets[item.ItemIndex]);
        }

        /// <summary>
        /// Event called when the selected mapset have been changed.
        /// </summary>
        private void OnMapsetChange(IMapset mapset)
        {
            CenterOnSelection(mapset);
        }

        /// <summary>
        /// Events called when the list of displayed mapsets have changed.
        /// </summary>
        private void OnMapsetListChange(List<IMapset> mapsets)
        {
            // Refresh the list.
            TotalItems = mapsets.Count;
            CenterOnSelection(Model.SelectedMapset.Value);
        }

        /// <summary>
        /// Centers the listview on selected mapset.
        /// </summary>
        private void CenterOnSelection(IMapset mapset)
        {
            // If the selected mapset currently exists in the new list, focus on that area.
            int mapsetIndex = Model.GetSelectedMapsetIndex();
            if (mapsetIndex >= 0)
            {
                // Smooth transition to selection position.
                ScrollTo(CalculateSelectionPos(mapsetIndex));
            }
        }

        /// <summary>
        /// Returns the target position where the listview should scroll to for specified item index.
        /// </summary>
        private Vector2 CalculateSelectionPos(int index)
        {
            // Calculate total height
            var totalHeight = container.Height;
            var viewportHeight = Height;

            // Calculate max top and bottom positions.
            var topBound = totalHeight * -0.5f + viewportHeight * 0.5f;
            var bottomBound = -topBound;

            var cellPos = totalHeight * 0.5f - CellHeight * (index + 0.5f);
            return new Vector2(0f, Mathf.Clamp(-cellPos, topBound, bottomBound));
        }
    }
}