using PBGame.UI.Models;
using PBGame.Animations;
using PBFramework.Animations;
using PBFramework.Dependencies;

namespace PBGame.UI.Navigations.Overlays
{
    public abstract class BaseOverlay<TModel> : BaseNavView<TModel>
        where TModel : class, IModel
    {
        protected override IAnime CreateShowAnime(IDependencyContainer dependencies)
        {
            return dependencies.Get<IAnimePreset>()?.GetDefaultOverlayShow(this);
        }

        protected override IAnime CreateHideAnime(IDependencyContainer dependencies)
        {
            return dependencies.Get<IAnimePreset>()?.GetDefaultOverlayHide(this);
        }
    }
}