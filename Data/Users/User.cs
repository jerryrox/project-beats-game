using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets;
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
        private Dictionary<GameModes, UserStatistics> statistics { get; set; }

    
        [Indexed]
        public string Username { get; set; }

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


        public User() {}

        /// <summary>
        /// Constructor for a new user data.
        /// </summary>
        public User(string username)
        {
            InitializeAsNew();

            Username = username;
            JoinedDate = DateTime.Now;
            statistics = new Dictionary<GameModes, UserStatistics>();
        }

        [InitWithDependency]
        private void Init(IModeManager modeManager, IDependencyContainer dependency)
        {
            // Create user statistics for missing game modes using mode manager.
            foreach (var mode in modeManager.AllServices())
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

        public IUserStatistics GetStatistics(GameModes gameMode) => statistics[gameMode];
    }
}