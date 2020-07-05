using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Threading;
using PBFramework.Animations;
using PBFramework.Dependencies;

namespace PBGame.UI.Navigations.Screens
{
    public class SplashScreen : BaseScreen<SplashModel>, ISplashScreen {

        protected override int ViewDepth => ViewDepths.SplashScreen;


        protected override IAnime CreateShowAnime(IDependencyContainer dependencies) => null;

        protected override IAnime CreateHideAnime(IDependencyContainer dependencies) => null;

        protected override void OnPostShow()
        {
            base.OnPostShow();
            model.WaitSplash();
        }

        protected override SplashModel CreateModel() => new SplashModel();
    }
}