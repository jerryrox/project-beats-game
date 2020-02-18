using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details.Meta
{
    public class MetaContainer : UguiSprite, IMetaContainer {

        private IMetaDescription description;
        private IMetaMisc misc;
        private IMetaDifficulty difficulty;


        [InitWithDependency]
        private void Init()
        {
            Color = new Color(1f, 1f, 1f, 0.0625f);

            description = CreateChild<MetaDescription>("description", 0);
            {
                description.Anchor = Anchors.Fill;
                description.OffsetLeft = description.OffsetTop = description.OffsetBottom = 0f;
                description.OffsetRight = 620f;
            }
            misc = CreateChild<MetaMisc>("misc", 1);
            {
                misc.Anchor = Anchors.RightStretch;
                misc.Pivot = Pivots.Right;
                misc.X = -300f;
                misc.Width = 320f;
                misc.RawHeight = 0f;
            }
            difficulty = CreateChild<MetaDifficulty>("difficulty", 2);
            {
                difficulty.Anchor = Anchors.RightStretch;
                difficulty.Pivot = Pivots.Right;
                difficulty.RawHeight = 0f;
                difficulty.Width = 300f;
                difficulty.X = 0f;
            }
        }
    }
}