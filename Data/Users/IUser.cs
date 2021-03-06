﻿using System;
using PBGame.Rulesets;
using PBGame.Networking.API;
using PBFramework.DB.Entities;

namespace PBGame.Data.Users
{
    public interface IUser : IDatabaseEntity {
    
        /// <summary>
        /// Returns the online user which represents this user.
        /// </summary>
        IOnlineUser OnlineUser { get; }

        /// <summary>
        /// Returns the ID of the user in online context.
        /// </summary>
        string OnlineId { get; }

        /// <summary>
        /// Returns whether this user is derived from API online user.
        /// </summary>
        bool IsOnlineUser { get; }

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
        IUserStatistics GetStatistics(GameModeType gameMode);

        /// <summary>
        /// Saves the user to the data store.
        /// </summary>
        void Save();
    }
}