using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBGame.Networking.API;
using PBFramework.UI;
using PBFramework.Data.Bindables;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

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

        [ReceivesDependency]
        private ProfileMenuModel Model { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
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

                apiDropdown.Context = Model.ApiDropdownContext;
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

            Model.CurrentProvider.BindAndTrigger(OnProviderChange);
            Model.IsLoggingIn.BindAndTrigger(OnLoggingInChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Model.CurrentProvider.OnNewValue -= OnProviderChange;
            Model.IsLoggingIn.OnNewValue -= OnLoggingInChange;
        }

        /// <summary>
        /// Event called when the selected api provider has changed.
        /// </summary>
        private void OnProviderChange(IApiProvider provider)
        {
            // Display OAuth or Credential login based on API information.
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
        /// Event called when the loggin in state has changed.
        /// </summary>
        private void OnLoggingInChange(bool isLoggingIn)
        {
            if(isLoggingIn)
                loader.Show();
            else
                loader.Hide();
        }
    }
}