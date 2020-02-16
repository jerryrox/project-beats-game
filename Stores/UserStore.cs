using System;
using System.Linq;
using PBGame.IO;
using PBGame.Data.Users;
using PBGame.Networking.API;
using PBFramework.DB;
using PBFramework.Stores;

namespace PBGame.Stores
{
    public class UserStore : DatabaseBackedStore<User>, IUserStore {


        public IUser LoadUser(IOnlineUser onlineUser)
        {
            var id = onlineUser.Id;

            using (var result = Database.Query()
                .Where(data => data["OnlineId"].ToString().Equals(id, StringComparison.Ordinal))
                .GetResult())
            {
                var user = result.FirstOrDefault();
                // If no match, create and return a new user.
                if (user == null)
                    return CreateUser(onlineUser);
                // Else, return the result.
                else
                {
                    user.OnlineUser = onlineUser;
                    return user;
                }
            }
        }

        public void SaveUser(User user)
        {
            Database.Edit().Write(user).Commit();
        }

        protected override IDatabase<User> CreateDatabase()
        {
            return new Database<User>(GameDirectory.Users);
        }

        /// <summary>
        /// Creates a new user data.
        /// </summary>
        private User CreateUser(IOnlineUser onlineUser)
        {
            return new User(onlineUser);
        }
    }
}