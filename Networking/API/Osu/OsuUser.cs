namespace PBGame.Networking.API.Osu
{
    public class OsuUser : IOnlineUser {

		public IApi Api { get; set; }

        public string Id { get; set; }

		public bool IsOnline => true;

		public string Username { get; set; }

		public string AvatarImage { get; set; }

		public string CoverImage { get; set; }

		public string Status { get; set; }

		public string ProfilePage { get; set; }
    }
}