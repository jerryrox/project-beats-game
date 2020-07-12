using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.ProfileMenu
{
    public class CredentialLoginView : BaseLoginView
    {
        private LoginInput username;
        private LoginInput password;
        private LabelledToggle remember;
        private BoxButton loginButton;


        public override float DesiredHeight => 200f;

        [ReceivesDependency]
        private LoggedOutView LoggedOutView { get; set; }

        [ReceivesDependency]
        private ProfileMenuModel Model { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            username = CreateChild<LoginInput>("username", 1);
            {
                username.Anchor = AnchorType.TopStretch;
                username.Pivot = PivotType.Top;
                username.SetOffsetHorizontal(32);
                username.Y = -16f;
                username.Height = 36f;
                username.Placeholder = "username";
            }
            password = CreateChild<LoginInput>("password", 2);
            {
                password.Anchor = AnchorType.TopStretch;
                password.Pivot = PivotType.Top;
                password.SetOffsetHorizontal(32f);
                password.Y = -60f;
                password.Height = 36f;
                password.InputType = InputField.InputType.Password;
                password.Placeholder = "password";
            }
            remember = CreateChild<LabelledToggle>("remember", 3);
            {
                remember.Anchor = AnchorType.Top;
                remember.Pivot = PivotType.Top;
                remember.Position = new Vector3(0f, -104f);
                remember.Size = new Vector2(170f, 24f);
                remember.LabelText = "Remember me";
            }
            loginButton = CreateChild<BoxButton>("login", 4);
            {
                loginButton.Anchor = AnchorType.TopStretch;
                loginButton.Pivot = PivotType.Top;
                loginButton.SetOffsetHorizontal(48f);
                loginButton.Y = -148f;
                loginButton.Height = 36f;
                loginButton.Color = colorPreset.Positive;
                loginButton.LabelText = "Log in";

                loginButton.OnTriggered += DoLogin;
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.OnLoginFailed += OnLoginFailed;

            SetupComponents();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Model.OnLoginFailed -= OnLoginFailed;
        }

        /// <summary>
        /// Sets up inner components based on model state.
        /// </summary>
        private void SetupComponents()
        {
            var saveCredentials = Model.IsSaveCredentials.Value;
            remember.IsFocused = saveCredentials;
            if (saveCredentials)
            {
                username.Text = Model.SavedUsername.Value;
                password.Text = Model.SavedPassword.Value;
            }
            else
            {
                username.Text = "";
                password.Text = "";
            }
        }

        /// <summary>
        /// Performs login process.
        /// </summary>
        private void DoLogin()
        {
            username.IsFocused = false;
            password.IsFocused = false;
            Model.RequestCredentialAuth(username.Text, password.Text);
        }

        /// <summary>
        /// Event called when the login has failed.
        /// </summary>
        private void OnLoginFailed()
        {
            username.ShowInvalid();
            password.ShowInvalid();
        }
    }
}