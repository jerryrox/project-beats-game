using System;
using PBGame.Rulesets;
using PBFramework.DB.Entities;

namespace PBGame.Data.Users
{
    public interface IUser : IDatabaseEntity {
    
        /// <summary>
        /// Returns the displayed name of the user.
        /// </summary>
        string Username { get; }

        /// <summary>
        /// Returns the date when the user joined the scoring system.
        /// </summary>
        DateTime JoinedDate { get; }

        /// <summary>
        /// Returns the statistics info of the game mode primarily played.
        /// </summary>
        IUserStatistics PrimaryStats { get; }


        /// <summary>
        /// Returns the statistics for specified game mode.
        /// </summary>
        IUserStatistics GetStatistics(GameModes gameMode);
    }
}