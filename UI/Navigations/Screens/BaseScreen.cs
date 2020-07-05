using PBGame.Animations;
using PBFramework.Animations;
using PBFramework.Dependencies;

namespace PBGame.UI.Navigations.Screens
{
    public abstract class BaseScreen : BaseNavView {

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