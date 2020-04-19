using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare
{
    public class InfoContainer : UguiObject {

        private SongMeta songMeta;
        private DetailContainer detailContainer;


        [InitWithDependency]
        private void Init()
        {
            songMeta = CreateChild<SongMeta>("song", 0);
            {
                songMeta.Anchor = Anchors.TopStretch;
                songMeta.Pivot = Pivots.Top;
                songMeta.RawWidth = 0f;
                songMeta.Y = 0f;
                songMeta.Height = 120f;
            }
            detailContainer = CreateChild<DetailContainer>("detail", 1);
            {
                detailContainer.Anchor = Anchors.Fill;
                detailContainer.RawWidth = detailContainer.OffsetBottom = 0f;
                detailContainer.OffsetTop = 120f;
            }
        }
    }
}