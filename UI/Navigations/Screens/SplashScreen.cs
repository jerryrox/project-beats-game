using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Threading;
using PBFramework.Animations;
using PBFramework.Dependencies;

namespace PBGame.UI.Navigations.Screens
{
    public class SplashScreen : BaseScreen, ISplashScreen {

        protected override int ViewDepth => ViewDepths.SplashScreen;

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }


        protected override IAnime CreateShowAnime(IDependencyContainer dependencies) => null;

        protected override IAnime CreateHideAnime(IDependencyContainer dependencies) => null;

        protected override void OnPostShow()
        {
            var timer = new SynchronizedTimer();
            timer.OnFinished += delegate
            {
                if (ScreenNavigator != null)
                {
                    ScreenNavigator.Show<InitializeScreen>();
                }
            };
            timer.Limit = 1f;
            timer.Start();
        }
    }
}