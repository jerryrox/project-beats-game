using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.ProfileMenu
{
    public class OAuthLoginView : BaseLoginView {

        private BoxButton loginButton;


        public override float DesiredHeight => 68f;

        [ReceivesDependency]
        private ProfileMenuModel Model { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            loginButton = CreateChild<BoxButton>("login", 4);
            {
                loginButton.Anchor = AnchorType.TopStretch;
                loginButton.Pivot = PivotType.Top;
                loginButton.SetOffsetHorizontal(48f);
                loginButton.Y = -16;
                loginButton.Height = 36f;
                loginButton.Color = colorPreset.Positive;
                loginButton.LabelText = "OAuth log in";

                loginButton.OnTriggered += DoLogin;
            }
        }

        /// <summary>
        /// Starts performing OAuth login.
        /// </summary>
        private void DoLogin() => Model.RequestOAuth();
    }
}