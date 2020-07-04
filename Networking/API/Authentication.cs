using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PBGame.Networking.API
{
    /// <summary>
    /// Object which holds authentication state values.
    /// </summary>
    public class Authentication {

        /// <summary>
        /// The API provider which the authentication is valid for.
        /// </summary>
        [JsonProperty("provider")]
        public ApiProviderType ProviderType { get; set; }

        /// <summary>
        /// The access token to access endpoints which require authorization.
        /// </summary>
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
    }
}