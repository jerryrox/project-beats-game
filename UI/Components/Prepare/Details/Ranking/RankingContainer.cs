using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.Data.Rankings;
using PBGame.Rulesets;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.Prepare.Details.Ranking
{
    public class RankingContainer : UguiSprite {

        private RankingTabDisplay tabDisplay;
        private RankingColumn column;
        private RankingList rankingList;


        [ReceivesDependency]
        private PrepareModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Alpha = 0.0625f;

            tabDisplay = CreateChild<RankingTabDisplay>("tab", 0);
            {
                tabDisplay.Anchor = AnchorType.TopStretch;
                tabDisplay.Pivot = PivotType.Top;
                tabDisplay.RawWidth = 0f;
                tabDisplay.Height = 0f;
                tabDisplay.Y = 0f;
            }
            column = CreateChild<RankingColumn>("column", 1);
            {
                column.Anchor = AnchorType.TopStretch;
                column.Pivot = PivotType.Top;
                column.Height = 36f;
                column.Y = -52f;
                
                column.SetOffsetHorizontal(0f);
            }
            rankingList = CreateChild<RankingList>("list", 2);
            {
                rankingList.Anchor = AnchorType.Fill;
                rankingList.Offset = new Offset(0f, 88f, 0f, 0f);
                rankingList.Column = column;
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.RankList.BindAndTrigger(OnRankListChange);
            Model.GameMode.BindAndTrigger(OnGameModeChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Model.RankList.OnNewValue -= OnRankListChange;
            Model.GameMode.OnNewValue -= OnGameModeChange;
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
    }
}