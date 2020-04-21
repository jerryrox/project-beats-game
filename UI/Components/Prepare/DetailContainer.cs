using System.Linq;
using PBGame.UI.Components.Prepare.Details;
using PBGame.UI.Components.Prepare.Details.Meta;
using PBGame.UI.Components.Prepare.Details.Ranking;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.Prepare
{
    public class DetailContainer : UguiSprite {

        private MenuHolder menuHolder;
        private VersionDisplay versionDisplay;

        private IScrollView contentScroll;
        private MetaContainer metaContainer;
        private RankingContainer rankingContainer;


        [InitWithDependency]
        private void Init()
        {
            Color = HexColor.Create("0E1216");

            menuHolder = CreateChild<MenuHolder>("menu", 0);
            {
                menuHolder.Anchor = Anchors.TopStretch;
                menuHolder.Pivot = Pivots.Top;
                menuHolder.RawWidth = 0f;
                menuHolder.Y = 0f;
                menuHolder.Height = 56f;
            }
            versionDisplay = CreateChild<VersionDisplay>("version", 1);
            {
                versionDisplay.Anchor = Anchors.TopStretch;
                versionDisplay.Pivot = Pivots.Top;
                versionDisplay.RawWidth = 0f;
                versionDisplay.Y = -56f;
                versionDisplay.Height = 72f;
            }
            contentScroll = CreateChild<UguiScrollView>("content", 2);
            {
                contentScroll.Anchor = Anchors.Fill;
                contentScroll.RawWidth = -128f;
                contentScroll.SetOffsetVertical(128f, 0f);

                contentScroll.Background.Alpha = 0f;

                metaContainer = contentScroll.Container.CreateChild<MetaContainer>("meta", 0);
                {
                    metaContainer.Anchor = Anchors.TopStretch;
                    metaContainer.Pivot = Pivots.Top;
                    metaContainer.RawWidth = 0f;
                    metaContainer.Y = -32f;
                    metaContainer.Height = 360f;
                }
                rankingContainer = contentScroll.Container.CreateChild<RankingContainer>("ranking", 1);
                {
                    rankingContainer.Anchor = Anchors.TopStretch;
                    rankingContainer.Pivot = Pivots.Top;
                    rankingContainer.RawWidth = 0f;
                    rankingContainer.Y = -424f;
                    rankingContainer.Height = 360f;
                }

                // Calculate height of the scrollview content.
                contentScroll.Container.Height = GetContentHeight();
            }
        }

        /// <summary>
        /// Returns the height of the content scroll container.
        /// </summary>
        private float GetContentHeight()
        {
            int count = contentScroll.Container.RawTransform.childCount;
            float size = 32f * (count + 1);
            for (int i = 0; i < count; i++)
            {
                var obj = contentScroll.Container.RawTransform.GetChild(i).GetComponent<IGraphicObject>();
                if (obj != null)
                    size += obj.Height;
            }
            return size;
        }
    }
}