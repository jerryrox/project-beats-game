using PBGame.UI.Components.Prepare;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;

namespace PBGame.UI.Navigations.Screens
{
    public class PrepareScreen : BaseScreen, IPrepareScreen {

        private const float InfoDetailedY = 640f;
        private const float InfoBriefY = 250f;

        private InfoContainer infoContainer;
        private VersionContainer versionContainer;

        private bool isInfoDetailed = false;
        private IAnime infoDetailAni;
        private IAnime infoBriefAni;


        protected override int ScreenDepth => ViewDepths.PrepareScreen;


        [InitWithDependency]
        private void Init()
        {
            // Cache this container for inner component.
            Dependencies = Dependencies.Clone();
            Dependencies.CacheAs<IPrepareScreen>(this);

            infoContainer = CreateChild<InfoContainer>("info", 0);
            {
                infoContainer.Anchor = Anchors.BottomStretch;
                infoContainer.Pivot = Pivots.Top;
                infoContainer.RawWidth = 0f;
                infoContainer.Height = 640f;
                infoContainer.Y = InfoBriefY;
            }
            versionContainer = CreateChild<VersionContainer>("version", 1);
            {
                versionContainer.Anchor = Anchors.TopStretch;
                versionContainer.Pivot = Pivots.Top;
                versionContainer.RawWidth = 0;
                versionContainer.Y = 0f;
                versionContainer.Height = 160f;
            }

            infoDetailAni = new Anime();
            infoDetailAni.AnimateFloat(y => infoContainer.Y = y)
                .AddTime(0f, () => infoContainer.Y)
                .AddTime(0.25f, InfoDetailedY)
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