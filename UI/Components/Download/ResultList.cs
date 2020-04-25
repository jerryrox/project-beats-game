using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Download.Result;
using PBGame.Networking.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Download
{
    public class ResultList : UguiListView {

        [ReceivesDependency]
        private DownloadState State { get; set; }


        [InitWithDependency]
        private void Init()
        {
            background.Alpha = 0f;

            Initialize(CreateCell, UpdateCell);

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();
            State.Results.BindAndTrigger(OnResultChange);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            State.Results.OnValueChanged -= OnResultChange;
        }

        /// <summary>
        /// Creates a new mapset result cell.
        /// </summary>
        private IListItem CreateCell()
        {
            var item = container.CreateChild<ResultCell>("item", container.ChildCount);
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
            TotalItems = result.Count;
        }
    }
}