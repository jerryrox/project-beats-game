using System;
using PBGame.UI.Models.ProfileMenu;
using PBGame.UI.Components.Common.Dropdown;
using PBGame.Data.Users;
using PBGame.Networking.API;
using PBGame.Networking.API.Requests;
using PBGame.Networking.API.Responses;
using PBGame.Configurations;
using PBFramework.Data.Bindables;
using PBFramework.Dependencies;

namespace PBGame.UI.Models
{
    public class ProfileMenuModel : BaseModel {

        /// <summary>
        /// Event called when the login request has failed.
        /// </summary>
        public event Action OnLoginFailed;


        private DropdownContext apiDropdownContext;

        private Bindable<IApiProvider> currentProvider = new Bindable<IApiProvider>(null);
        private Bindable<IApiRequest> authRequest = new Bindable<IApiRequest>();
        private Bindable<IApiRequest> meRequest = new Bindable<IApiRequest>();
        private BindableBool isLoggingIn = new BindableBool(false);


        /// <summary>
        /// Returns the context instance for api type dropdown seleciton.
        /// </summary>
        public DropdownContext ApiDropdownContext => apiDropdownContext;

        /// <summary>
        /// Returns the current user profile.
        /// </summary>
        public IReadOnlyBindable<IUser> CurrentUser => UserManager.CurrentUser;

        /// <summary>
        /// Current authentication profile.
        /// </summary>
        public IReadOnlyBindable<Authentication> Auth => Api.Authentication;

        /// <summary>
        /// Returns the api provider currently selected.
        /// </summary>
        public IReadOnlyBindable<IApiProvider> CurrentProvider => currentProvider;

        /// <summary>
        /// Returns the currently on-going auth request, if exists.
        /// </summary>
        public IReadOnlyBindable<IApiRequest> AuthRequest => authRequest;

        /// <summary>
        /// Returns the currently on-going me request, if exists.
        /// </summary>
        public IReadOnlyBindable<IApiRequest> MeRequest => meRequest;

        /// <summary>
        /// Returns whether the user is currently loggin in.
        /// </summary>
        public IReadOnlyBindable<bool> IsLoggingIn => isLoggingIn;

        /// <summary>
        /// Returns the type of the currently selected provider.
        /// </summary>
        public IReadOnlyBindable<ApiProviderType> CurProviderType => GameConfiguration.LastLoginApi;

        /// <summary>
        /// Returns whether credentials are being saved.
        /// </summary>
        public IReadOnlyBindable<bool> IsSaveCredentials => GameConfiguration.SaveCredentials;

        /// <summary>
        /// Returns the username credential saved in the configuration.
        /// </summary>
        public IReadOnlyBindable<string> SavedUsername => GameConfiguration.Username;

        /// <summary>
        /// Returns the password credential saved in the configuration.
        /// </summary>
        public IReadOnlyBindable<string> SavedPassword => GameConfiguration.Password;

        [ReceivesDependency]
        private IApi Api { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IUserManager UserManager { get; set; }


        [InitWithDependency]
        private void Init()
        {
            apiDropdownContext = new DropdownContext();
            apiDropdownContext.ImportFromEnum<ApiProviderType>(CurProviderType.Value);
        }

        /// <summary>
        /// Selects the API provider to use.
        /// </summary>
        public void SelectApi(ApiProviderType type)
        {
            GameConfiguration.LastLoginApi.Value = type;
            GameConfiguration.Save();
        }

        /// <summary>
        /// Requests credential-based auth request.
        /// </summary>
        public void RequestCredentialAuth(string username, string password)
        {
            var request = currentProvider.Value.Auth();
            request.Username = username.Trim();
            request.Password = password.Trim();
            RequestAuth(request);
        }

        /// <summary>
        /// Toggles credential save configuration.
        /// </summary>
        public void ToggleSaveCredentials()
        {
            bool newValue = !IsSaveCredentials.Value;
            // If not saving
            if (!newValue)
            {
                GameConfiguration.Username.Value = "";
                GameConfiguration.Password.Value = "";
            }
            GameConfiguration.SaveCredentials.Value = newValue;
            GameConfiguration.Save();
        }

        /// <summary>
        /// Requests OAuth request.
        /// </summary>
        public void RequestOAuth()
        {
            var request = currentProvider.Value.OAuth();
            RequestAuth(request);
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();

            CurProviderType.BindAndTrigger(OnLastLoginApiChange);
            Auth.BindAndTrigger(OnAuthenticationChange);

            // Synchronize selection with the actual value in configuration.
            apiDropdownContext.SelectDataWithText(CurProviderType.Value.ToString());
            apiDropdownContext.OnSelection += OnApiDropdownSelection;
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            CurProviderType.OnNewValue -= OnLastLoginApiChange;
            Auth.OnNewValue -= OnAuthenticationChange;
            apiDropdownContext.OnSelection -= OnApiDropdownSelection;

            DisposeAuthRequest(false);
            DisposeMeRequest(false);
        }

        protected override void OnPostHide()
        {
            base.OnPostHide();

            isLoggingIn.Value = false;
        }

        /// <summary>
        /// Starts requesting authentication.
        /// </summary>
        private void RequestAuth(IApiRequest request)
        {
            DisposeAuthRequest(false);
            DisposeMeRequest(false);

            request.RawResponse.OnNewRawValue += OnAuthResponse;
            authRequest.Value = request;
            Api.Request(request);

            CheckIsLoggingIn();
        }

        /// <summary>
        /// Requests me request.
        /// </summary>
        private void RequestMe()
        {
            DisposeMeRequest(false);

            var request = currentProvider.Value.Me();

            request.Response.OnNewRawValue += OnMeResponse;
            meRequest.Value = request;
            Api.Request(request);

            CheckIsLoggingIn();
        }

        /// <summary>
        /// Disposes currently on-going auth request, if exists.
        /// </summary>
        private void DisposeAuthRequest(bool softDispose)
        {
            var request = authRequest.Value;
            if(request == null)
                return;
            if(softDispose)
                request.Dispose();
            authRequest.Value = null;
            CheckIsLoggingIn();
        }

        /// <summary>
        /// Disposes currently on-going me request, if exists.
        /// </summary>
        private void DisposeMeRequest(bool softDispose)
        {
            var request = meRequest.Value;
            if(request == null)
                return;
            if(softDispose)
                request.Dispose();
            meRequest.Value = null;
            CheckIsLoggingIn();
        }
        
        /// <summary>
        /// Evaluates whether the user is considered loggin in or not.
        /// </summary>
        private void CheckIsLoggingIn()
        {
            isLoggingIn.Value = authRequest.Value != null && meRequest.Value != null;
        }

        /// <summary>
        /// Saves the specified credentials to the configuration.
        /// </summary>
        private void SaveCredentials(string username, string password)
        {
            GameConfiguration.Username.Value = username;
            GameConfiguration.Password.Value = password;
            GameConfiguration.Save();
        }

        /// <summary>
        /// Event called when the last login api setting has been changed.
        /// </summary>
        private void OnLastLoginApiChange(ApiProviderType type)
        {
            currentProvider.Value = Api.GetProvider(type);

            DisposeAuthRequest(false);
            DisposeMeRequest(false);
        }

        /// <summary>
        /// Event called when the authentication state has changed.
        /// </summary>
        private void OnAuthenticationChange(Authentication newAuth)
        {
            if(newAuth != null)
                RequestMe();
        }

        /// <summary>
        /// Event called on api provider type selection change.
        /// </summary>
        private void OnApiDropdownSelection(DropdownData data)
        {
            if(data != null && data.ExtraData != null)
                SelectApi((ApiProviderType)data.ExtraData);
        }

        /// <summary>
        /// Event called when the authentication response has been returned.
        /// </summary>
        private void OnAuthResponse(object rawResponse)
        {
            var response = rawResponse as ApiResponse;
            if (response.IsSuccess)
            {
                if (authRequest.Value is AuthRequest credentialAuth)
                    SaveCredentials(credentialAuth.Username, credentialAuth.Password);
            }
            else
                OnLoginFailed?.Invoke();

            DisposeAuthRequest(true);
        }

        /// <summary>
        /// Event called when the me response has been returned.
        /// </summary>
        private void OnMeResponse(object rawResponse)
        {
            var response = rawResponse as ApiResponse;
            if (!response.IsSuccess)
                OnLoginFailed?.Invoke();

            DisposeMeRequest(true);
        }

    }
}