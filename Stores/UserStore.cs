using System;
using System.Linq;
using PBGame.IO;
using PBGame.Data.Users;
using PBFramework.DB;
using PBFramework.Stores;

namespace PBGame.Stores
{
    public class UserStore : DatabaseBackedStore<User>, IUserStore {


        public IUser LoadUser(string username)
        {
            using (var result = Database.Query()
                .Where(data => data["Username"].ToString().Equals(username, StringComparison.Ordinal))
                .GetResult())
            {
                var user = result.FirstOrDefault();
                // If no match, create and return a new user.
                if (user == null)
                    return CreateUser(username);
                // Else, return the result.
                else
                    return user;
            }
        }

        protected override IDatabase<User> CreateDatabase()
        {
            return new Database<User>(GameDirectory.Users);
        }

        /// <summary>
        /// Creates a new user data.
        /// </summary>
        private User CreateUser(string username)
        {
            return new User(username);
        }
    }
}