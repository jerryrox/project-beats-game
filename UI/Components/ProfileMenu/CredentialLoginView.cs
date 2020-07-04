using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBGame.Networking.API;
using PBGame.Networking.API.Requests;
using PBGame.Networking.API.Responses;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
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

        private AuthRequest authRequest;


        public override float DesiredHeight => 200f;

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private LoggedOutView LoggedOutView { get; set; }


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
                remember.Size = new Vector2(160f, 24f);
                remember.LabelText = "Remember me";

                remember.OnFocused += (isFocused) =>
                {
                    if (!isFocused)
                    {
                        GameConfiguration.Username.Value = "";
                        GameConfiguration.Password.Value = "";
                    }
                    GameConfiguration.SaveCredentials.Value = isFocused;
                    GameConfiguration.Save();
                };
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

                loginButton.OnTriggered += () =>
                {
                    DoLogin();
                };
            }
        }

        public override void Setup(IApiProvider provider)
        {
            base.Setup(provider);

            if (GameConfiguration.SaveCredentials.Value)
            {
                remember.IsFocused = true;
                username.Text = GameConfiguration.Username.Value;
                password.Text = GameConfiguration.Password.Value;
            }
            else
            {
                remember.IsFocused = false;
                username.Text = "";
                password.Text = "";
            }
        }

        public override void OnAuthSuccess()
        {
                if (remember.IsFocused)
                {
                    GameConfiguration.Username.Value = username.Text;
                    GameConfiguration.Password.Value = password.Text;
                    GameConfiguration.Save();
                }
        }

        public override void OnAuthFailed()
        {
            username.ShowInvalid();
            password.ShowInvalid();
        }

        /// <summary>
        /// Performs login process.
        /// </summary>
        private void DoLogin()
        {
            username.IsFocused = false;
            password.IsFocused = false;

            // Start request.
            authRequest = ApiProvider.Auth();
            authRequest.Username = username.Text;
            authRequest.Password = password.Text;
            LoggedOutView?.RequestAuth(authRequest);
        }
    }
}