using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using PBGame.Stores;
using PBGame.Networking.API;
using PBFramework.Data.Bindables;
using PBFramework.Services;
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

        public Task Reload(IEventProgress progress)
        {
            progress?.Report(0f);
            return Task.Run(() =>
            {
                userStore.Reload();

                UnityThreadService.DispatchUnattended(() =>
                {
                    if (progress != null)
                    {
                        progress.Report(1f);
                        progress.InvokeFinished();
                    }
                    return null;
                });
            });
        }

        public Task SetUser(IOnlineUser onlineUser, IReturnableProgress<IUser> progress)
        {
            if(onlineUser == null) throw new ArgumentNullException(nameof(onlineUser));

            progress?.Report(0f);
            return Task.Run(() =>
            {
                try
                {
                    var user = userStore.LoadUser(onlineUser) as User;

                    UnityThreadService.DispatchUnattended(() =>
                    {
                        dependencies.Inject(user);
                        UnityEngine.Debug.Log($"User set: {user.Username}");
                        currentUser.Value = user;

                        if (progress != null)
                        {
                            progress.Report(1f);
                            progress.InvokeFinished(user);
                        }
                        return null;
                    });
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError($"Error whie setting user: {e.ToString()}");
                }
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