using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.Data.Users;
using PBGame.Assets.Fonts;
using PBGame.Graphics;
using PBGame.Configurations;
using PBGame.Networking.API;
using PBGame.Networking.API.Osu.Requests;
using PBGame.Networking.API.Osu.Responses;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Threading;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.ProfileMenu
{
    // TODO: Support for logging in using other API providers.
    public class LoggedOutView : UguiObject, IHasAlpha
    {
        private CanvasGroup canvasGroup;

        private ISprite bg;
        private LoginInput username;
        private LoginInput password;
        private IToggle remember;
        private BoxButton loginButton;
        private Loader loader;


        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }

        private IApi OsuApi => ApiManager.GetApi(ApiProviders.Osu);

        [ReceivesDependency]
        private IApiManager ApiManager { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IUserManager UserManager { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset, IFontManager fontManager)
        {
            canvasGroup = myObject.AddComponent<CanvasGroup>();

            bg = CreateChild<UguiSprite>("bg", 0);
            {
                bg.Anchor = Anchors.Fill;
                bg.RawSize = Vector2.zero;
                bg.Color = HexColor.Create("1D2126");
            }
            username = CreateChild<LoginInput>("username", 1);
            {
                username.Anchor = Anchors.TopStretch;
                username.SetOffsetHorizontal(32);
                username.Y = -40f;
                username.Height = 36f;
                username.Placeholder = "username";
            }
            password = CreateChild<LoginInput>("password", 2);
            {
                password.Anchor = Anchors.TopStretch;
                password.SetOffsetHorizontal(32f);
                password.Y = -80f;
                password.Height = 36f;
                password.InputType = InputField.InputType.Password;
                password.Placeholder = "password";
            }
            remember = CreateChild<UguiToggle>("remember", 3);
            {
                remember.Anchor = Anchors.Top;
                remember.X = -64f;
                remember.Y = -120f;

                remember.Background.Size = remember.Tick.Size = new Vector2(24f, 24f);
                remember.Background.SpriteName = remember.Tick.SpriteName = "circle-32";
                remember.Background.Color = Color.black;
                remember.Tick.Color = colorPreset.PrimaryFocus;

                remember.Label.X = 24f;
                remember.Label.Width = 120f;
                remember.Label.Font = fontManager.DefaultFont;
                remember.Label.FontSize = 16;
                remember.Label.Text = "Remember me";

                remember.OnChange += (isToggled) =>
                {
                    GameConfiguration.SaveCredentials.Value = remember.Value;
                };
            }
            loginButton = CreateChild<BoxButton>("login", 4);
            {
                loginButton.Anchor = Anchors.TopStretch;
                loginButton.SetOffsetHorizontal(48f);
                loginButton.Y = -162f;
                loginButton.Height = 36f;
                loginButton.Color = colorPreset.Positive;
                loginButton.LabelText = "Login to osu!";

                loginButton.OnTriggered += () =>
                {
                    DoLogin();
                };
            }
            loader = CreateChild<Loader>("loader", 5);
            {
                loader.Anchor = Anchors.Fill;
                loader.RawSize = Vector2.zero;
            }

            if (GameConfiguration.SaveCredentials.Value)
            {
                remember.Value = true;
                username.Text = GameConfiguration.Username.Value;
                password.Text = GameConfiguration.Password.Value;
            }
        }

        /// <summary>
        /// Performs login process.
        /// </summary>
        private void DoLogin()
        {
            loader.Show();

            username.SetFocus(false);
            password.SetFocus(false);

            // Start request.
            var request = new LoginRequest(username.Text, password.Text);
            request.OnRequestEnd += OnLoginResponse;
            OsuApi.Request(request);
        }

        /// <summary>
        /// Event called on login API response.
        /// </summary>
        private void OnLoginResponse(LoginResponse response)
        {
            if (!response.IsSuccess)
            {
                loader.Hide();

                GameConfiguration.Username.Value = "";
                GameConfiguration.Password.Value = "";

                username.ShowInvalid();
                password.ShowInvalid();

                // TODO: Display quick message "response.ErrorMessage"
                return;
            }

            // Wait for online user load.
            if(OsuApi.User.Value.IsOnline)
                LoadUserData(OsuApi.User.Value);
            else
            {
                OsuApi.User.OnValueChanged += LoadUserData;
            }
        }

        /// <summary>
        /// Starts loading the user data in accordance to the currently logged-in online user.
        /// </summary>
        private void LoadUserData(IOnlineUser onlineUser, IOnlineUser _ = null)
        {
            OsuApi.User.OnValueChanged -= LoadUserData;

            if (onlineUser.IsOnline)
            {
                var progress = new ReturnableProgress<IUser>();
                progress.OnFinished += OnUserDataLoaded;
                UserManager.SetUser(onlineUser, progress);
            }
            else
            {
                // TODO: Display quick message "User is not logged in!"
                loader.Hide();
            }
        }

        /// <summary>
        /// Event called on user data load end.
        /// </summary>
        private void OnUserDataLoaded(IUser user)
        {
            if (user == null)
            {
                // TODO: Display quick message "Failed to load user data."
                OsuApi.Logout();
                return;
            }

            if (remember.Value)
            {
                GameConfiguration.Username.Value = username.Text;
                GameConfiguration.Password.Value = password.Text;
            }
            username.Text = password.Text = "";
        }
    }
}