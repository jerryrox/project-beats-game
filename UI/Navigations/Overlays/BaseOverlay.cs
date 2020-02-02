using PBGame.Graphics;
using PBGame.Animations;
using PBFramework.UI.Navigations;
using PBFramework.Animations;
using PBFramework.Dependencies;

namespace PBGame.UI.Navigations.Overlays
{
    public abstract class BaseOverlay : UguiNavigationView, INavigationView {

        public override HideActions HideAction => HideActions.Recycle;

        /// <summary>
        /// Returns the depth of the overlay.
        /// </summary>
        protected abstract int OverlayDepth { get; }

        /// <summary>
        /// Returns whether the overlay should be displayed on 3D root.
        /// </summary>
        protected virtual bool IsRoot3D { get; } = false;

        [ReceivesDependency]
        protected IAnimePreset AniPreset { get; set; }


        [InitWithDependency]
        private void Init(IRoot3D root3D)
        {
            if (IsRoot3D)
                SetParent(root3D);

            Depth = OverlayDepth;
        }

        protected override IAnime CreateShowAnime() => AniPreset?.GetDefaultScreenShow(this);

        protected override IAnime CreateHideAnime() => AniPreset?.GetDefaultScreenHide(this);
    }
}