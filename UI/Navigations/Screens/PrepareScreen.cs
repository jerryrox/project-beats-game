using PBGame.UI.Models;
using PBGame.UI.Components.Prepare;
using PBGame.Graphics;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Screens
{
    public class PrepareScreen : BaseScreen<PrepareModel>, IPrepareScreen {

        private const float InfoDetailedYDiff = 720f - 640f;
        private const float InfoBriefY = 250f;

        private InfoContainer infoContainer;
        private VersionContainer versionContainer;

        private IAnime infoDetailAni;
        private IAnime infoBriefAni;


        protected override int ViewDepth => ViewDepths.PrepareScreen;


        [InitWithDependency]
        private void Init(IRootMain rootMain)
        {
            Dependencies.Cache(this);

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
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();
            model.IsDetailedMode.BindAndTrigger(OnDetailedModeChange);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            model.IsDetailedMode.OnNewValue -= OnDetailedModeChange;
        }

        /// <summary>
        /// Event called when the detailed information display mode is changed.
        /// </summary>
        private void OnDetailedModeChange(bool isDetailed)
        {
            if (isDetailed)
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