using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets;
using PBGame.Networking.API;
using PBFramework.DB.Entities;
using PBFramework.Dependencies;
using Newtonsoft.Json;

namespace PBGame.Data.Users
{
    public class User : DatabaseEntity, IUser {

        /// <summary>
        /// Table of statistic information for each game mode.
        /// </summary>
        [JsonProperty]
        private Dictionary<GameModeType, UserStatistics> statistics { get; set; }


        [JsonIgnore]
        public IOnlineUser OnlineUser { get; set; }

        [Indexed]
        public string OnlineId { get; set; }

        [JsonIgnore]
        public string Username => OnlineUser.Username;

        public DateTime JoinedDate { get; set; }

        [JsonIgnore]
        public IUserStatistics PrimaryStats
        {
            get
            {
                // Return the stats with the highest play time.
                var stats = statistics.Values.OrderByDescending(s => s.PlayTime).FirstOrDefault();
                return stats;
            }
        }

        [JsonIgnore]
        [ReceivesDependency]
        private IUserManager UserManager { get; set; }


        public User() {}

        /// <summary>
        /// Constructor for a new user data.
        /// </summary>
        public User(IOnlineUser onlineUser)
        {
            InitializeAsNew();

            OnlineUser = onlineUser;
            OnlineId = onlineUser.Id;
            JoinedDate = DateTime.Now;
            statistics = new Dictionary<GameModeType, UserStatistics>();
        }

        [InitWithDependency]
        private void Init(IModeManager modeManager, IDependencyContainer dependency)
        {
            // Create user statistics for missing game modes using mode manager.
            foreach (var mode in modeManager.PlayableServices())
            {
                if (!statistics.ContainsKey(mode.GameMode))
                {
                    statistics[mode.GameMode] = new UserStatistics()
                    {
                        GameMode = mode.GameMode
                    };
                }
            }

            // Inject dependencies to all user statistics.
            foreach (var s in statistics.Values)
            {
                s.User = this;
                dependency.Inject(s);
            }
        }

        public IUserStatistics GetStatistics(GameModeType gameMode)
        {
            if(statistics.TryGetValue(gameMode, out UserStatistics value))
                return value;
            return PrimaryStats;
        }

        public void Save() => UserManager.SaveUser(this);
    }
}