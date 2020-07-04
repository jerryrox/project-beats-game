using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using PBGame.Networking.API.Osu;
using PBGame.Networking.API.Bloodcat;
using PBGame.Networking.API.Requests;
using PBGame.Networking.API.Responses;
using PBGame.Notifications;
using PBGame.Configurations;
using PBFramework.Data.Bindables;
using PBFramework.Networking;
using PBFramework.Networking.Linking;
using UnityEngine;

using Logger = PBFramework.Debugging.Logger;

namespace PBGame.Networking.API
{
    public class Api : IApi {

        private readonly static IOnlineUser OfflineUser = new OfflineUser();

        private IEnvConfiguration envConfig;
        private INotificationBox notificationBox;

        private Dictionary<ApiProviderType, IApiProvider> providers;

        private Bindable<IOnlineUser> user = new Bindable<IOnlineUser>(new OfflineUser());
        private Bindable<Authentication> authentication = new Bindable<Authentication>();


        public IReadOnlyBindable<IOnlineUser> User => user;

        public IReadOnlyBindable<Authentication> Authentication => authentication;

        public bool IsLoggedIn => user.Value.IsOnline && authentication.Value != null;

        public IApiProvider AuthenticatedProvider => IsLoggedIn ? GetProvider(authentication.Value.ProviderType) : null;


        public Api(IEnvConfiguration envConfig, INotificationBox notificationBox, DeepLinker deepLinker)
        {
            if (envConfig == null)
                throw new ArgumentNullException(nameof(envConfig));

            this.envConfig = envConfig;
            this.notificationBox = notificationBox;

            this.providers = new Dictionary<ApiProviderType, IApiProvider>()
            {
                { ApiProviderType.Osu, new OsuApiProvider(this) },
                { ApiProviderType.Bloodcat, new BloodcatApiProvider(this) },
            };

            if (deepLinker != null)
            {
                deepLinker.LinkPath("api", OnDeepLinkApi);
            }
        }

        public IApiProvider GetProvider(ApiProviderType type) => providers[type];

        public string GetUrl(IApiProvider provider, string path)
        {
            return $"{envConfig.Variables.BaseApiUrl}/{provider.InternalName}{path}";
        }

        public void Logout()
        {
            user.Value = OfflineUser;
            authentication.Value = null;
        }

        public void Request(IApiRequest request)
        {
            if (request.DidRequest)
                throw new Exception("Attempted to request an API request under the Api when it has already been requested.");

            // Attach authorization header.
            if (authentication.Value != null)
                request.InnerRequest.SetHeader("Authorization", $"Bearer {authentication.Value.AccessToken}");

            // TODO: Display request as notification.
            request.Request();
        }

        public void HandleResponse(IApiResponse response)
        {
            if(response == null)
                return;

            if (!response.IsSuccess)
            {
                notificationBox?.Add(new Notification()
                {
                    Scope = NotificationScope.Stored,
                    Message = response.ErrorMessage,
                    Type = NotificationType.Negative
                });
            }
            else
            {
                if (response is AuthResponse authResponse)
                {
                    if(authResponse.Authentication != null)
                        authentication.Value = authResponse.Authentication;
                }
                else if (response is MeResponse meResponse)
                {
                    if (meResponse.User != null)
                    {
                        meResponse.User.Provider = AuthenticatedProvider;
                        user.Value = meResponse.User;
                    }
                }
                else if (response is OAuthResponse oAuthResponse)
                {
                    if(!string.IsNullOrEmpty(oAuthResponse.OAuthUrl))
                        Application.OpenURL(oAuthResponse.OAuthUrl);
                }
            }
        }

        /// <summary>
        /// Event called from deep linker when the path indicates API response.
        /// </summary>
        private void OnDeepLinkApi(WebLink link)
        {
            if (link.Parameters.TryGetValue("response", out string response))
            {
                try
                {
                    var bytes = Convert.FromBase64String(response);
                    string decodedResponse = Encoding.UTF8.GetString(bytes);
                    var authResponse = new AuthResponse(new CustomWebResponse()
                    {
                        IsSuccess = true,
                        TextData = decodedResponse
                    });
                    authResponse.Evaluate();
                    HandleResponse(authResponse);
                }
                catch (Exception e)
                {
                    // TODO: Log
                    Logger.LogError($"Failed to parse deeplink response: {response}\n{e.ToString()}");
                }
            }
        }
    }
}