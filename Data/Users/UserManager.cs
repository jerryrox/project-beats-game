using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using PBGame.Stores;
using PBGame.Networking.API;
using PBFramework.Data.Bindables;
using PBFramework.Threading;
using PBFramework.Dependencies;

namespace PBGame.Data.Users
{
    public class UserManager : IUserManager {

        private IUserStore userStore;
        private Bindable<IUser> currentUser;
        private IDependencyContainer dependencies;


        public IReadOnlyBindable<IUser> CurrentUser => currentUser;


        public UserManager(IDependencyContainer dependencies)
        {
            if(dependencies == null) throw new ArgumentNullException(nameof(dependencies));

            this.dependencies = dependencies;

            userStore = new UserStore();
            currentUser = new Bindable<IUser>();
        }

        public Task Reload(TaskListener listener = null)
        {
            return Task.Run(() =>
            {
                userStore.Reload();

                listener?.SetFinished();
            });
        }

        public Task<IUser> SetUser(IOnlineUser onlineUser, TaskListener<IUser> listener = null)
        {
            if(onlineUser == null)
                throw new ArgumentNullException(nameof(onlineUser));
            return Task.Run<IUser>(() =>
            {
                var user = userStore.LoadUser(onlineUser) as User;

                UnityThread.DispatchUnattended(() =>
                {
                    dependencies.Inject(user);
                    currentUser.Value = user;

                    listener?.SetFinished(user);
                    return null;
                });
                return user;
            });
        }

        public void SaveUser(IUser user)
        {
            if(user == null) return;
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