using PBGame.Data.Users;
using PBGame.Networking.API;
using PBFramework.Stores;

namespace PBGame.Stores
{
    public interface IUserStore : IDatabaseBackedStore<User> {

        /// <summary>
        /// Loads and returns the user of specified online id.
        /// </summary>
        IUser LoadUser(IOnlineUser onlineUser);

        /// <summary>
        /// Saves the specified user to the database.
        /// </summary>
        void SaveUser(User user);
    }
}