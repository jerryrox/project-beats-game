using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Components.Result;
using PBGame.UI.Components.Common;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Navigations.Screens
{
    public class ResultScreen : BaseScreen<ResultModel> {

        protected override int ViewDepth => ViewDepths.ResultScreen;


        [InitWithDependency]
        private void Init()
        {
            var blur = CreateChild<BlurDisplay>("blur");
            {
                blur.Offset = Offset.Zero;

                var darken = blur.CreateChild<UguiSprite>("darken");
                {
                    darken.Offset = Offset.Zero;
                    darken.Color = new Color(0f, 0f, 0f, 0.25f);
                }
            }
            var bottomMenu = CreateChild<BottomMenu>("bottom-menu", 4);
            {
                bottomMenu.Anchor = AnchorType.BottomStretch;
                bottomMenu.Pivot = PivotType.Bottom;
                bottomMenu.SetOffsetHorizontal(0f);
                bottomMenu.Y = 0f;
                bottomMenu.Height = 72f;
            }
            var infoBlock = CreateChild<InfoBlock>("info-block", 2);
            {
                infoBlock.Anchor = AnchorType.BottomStretch;
                infoBlock.Pivot = PivotType.Bottom;
                infoBlock.SetOffsetHorizontal(0f);
                infoBlock.Y = bottomMenu.Height;
                infoBlock.Height = 224f;
            }
            var infoStrip = CreateChild<InfoStrip>("info-strip", 1);
            {
                infoStrip.Anchor = AnchorType.BottomStretch;
                infoStrip.Pivot = PivotType.Bottom;
                infoStrip.SetOffsetHorizontal(0f);
                infoStrip.Y = bottomMenu.Height + infoBlock.Height;
                infoStrip.Height = 80;
            }
            var rankCircle = CreateChild<RankCircle>("rank-circle", 3);
            {
                rankCircle.Anchor = AnchorType.Bottom;
                rankCircle.Size = new Vector2(340f, 340f);
                rankCircle.Y = infoStrip.Y + infoStrip.Height;
            }
        }
    }
}