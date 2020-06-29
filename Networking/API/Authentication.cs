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
        /// The index of the API provider which the authentication is valid for.
        /// </summary>
        [JsonProperty("provider")]
        public int ProviderIndex { get; set; }

        /// <summary>
        /// Returns the type of the api provider corresponding to the ProviderIndex.
        /// </summary>
        [JsonIgnore]
        public ApiProviderType ProviderType => (ApiProviderType)ProviderIndex;

        /// <summary>
        /// The access token to access endpoints which require authorization.
        /// </summary>
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
    }
}