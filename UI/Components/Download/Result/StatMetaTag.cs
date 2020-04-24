using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Networking.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Download.Result
{
    public class StatMetaTag : BaseMetaTag {

        private ISprite iconSprite;


        [InitWithDependency]
        private void Init()
        {
            label.Anchor = Anchors.RightStretch;
            label.Pivot = Pivots.Right;

            iconSprite = CreateChild<UguiSprite>("icon", 1);
            {
                iconSprite.Anchor = Anchors.RightStretch;
                iconSprite.Pivot = Pivots.Right;
                iconSprite.X = -8f;
                iconSprite.SetOffsetVertical(2);
                iconSprite.Color = Color.black;
            }

            InvokeAfterFrames(1, () =>
            {
                iconSprite.Width = iconSprite.Height;

                label.X = -16 - iconSprite.Width;
            });
        }

        /// <summary>
        /// Sets the play count to display.
        /// </summary>
        public void SetPlayCount(int count) => Setup("icon-play", count);

        /// <summary>
        /// Sets the favorite count to display.
        /// </summary>
        public void SetFavoriteCount(int count) => Setup("icon-heart", count);

        /// <summary>
        /// Sets the display using specified values.
        /// </summary>
        private void Setup(string spritename, int count)
        {
            iconSprite.SpriteName = spritename;
            label.Text = count.ToString("N0");

            Width = label.PreferredWidth + 15 + Mathf.Abs(label.X);
        }
    }
}