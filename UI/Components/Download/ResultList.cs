using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.UI.Components.Download.Result;
using PBGame.Networking.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download
{
    public class ResultList : UguiListView
    {
        /// <summary>
        /// Amount of offset applied to container end Y pos which triggers next page request.
        /// </summary>
        private const float NextPageReqThreshold = 150f;

        private BasicScrollbar scrollbar;

        private bool requestedNextPage;


        [ReceivesDependency]
        private DownloadModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            background.Alpha = 0f;

            scrollbar = CreateChild<BasicScrollbar>("scrollbar", 100);
            {
                scrollbar.Anchor = AnchorType.RightStretch;
                scrollbar.Pivot = PivotType.Right;
                scrollbar.X = 0;
                scrollbar.Width = 2f;
                scrollbar.SetOffsetVertical(0f);
                scrollbar.SetVertical();
            }

            VerticalScrollbar = scrollbar;

            InvokeAfterTransformed(1, () =>
            {
                Initialize(CreateCell, UpdateCell);
                CellSize = new Vector2(this.Width / 2, 180f);
                Axis = GridLayoutGroup.Axis.Vertical;
                Limit = 2;

                OnEnableInited();
            });
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            requestedNextPage = false;

            Model.OnMapsetListChange += OnResultChange;
            OnResultChange(Model.MapsetList.Value, false);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Model.OnMapsetListChange -= OnResultChange;
        }

        protected override void Update()
        {
            base.Update();
            if (!ShouldUpdate || Model.MapsetList.Value.Count == 0 || requestedNextPage)
                return;

            var triggerPos = ContainerEndPos.y - NextPageReqThreshold;
            if (container.Position.y >= triggerPos)
            {
                requestedNextPage = true;
                Model.RequestMapsets();
            }
        }

        /// <summary>
        /// Creates a new mapset result cell.
        /// </summary>
        private IListItem CreateCell()
        {
            var item = container.CreateChild<ResultCell>("item", container.ChildCount);
            item.Size = CellSize;
            return item;
        }

        /// <summary>
        /// Updates the specified cell displays.
        /// </summary>
        private void UpdateCell(IListItem item)
        {
            var mapsets = Model.MapsetList.Value;
            var cell = (ResultCell)item;
            cell.Setup(mapsets[item.ItemIndex]);
        }

        /// <summary>
        /// Event called on result list change.
        /// </summary>
        private void OnResultChange(List<OnlineMapset> result, bool hadCursor)
        {
            Vector2? lastDelta = null;
            if (hadCursor)
            {
                lastDelta = container.Position;
                lastDelta -= ContainerStartPos;
            }

            int countBeforeRequest = TotalItems;
            TotalItems = result.Count;

            // If there are no new results, the list shouldn't request for more results on current search options.
            if(countBeforeRequest != TotalItems)
                requestedNextPage = false;

            if (lastDelta.HasValue)
            {
                Vector2 pos = ContainerStartPos;
                pos += lastDelta.Value;
                container.Position = pos;
            }
        }
    }
}