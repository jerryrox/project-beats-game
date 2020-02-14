namespace PBGame.Networking.API
{
    public interface IOnlineUser {
        
		/// <summary>
		/// Returns the identifier of the user in the server.
		/// </summary>
		int Id { get; }

		/// <summary>
		/// Whether this user is a logged in user.
		/// </summary>
		bool IsOnline { get; }

        /// <summary>
        /// The username provided by user.
        /// </summary>
        string Username { get; }

		/// <summary>
		/// Returns the url of the user's avatar.
		/// </summary>
		string AvatarImage { get; }

		/// <summary>
		/// Returns the url of the user's cover image.
		/// </summary>
		string CoverImage { get; }

		/// <summary>
		/// Returns the status message of the user.
		/// </summary>
		string Status { get; }

		/// <summary>
		/// Returns the url of the user's profile.
		/// </summary>
		string ProfilePage { get; }
	}
}

