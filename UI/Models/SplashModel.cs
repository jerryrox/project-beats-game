using PBGame.UI.Navigations.Screens;
using PBFramework.UI.Navigations;
using PBFramework.Threading;
using PBFramework.Dependencies;

namespace PBGame.UI.Models
{
    public class SplashModel : BaseModel {

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }


        /// <summary>
        /// Waits for a certain amount of time and navigates away to initialize screen.
        /// </summary>
        protected override void OnPostShow()
        {
            base.OnPostShow();

            var timer = new SynchronizedTimer();
            timer.OnFinished += delegate
            {
                if (ScreenNavigator != null)
                    ScreenNavigator.Show<InitializeScreen>();
            };
            timer.Limit = 1f;
            timer.Start();
        }
    }
}