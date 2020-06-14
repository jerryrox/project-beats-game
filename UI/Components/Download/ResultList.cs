using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.UI.Components.Download.Result;
using PBGame.Networking.API.Requests;
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
        private DownloadState State { get; set; }


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

            State.Results.BindAndTrigger(OnResultChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            State.Results.OnValueChanged -= OnResultChange;
        }

        protected override void Update()
        {
            base.Update();
            if (!ShouldUpdate || State.Results.Value.Count == 0 || requestedNextPage)
                return;

            var triggerPos = ContainerEndPos.y - NextPageReqThreshold;
            if (container.Position.y >= triggerPos)
            {
                requestedNextPage = true;
                State.RequestNextPage();
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
            var cell = (ResultCell)item;
            cell.Setup(State.Results.Value[item.ItemIndex]);
        }

        /// <summary>
        /// Event called on result list change.
        /// </summary>
        private void OnResultChange(List<OnlineMapset> result, List<OnlineMapset> _)
        {
            Vector2? lastDelta = null;
            if (State.IsRequestingNextPage)
            {
                lastDelta = container.Position;
                lastDelta -= ContainerStartPos;
            }

            TotalItems = result.Count;

            if (lastDelta.HasValue)
            {
                Vector2 pos = ContainerStartPos;
                pos += lastDelta.Value;
                container.Position = pos;
            }

            requestedNextPage = false;
        }
    }
}