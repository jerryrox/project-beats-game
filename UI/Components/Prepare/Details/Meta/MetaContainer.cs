using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details.Meta
{
    public class MetaContainer : UguiSprite {

        private MetaDescription description;
        private MetaMisc misc;
        private MetaDifficulty difficulty;


        [InitWithDependency]
        private void Init()
        {
            Color = new Color(1f, 1f, 1f, 0.0625f);

            description = CreateChild<MetaDescription>("description", 0);
            {
                description.Anchor = AnchorType.Fill;
                description.Offset = new Offset(0f, 0f, 620f, 0f);
            }
            misc = CreateChild<MetaMisc>("misc", 1);
            {
                misc.Anchor = AnchorType.RightStretch;
                misc.Pivot = PivotType.Right;
                misc.X = -300f;
                misc.Width = 320f;
                misc.RawHeight = 0f;
            }
            difficulty = CreateChild<MetaDifficulty>("difficulty", 2);
            {
                difficulty.Anchor = AnchorType.RightStretch;
                difficulty.Pivot = PivotType.Right;
                difficulty.RawHeight = 0f;
                difficulty.Width = 300f;
                difficulty.X = 0f;
            }
        }
    }
}