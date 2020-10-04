using System;
using System.Threading.Tasks;
using PBGame.Stores;
using PBGame.Networking.API;
using PBFramework.Data.Bindables;
using PBFramework.Threading;
using PBFramework.Dependencies;

namespace PBGame.Data.Users
{
    public class UserManager : IUserManager {

        private static User offlineUser = new User();

        private IUserStore userStore;
        private Bindable<IUser> currentUser = new Bindable<IUser>(offlineUser);
        private IDependencyContainer dependencies;


        public IReadOnlyBindable<IUser> CurrentUser => currentUser;


        public UserManager(IDependencyContainer dependencies)
        {
            if(dependencies == null) throw new ArgumentNullException(nameof(dependencies));

            this.dependencies = dependencies;

            userStore = new UserStore();
        }

        public Task Reload(TaskListener listener = null)
        {
            return Task.Run(() =>
            {
                userStore.Reload();

                listener?.SetFinished();
            });
        }

        public IUser SetUserOffline()
        {
            return currentUser.Value = offlineUser;
        }

        public IUser SetUser(IOnlineUser onlineUser)
        {
            if (onlineUser == null || !onlineUser.IsOnline)
                return SetUserOffline();

            var user = userStore.LoadUser(onlineUser) as User;
            dependencies.Inject(user);
            currentUser.Value = user;
            return user;
        }

        public void SaveUser(IUser user)
        {
            if(user == null || !user.IsOnlineUser)
                return;
            if (!(user is User u))
                throw new ArgumentException($"user must be a type of {nameof(User)}");

            // Save to store.
            userStore.SaveUser(u);
        }

        public void RemoveUser()
        {
            currentUser.Value = null;
        }
    }
}