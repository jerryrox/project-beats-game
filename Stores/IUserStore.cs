using PBGame.Data.Users;
using PBFramework.Stores;

namespace PBGame.Stores
{
    public interface IUserStore : IDatabaseBackedStore<User> {

        /// <summary>
        /// Loads and returns the user of specified username.
        /// </summary>
        IUser LoadUser(string username);
    }
}