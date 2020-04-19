using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Data.Rankings;
using PBFramework.UI;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details.Ranking
{
    public class RankingTabDisplay : UguiGrid, IRankingTabDisplay {

        private RankingTabButton globalTab;
        private RankingTabButton localTab;


        [InitWithDependency]
        private void Init()
        {
            CellSize = new Vector2(130f, 36f);
            Alignment = TextAnchor.UpperCenter;

            globalTab = CreateChild<RankingTabButton>("global", 0);
            {
                globalTab.LabelText = "Global rank";
                globalTab.RankDisplay = RankDisplayTypes.Global;
            }
            localTab = CreateChild<RankingTabButton>("local", 1);
            {
                localTab.LabelText = "Local rank";
                localTab.RankDisplay = RankDisplayTypes.Local;
            }
        }
    }
}