using System;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.Data.Records;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Judgements;
using PBFramework.UI;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Result
{
    public class JudgementCounter : UguiGrid {

        private Dictionary<HitResultType, JudgementCountItem> items = new Dictionary<HitResultType, JudgementCountItem>();


        [ReceivesDependency]
        private ResultModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Size = Vector2.zero;

            CellSize = new Vector2(72, 48f);
            Spacing = new Vector2(2f, 0f);
            Corner = GridLayoutGroup.Corner.LowerLeft;
            Axis = GridLayoutGroup.Axis.Horizontal;
            Alignment = TextAnchor.LowerRight;
            Limit = 1;

            foreach (var type in (HitResultType[])Enum.GetValues(typeof(HitResultType)))
            {
                if (type != HitResultType.None)
                {
                    var item = CreateChild<JudgementCountItem>($"item{(int)type}");
                    items[type] = item;
                    item.SetResultType(type);
                }
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.Map.BindAndTrigger(OnMapChange);
            Model.Record.BindAndTrigger(OnRecordChange);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            Model.Map.Unbind(OnMapChange);
            Model.Record.Unbind(OnRecordChange);
        }

        /// <summary>
        /// Initializes counters for current map and record.
        /// </summary>
        private void SetupCounters()
        {
            var record = Model.Record.Value;
            if (record == null)
            {
                HideCounters();
                return;
            }

            // Toggle count items based on whether the judgement type is supported by current ruleset.
            foreach (var type in items.Keys)
            {
                bool isSupported = Model.IsSupportedHitType(type);
                items[type].Active = isSupported;
                if (isSupported)
                    items[type].SetCount(record.GetHitCount(type));
            }
        }

        /// <summary>
        /// Hides all counter items.
        /// </summary>
        private void HideCounters()
        {
            foreach (var item in items.Values)
                item.Active = false;
        }

        /// <summary>
        /// Event called on map change.
        /// </summary>
        private void OnMapChange(IPlayableMap map) => SetupCounters();

        /// <summary>
        /// Event called on record change.
        /// </summary>
        private void OnRecordChange(IRecord record) => SetupCounters();
    }
}