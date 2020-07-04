using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.UI.Components.Common.Dropdown;
using PBGame.Data.Users;
using PBGame.Graphics;
using PBGame.Configurations;
using PBGame.Networking.API;
using PBGame.Networking.API.Requests;
using PBGame.Networking.API.Responses;
using PBFramework.UI;
using PBFramework.Data.Bindables;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Threading;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

using Logger = PBFramework.Debugging.Logger;

namespace PBGame.UI.Components.ProfileMenu
{
    public class LoggedOutView : UguiObject, IHasAlpha
    {
        private const float BaseHeight = 104f;

        private CanvasGroup canvasGroup;

        private ISprite bg;
        private DropdownButton apiDropdown;
        private CredentialLoginView credentialLogin;
        private OAuthLoginView oAuthLogin;
        private Loader loader;

        private DropdownContext dropdownContext;

        private IApiRequest curRequest;


        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }

        /// <summary>
        /// Returns the preferred height of this view within the login content holder.
        /// </summary>
        public float DesiredHeight
        {
            get
            {
                var loginView = CurLoginView.Value;
                if(loginView == null)
                    return BaseHeight;
                return BaseHeight + loginView.DesiredHeight;
            }
        }

        /// <summary>
        /// Current login provider view in use.
        /// </summary>
        public Bindable<ILoginView> CurLoginView { get; private set; } = new Bindable<ILoginView>();

        /// <summary>
        /// Returns the provider currently selected.
        /// </summary>
        private IApiProvider CurProvider => Api.GetProvider(GameConfiguration.LastLoginApi.Value);

        [ReceivesDependency]
        private IApi Api { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IUserManager UserManager { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            Dependencies = Dependencies.Clone();
            Dependencies.Cache(this);

            canvasGroup = myObject.AddComponent<CanvasGroup>();
            
            CurLoginView.TriggerWhenDifferent = true;

            var providerLabel = CreateChild<Label>("provider-label");
            {
                providerLabel.Anchor = AnchorType.TopStretch;
                providerLabel.SetOffsetHorizontal(32f);
                providerLabel.Y = -32f;
                providerLabel.Height = 16f;
                providerLabel.IsBold = true;
                providerLabel.FontSize = 16;
                providerLabel.Alignment = TextAnchor.MiddleCenter;
                providerLabel.Text = "Select provider";
            }
            apiDropdown = CreateChild<DropdownButton>("api-dropdown");
            {
                apiDropdown.Anchor = AnchorType.TopStretch;
                apiDropdown.SetOffsetHorizontal(32f);
                apiDropdown.Y = -72f;
                apiDropdown.Height = 40f;

                dropdownContext = new DropdownContext();
                dropdownContext.ImportFromEnum<ApiProviderType>(GameConfiguration.LastLoginApi.Value);
                dropdownContext.OnSelection += (value) =>
                {
                    if(value != null && value.ExtraData != null)
                        SelectApi((ApiProviderType)value.ExtraData);
                };

                apiDropdown.Context = dropdownContext;
            }
            credentialLogin = CreateChild<CredentialLoginView>("credentials");
            {
                credentialLogin.Anchor = AnchorType.TopStretch;
                credentialLogin.Pivot = PivotType.Top;
                credentialLogin.SetOffsetHorizontal(0f);
                credentialLogin.Y = -BaseHeight;
                credentialLogin.Height = credentialLogin.DesiredHeight;
                credentialLogin.Active = false;
            }
            oAuthLogin = CreateChild<OAuthLoginView>("oauth");
            {
                oAuthLogin.Anchor = AnchorType.TopStretch;
                oAuthLogin.Pivot = PivotType.Top;
                oAuthLogin.SetOffsetHorizontal(0f);
                oAuthLogin.Y = -BaseHeight;
                oAuthLogin.Height = oAuthLogin.DesiredHeight;
                oAuthLogin.Active = false;
            }
            loader = CreateChild<Loader>("loader");
            {
                loader.Anchor = AnchorType.Fill;
                loader.Offset = new Offset(0f, BaseHeight, 0f, 0f);
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            // Listen to API authentication state change.
            Api.Authentication.OnValueChanged += OnApiAuthenticated;

            // Setup view for the currently selected API provider.
            SelectApi((ApiProviderType)dropdownContext.Selection.ExtraData);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Api.Authentication.OnValueChanged -= OnApiAuthenticated;

            FinishCurRequest(true);
        }

        /// <summary>
        /// Sets the type of API provider to use.
        /// </summary>
        public void SelectApi(ApiProviderType apiType)
        {
            GameConfiguration.LastLoginApi.Value = apiType;
            GameConfiguration.Save();

            // Current auth request must be stopped.
            FinishCurRequest(true);

            // Display OAuth or Credential login based on API information.
            var provider = Api.GetProvider(apiType);
            ILoginView loginView = null;
            if (provider.IsOAuthLogin)
            {
                loginView = oAuthLogin;
                oAuthLogin.Show();
                credentialLogin.Hide();
            }
            else
            {
                loginView = credentialLogin;
                oAuthLogin.Hide();
                credentialLogin.Show();
            }
            loginView.Setup(provider);
            CurLoginView.Value = loginView;
        }

        /// <summary>
        /// Starts authentication request routine.
        /// </summary>
        public void RequestAuth(IApiRequest request)
        {
            if(request == null)
                return;

            loader.Show();

            curRequest = request;
            request.RawResponse.OnNewRawValue += OnAuthResponse;
            Api.Request(request);
        }

        /// <summary>
        /// Requests for online Me information from API.
        /// </summary>
        private void RequestMe()
        {
            if(Api.Authentication.Value == null)
                return;

            loader.Show();
            curRequest = CurProvider.Me();
            curRequest.RawResponse.OnNewRawValue += OnMeResponse;
            Api.Request(curRequest);
        }

        /// <summary>
        /// Aborts current auth API request.
        /// </summary>
        private void FinishCurRequest(bool disposeRequest)
        {
            if(curRequest == null)
                return;

            loader.Hide();

            if(disposeRequest)
                curRequest.Dispose();
            curRequest = null;
        }

        /// <summary>
        /// Event called on initial authentication response.
        /// </summary>
        private void OnAuthResponse(object rawResponse)
        {
            FinishCurRequest(false);

            if(!(rawResponse is OAuthResponse) && !(rawResponse is AuthResponse))
            {
                Logger.LogError($"Invalid auth response type: {rawResponse.GetType().Name}");
            }
        }

        /// <summary>
        /// Event called on online Me information response.
        /// </summary>
        private void OnMeResponse(object rawResponse)
        {
            FinishCurRequest(false);

            if (!(rawResponse is MeResponse))
            {
                Logger.LogError($"Invalid me response type: {rawResponse.GetType().Name}");
            }
        }

        /// <summary>
        /// Event called when the authentication state has been changed in the API.
        /// </summary>
        private void OnApiAuthenticated(Authentication newAuth, Authentication oldAuth)
        {
            // Became authenticated.
            if (newAuth != null && newAuth != oldAuth)
                RequestMe();
        }
    }
}