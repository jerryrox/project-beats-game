namespace PBGame.Networking.API
{
    public class OfflineUser : IOnlineUser {

        public IApiProvider Provider => null;

        public string Id => "";

        public bool IsOnline => false;

        public string Username => "Offline user";

        public string AvatarImage => "";

        public string CoverImage => "";

        public string Status => "";

        public string ProfilePage => "";
    }
}