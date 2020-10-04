using System.Threading.Tasks;
using PBGame.Networking.API;
using PBFramework.Data.Bindables;
using PBFramework.Threading;

namespace PBGame.Data.Users
{
    public interface IUserManager {

        /// <summary>
        /// Returns the current user in play.
        /// </summary>
        IReadOnlyBindable<IUser> CurrentUser { get; }


        /// <summary>
        /// Starts loading user data from user store.
        /// </summary>
        Task Reload(TaskListener listener = null);

        /// <summary>
        /// Switches current user to offline user.
        /// </summary>
        IUser SetUserOffline();

        /// <summary>
        /// Switches current user to the user of specified online user.
        /// </summary>
        IUser SetUser(IOnlineUser onlineUser);

        /// <summary>
        /// Saves the specified user to store.
        /// </summary>
        void SaveUser(IUser user);
    }
}