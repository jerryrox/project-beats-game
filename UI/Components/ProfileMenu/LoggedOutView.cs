using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.UI.Components.Common.Dropdown;
using PBGame.Data.Users;
using PBGame.Graphics;
using PBGame.Configurations;
using PBGame.Networking.API;
using PBGame.Networking.API.Responses;
using PBFramework.UI;
using PBFramework.Data.Bindables;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Threading;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

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

        private DropdownContext dropdownContext;


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

        private IApi OsuApi => ApiManager.GetApi(ApiProviderType.Osu);

        [ReceivesDependency]
        private IApiManager ApiManager { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IUserManager UserManager { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            canvasGroup = myObject.AddComponent<CanvasGroup>();
            
            CurLoginView.TriggerWhenDifferent = true;

            bg = CreateChild<UguiSprite>("bg");
            {
                bg.Anchor = AnchorType.Fill;
                bg.RawSize = Vector2.zero;
                bg.Color = HexColor.Create("1D2126");
            }
            var providerLabel = CreateChild<Label>("provider-label");
            {
                providerLabel.Anchor = AnchorType.TopStretch;
                providerLabel.SetOffsetHorizontal(32f);
                providerLabel.Y = -32f;
                providerLabel.Height = 0f;
                providerLabel.IsBold = true;
                providerLabel.Alignment = TextAnchor.MiddleCenter;
            }
            apiDropdown = CreateChild<DropdownButton>("api-dropdown");
            {
                apiDropdown.Anchor = AnchorType.TopStretch;
                apiDropdown.SetOffsetHorizontal(32f);
                apiDropdown.Y = -72f;
                apiDropdown.Height = 40f;

                dropdownContext = new DropdownContext();
                dropdownContext.OnSelection += (value) =>
                {
                    SelectApi((ApiProviderType)value.ExtraData);
                };
                dropdownContext.ImportFromEnum<ApiProviderType>(GameConfiguration.LastLoginApi.Value);

                apiDropdown.Context = dropdownContext;
            }
            credentialLogin = CreateChild<CredentialLoginView>("credentials");
            {
                credentialLogin.Anchor = AnchorType.TopStretch;
                credentialLogin.Pivot = PivotType.Top;
                credentialLogin.Y = -BaseHeight;
                credentialLogin.Height = credentialLogin.DesiredHeight;
                credentialLogin.Active = false;
            }
            oAuthLogin = CreateChild<OAuthLoginView>("oauth");
            {
                oAuthLogin.Anchor = AnchorType.TopStretch;
                oAuthLogin.Pivot = PivotType.Top;
                oAuthLogin.Y = -BaseHeight;
                oAuthLogin.Height = oAuthLogin.DesiredHeight;
                oAuthLogin.Active = false;
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            // TODO: Bind to online user state change.

            // Setup view for the currently selected API provider.
            SelectApi((ApiProviderType)dropdownContext.Selection.ExtraData);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            // TODO: Unbind from online user state change.
        }

        /// <summary>
        /// Sets the type of API provider to use.
        /// </summary>
        public void SelectApi(ApiProviderType apiType)
        {
            GameConfiguration.LastLoginApi.Value = apiType;
            GameConfiguration.Save();

            var api = ApiManager.GetApi(apiType);
            // TODO: Display OAuth or Credential login based on API information.
            // CurLoginView.Value = 
        }

        // /// <summary>
        // /// Starts loading the user data in accordance to the currently logged-in online user.
        // /// </summary>
        // private void LoadUserData(IOnlineUser onlineUser, IOnlineUser _ = null)
        // {
        //     if (onlineUser.IsOnline)
        //     {
        //         var progress = new ReturnableProgress<IUser>();
        //         progress.OnFinished += OnUserDataLoaded;
        //         UserManager.SetUser(onlineUser, progress);
        //     }
        //     else
        //     {
        //         loader.Hide();
        //     }
        // }
    }
}