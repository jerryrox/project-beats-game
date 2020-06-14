using PBGame.UI.Components.Prepare;
using PBGame.Graphics;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Screens
{
    public class PrepareScreen : BaseScreen, IPrepareScreen {

        private const float InfoDetailedYDiff = 720f - 640f;
        private const float InfoBriefY = 250f;

        private InfoContainer infoContainer;
        private VersionContainer versionContainer;

        private bool isInfoDetailed = false;
        private IAnime infoDetailAni;
        private IAnime infoBriefAni;


        protected override int ScreenDepth => ViewDepths.PrepareScreen;


        [InitWithDependency]
        private void Init(IRootMain rootMain)
        {
            // Cache this container for inner component.
            Dependencies = Dependencies.Clone();
            Dependencies.CacheAs<IPrepareScreen>(this);

            infoContainer = CreateChild<InfoContainer>("info", 0);
            {
                infoContainer.Anchor = AnchorType.BottomStretch;
                infoContainer.Pivot = PivotType.Top;
                infoContainer.RawWidth = 0f;
                infoContainer.Height = Mathf.Min(infoContainer.FullDetailHeight, rootMain.Resolution.y - InfoDetailedYDiff);
                infoContainer.Y = InfoBriefY;
            }
            versionContainer = CreateChild<VersionContainer>("version", 1);
            {
                versionContainer.Anchor = AnchorType.TopStretch;
                versionContainer.Pivot = PivotType.Top;
                versionContainer.RawWidth = 0;
                versionContainer.Y = 0f;
                versionContainer.Height = 160f;
            }

            infoDetailAni = new Anime();
            infoDetailAni.AnimateFloat(y => infoContainer.Y = y)
                .AddTime(0f, () => infoContainer.Y)
                .AddTime(0.25f, infoContainer.Height)
                .Build();

            infoBriefAni = new Anime();
            infoBriefAni.AnimateFloat(y => infoContainer.Y = y)
                .AddTime(0f, () => infoContainer.Y)
                .AddTime(0.25f, InfoBriefY)
                .Build();

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            // For the info container in brief mode.
            isInfoDetailed = false;
            infoContainer.Y = InfoBriefY;
        }

        public void ToggleInfoDetail()
        {
            isInfoDetailed = !isInfoDetailed;

            if (isInfoDetailed)
            {
                infoBriefAni.Stop();
                infoDetailAni.PlayFromStart();
            }
            else
            {
                infoDetailAni.Stop();
                infoBriefAni.PlayFromStart();
            }
        }
    }
}