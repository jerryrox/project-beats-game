using Newtonsoft.Json;

namespace PBGame.Networking.API
{
    public class OnlineUser : IOnlineUser {

        public IApiProvider Provider { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        public bool IsOnline => true;

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("avatarImage")]
        public string AvatarImage { get; set; }

        [JsonProperty("coverImage")]
        public string CoverImage { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("profilePage")]
        public string ProfilePage { get; set; }
    }
}