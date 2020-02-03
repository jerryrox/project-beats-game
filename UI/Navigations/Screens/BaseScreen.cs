using PBGame.Graphics;
using PBGame.Animations;
using PBFramework;
using PBFramework.UI.Navigations;
using PBFramework.Animations;
using PBFramework.Dependencies;

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


        [InitWithDependency]
        private void Init(IRoot3D root3D)
        {
            if (IsRoot3D)
            {
                SetParent(root3D);
                myTransform.ResetTransform();
            }

            Depth = ScreenDepth;
        }


        protected override IAnime CreateShowAnime(IDependencyContainer dependencies)
        {
            return dependencies.Get<IAnimePreset>()?.GetDefaultScreenShow(this);
        }

        protected override IAnime CreateHideAnime(IDependencyContainer dependencies)
        {
            return dependencies.Get<IAnimePreset>()?.GetDefaultScreenHide(this);
        }
    }
}