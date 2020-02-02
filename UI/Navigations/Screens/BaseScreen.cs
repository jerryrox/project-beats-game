using PBGame.Graphics;
using PBGame.Animations;
using PBFramework.UI.Navigations;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Screens
{
    public abstract class BaseScreen : UguiNavigationView, IBaseScreen {

        public override HideActions HideAction => HideActions.Recycle;

        /// <summary>
        /// Returns the depth of the screen.
        /// </summary>
        protected abstract int ScreenDepth { get; }

        /// <summary>
        /// Returns whether the screen should be displayed on 3D root.
        /// </summary>
        protected virtual bool IsRoot3D { get; } = false;

        [ReceivesDependency]
        protected IAnimePreset AniPreset { get; set; }


        [InitWithDependency]
        private void Init(IRoot3D root3D)
        {
            if (IsRoot3D)
                SetParent(root3D);

            Depth = ScreenDepth;
        }

        /// <summary>
        /// Creates a new instance of the screen show animation.
        /// </summary>
        protected override IAnime CreateShowAnime() => AniPreset?.GetDefaultScreenShow(this);

        /// <summary>
        /// Creates a new instance of the screen hide animation.
        /// </summary>
        protected override IAnime CreateHideAnime() => AniPreset?.GetDefaultScreenHide(this);
    }
}