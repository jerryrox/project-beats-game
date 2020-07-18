using PBGame.UI.Models;
using PBFramework.Animations;
using PBFramework.Dependencies;

namespace PBGame.UI.Navigations.Screens
{
    public class SplashScreen : BaseScreen<SplashModel>, ISplashScreen {

        protected override int ViewDepth => ViewDepths.SplashScreen;


        protected override IAnime CreateShowAnime(IDependencyContainer dependencies) => null;

        protected override IAnime CreateHideAnime(IDependencyContainer dependencies) => null;
    }
}