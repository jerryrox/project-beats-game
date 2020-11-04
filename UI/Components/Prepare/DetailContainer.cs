using PBGame.Graphics;
using PBGame.UI.Components.Prepare.Details;
using PBGame.UI.Components.Prepare.Details.Meta;
using PBGame.UI.Components.Prepare.Details.Actions;
using PBGame.UI.Components.Prepare.Details.Ranking;
using PBFramework.UI;
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
        private ActionsContainer actionsContainer;


        /// <summary>
        /// Returns the total height of all children.
        /// </summary>
        public float TotalChildrenHeight
        {
            get
            {
                return menuHolder.Height +
                    versionDisplay.Height +
                    contentScroll.Container.Height;
            }
        }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            Color = colorPreset.DarkBackground;

            menuHolder = CreateChild<MenuHolder>("menu", 0);
            {
                menuHolder.Anchor = AnchorType.TopStretch;
                menuHolder.Pivot = PivotType.Top;
                menuHolder.RawWidth = 0f;
                menuHolder.Y = 0f;
                menuHolder.Height = 56f;
            }
            versionDisplay = CreateChild<VersionDisplay>("version", 1);
            {
                versionDisplay.Anchor = AnchorType.TopStretch;
                versionDisplay.Pivot = PivotType.Top;
                versionDisplay.RawWidth = 0f;
                versionDisplay.Y = -56f;
                versionDisplay.Height = 72f;
            }
            contentScroll = CreateChild<UguiScrollView>("content", 2);
            {
                contentScroll.Anchor = AnchorType.Fill;
                contentScroll.RawWidth = -128f;
                contentScroll.SetOffsetVertical(128f, 0f);

                contentScroll.Background.Alpha = 0f;

                metaContainer = contentScroll.Container.CreateChild<MetaContainer>("meta");
                {
                    metaContainer.Anchor = AnchorType.TopStretch;
                    metaContainer.Pivot = PivotType.Top;
                    metaContainer.Y = -32f;
                    metaContainer.Height = 360f;
                    metaContainer.SetOffsetHorizontal(0f);
                }
                rankingContainer = contentScroll.Container.CreateChild<RankingContainer>("ranking");
                {
                    rankingContainer.Anchor = AnchorType.TopStretch;
                    rankingContainer.Pivot = PivotType.Top;
                    rankingContainer.Y = -424f;
                    rankingContainer.Height = 360f;
                    rankingContainer.SetOffsetHorizontal(0f);
                }
                actionsContainer = contentScroll.Container.CreateChild<ActionsContainer>("actions");
                {
                    actionsContainer.Anchor = AnchorType.TopStretch;
                    actionsContainer.Pivot = PivotType.Top;
                    actionsContainer.Y = -816f;
                    actionsContainer.Height = 48f;
                    actionsContainer.SetOffsetHorizontal(0f);
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