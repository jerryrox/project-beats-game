using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.Data.Rankings;
using PBGame.Rulesets;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details.Ranking
{
    public class RankingContainer : UguiSprite {

        private LoaderIcon loaderIcon;
        private RankingTabDisplay tabDisplay;
        private RankingColumn column;
        private RankingList rankingList;

        private IAnime loadShowAni;
        private IAnime loadHideAni;


        [ReceivesDependency]
        private PrepareModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Alpha = 0.0625f;

            loaderIcon = CreateChild<LoaderIcon>("loader");
            {
                loaderIcon.Position = Vector3.zero;
                loaderIcon.Size = new Vector2(32f, 32f);
            }
            tabDisplay = CreateChild<RankingTabDisplay>("tab");
            {
                tabDisplay.Anchor = AnchorType.TopStretch;
                tabDisplay.Pivot = PivotType.Top;
                tabDisplay.RawWidth = 0f;
                tabDisplay.Height = 0f;
                tabDisplay.Y = 0f;
            }
            column = CreateChild<RankingColumn>("column");
            {
                column.Anchor = AnchorType.TopStretch;
                column.Pivot = PivotType.Top;
                column.Height = 36f;
                column.Y = -52f;

                column.SetOffsetHorizontal(0f);
            }
            rankingList = CreateChild<RankingList>("list");
            {
                rankingList.Anchor = AnchorType.Fill;
                rankingList.Offset = new Offset(0f, 88f, 0f, 0f);
                rankingList.Column = column;
            }

            loadShowAni = new Anime();
            loadShowAni.AnimateFloat((alpha) => loaderIcon.Alpha = alpha)
                .AddTime(0f, () => loaderIcon.Alpha)
                .AddTime(0.35f, 1f)
                .Build();
            loadShowAni.AnimateFloat((alpha) => rankingList.Alpha = alpha)
                .AddTime(0f, () => rankingList.Alpha)
                .AddTime(0.35f, 0f)
                .Build();

            loadHideAni = new Anime();
            loadHideAni.AnimateFloat((alpha) => loaderIcon.Alpha = alpha)
                .AddTime(0f, () => loaderIcon.Alpha)
                .AddTime(0.35f, 0f)
                .Build();
            loadHideAni.AnimateFloat((alpha) => rankingList.Alpha = alpha)
                .AddTime(0f, () => rankingList.Alpha)
                .AddTime(0.35f, 1f)
                .Build();

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.RankList.BindAndTrigger(OnRankListChange);
            Model.GameMode.BindAndTrigger(OnGameModeChange);
            Model.IsRetrievingRecords.BindAndTrigger(OnRetrievingRecordsChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Model.RankList.Unbind(OnRankListChange);
            Model.GameMode.Unbind(OnGameModeChange);
            Model.IsRetrievingRecords.Unbind(OnRetrievingRecordsChange);
        }

        /// <summary>
        /// Starts reloading ranking info cells from appropriate sources.
        /// </summary>
        private void OnRankListChange(List<RankInfo> rankings)
        {
            rankingList.Clear();
            rankingList.Setup(rankings);
        }

        /// <summary>
        /// Event called on game mode configuration change.
        /// </summary>
        private void OnGameModeChange(GameModeType newMode)
        {
            column.RefreshColumns(Model.GetSelectedModeService());
        }

        /// <summary>
        /// Event called on records retrieving flag change.
        /// </summary>
        private void OnRetrievingRecordsChange(bool isRetrieving)
        {
            if (isRetrieving)
            {
                loadHideAni.Pause();
                loadShowAni.PlayFromStart();
            }
            else
            {
                loadShowAni.Pause();
                loadHideAni.PlayFromStart();
            }
        }
    }
}