namespace PBGame.Networking.API
{
    public class OfflineUser : IOnlineUser {

        public int Id => 0;

        public string Username => "Offline user";

        public string AvatarImage => "";

        public string CoverImage => "";

        public string Status => "";

        public string ProfilePage => "";
    }
}