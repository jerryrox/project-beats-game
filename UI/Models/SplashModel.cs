using PBGame.UI.Navigations.Screens;
using PBFramework.UI.Navigations;
using PBFramework.Threading;
using PBFramework.Dependencies;

namespace PBGame.UI.Models
{
    public class SplashModel : IModel {

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }


        /// <summary>
        /// Waits for a certain amount of time and navigates away to initialize screen.
        /// </summary>
        public void WaitSplash()
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