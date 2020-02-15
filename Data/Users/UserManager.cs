using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using PBGame.Stores;
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

        public Task SetUser(string username, IReturnableProgress<IUser> progress)
        {
            progress?.Report(0f);
            return Task.Run(() =>
            {
                var user = userStore.LoadUser(username);

                UnityThreadService.DispatchUnattended(() =>
                {
                    dependencies.Inject(user);
                    currentUser.Value = user;

                    if (progress != null)
                    {
                        progress.Report(1f);
                        progress.InvokeFinished(user);
                    }
                    return null;
                });
            });
        }

        public void RemoveUser()
        {
            currentUser.Value = null;
        }
    }
}