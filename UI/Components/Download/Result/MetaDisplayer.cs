using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PBGame.Networking.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download.Result
{
    public class MetaDisplayer : UguiObject
    {
        private const float TagHeight = 24f;
        private const float MapTagWidth = 68f;
        private const float HorizontalTagSpace = 8f;

        private RankMetaTag rankTag;
        private StatMetaTag playCountTag;
        private StatMetaTag favoriteCountTag;

        private ManagedRecycler<MapMetaTag> mapTagRecycler;


        [InitWithDependency]
        private void Init()
        {
            rankTag = CreateChild<RankMetaTag>("rank", 0);
            {
                rankTag.Anchor = AnchorType.TopLeft;
                rankTag.Pivot = PivotType.TopLeft;
                rankTag.Position = Vector2.zero;
                rankTag.Size = new Vector2(100f, TagHeight);
            }
            playCountTag = CreateChild<StatMetaTag>("stat", 1);
            {
                playCountTag.Anchor = AnchorType.TopRight;
                playCountTag.Pivot = PivotType.TopRight;
                playCountTag.Position = Vector2.zero;
                playCountTag.Height = TagHeight;
            }
            favoriteCountTag = CreateChild<StatMetaTag>("favorite", 2);
            {
                favoriteCountTag.Anchor = AnchorType.TopRight;
                favoriteCountTag.Pivot = PivotType.TopRight;
                favoriteCountTag.Position = new Vector2(0f, -28);
                favoriteCountTag.Height = TagHeight;
            }

            mapTagRecycler = new ManagedRecycler<MapMetaTag>(CreateMetaTag);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Reset();
        }

        /// <summary>
        /// Sets meta tags for specified mapset.
        /// </summary>
        public void Setup(OnlineMapset mapset)
        {
            Reset();

            if(mapset == null)
                return;

            rankTag.SetRank(mapset.Status);
            playCountTag.SetPlayCount(mapset.PlayCount);
            favoriteCountTag.SetFavoriteCount(mapset.FavoriteCount);

            float tagX = rankTag.Width + HorizontalTagSpace;
            foreach (var mode in mapset.Maps.GroupBy(m => m.GameMode))
            {
                var tag = mapTagRecycler.GetNext();
                tag.SetMapCount(mode.Key, mode.Count());
                tag.Position = new Vector3(tagX, 0f);
                tagX += MapTagWidth + HorizontalTagSpace;
            }
        }

        /// <summary>
        /// Resets the displayer to initial state.
        /// </summary>
        public void Reset()
        {
            playCountTag.SetPlayCount(0);
            favoriteCountTag.SetPlayCount(0);
            mapTagRecycler.ReturnAll();
        }

        /// <summary>
        /// Creates a new meta tag.
        /// </summary>
        private MapMetaTag CreateMetaTag()
        {
            var tag = CreateChild<MapMetaTag>();
            tag.Anchor = AnchorType.TopLeft;
            tag.Pivot = PivotType.TopLeft;
            tag.Size = new Vector2(MapTagWidth, TagHeight);
            return tag;
        }
    }
}