using System.Threading.Tasks;
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
        Task Reload(IEventProgress progress);

        /// <summary>
        /// Switches current user to the user of specified username.
        /// </summary>
        Task SetUser(string username, IReturnableProgress<IUser> progress);

        /// <summary>
        /// Saves the specified user to store.
        /// </summary>
        void SaveUser(IUser user);

        /// <summary>
        /// Removes current user.
        /// </summary>
        void RemoveUser();
    }
}