using System;
using System.Collections.Generic;
using PBGame.Rulesets;
using PBGame.Rulesets.Scoring;
using PBGame.Rulesets.Judgements;
using PBFramework.DB.Entities;

namespace PBGame.Data.Records
{
    // TODO: Store mods used.
    public interface IRecord : IDatabaseEntity {

        /// <summary>
        /// Returns the ID of the user who made this record.
        /// </summary>
        Guid UserId { get; }

        /// <summary>
        /// Returns the hashcode of the map.
        /// </summary>
        string MapHash { get; }

        /// <summary>
        /// Returns the game mode this record was made in.
        /// </summary>
        GameModeType GameMode { get; }

        /// <summary>
        /// Returns the name of the user that achieved the record.
        /// </summary>
        string Username { get; }

        /// <summary>
        /// Returns the url to the user's profile image.
        /// </summary>
        string AvatarUrl { get; }

        /// <summary>
        /// Returns the rank achieved.
        /// </summary>
        RankType Rank { get; }

        /// <summary>
        /// Returns the score achieved.
        /// </summary>
        int Score { get; }

        /// <summary>
        /// Returns the highest combo achieved in this record.
        /// </summary>
        int MaxCombo { get; }

        /// <summary>
        /// Returns the accuracy of the play.
        /// </summary>
        float Accuracy { get; }

        /// <summary>
        /// List of judgements made.
        /// </summary>
        IReadOnlyList<JudgementRecord> Judgements { get; }

        /// <summary>
        /// Returns the number of hits recorded for each hit result types.
        /// </summary>
        IReadOnlyDictionary<HitResultType, int> HitResultCounts { get; }

        /// <summary>
        /// Returns the number of hit judgements made.
        /// </summary>
        int HitCount { get; }

        /// <summary>
        /// Returns the number of seconds spent on this record.
        /// </summary>
        int Time { get; }

        /// <summary>
        /// Returns the average offset of hitobject hits.
        /// </summary>
        float AverageOffset { get; }

        /// <summary>
        /// Returns the date & time when this record was made.
        /// </summary>
        DateTime Date { get; }

        /// <summary>
        /// Returns whether this record is a complete play.
        /// </summary>
        bool IsClear { get; }


        /// <summary>
        /// Returns the number of hits made for specified hit result.
        /// </summary>
        int GetHitCount(HitResultType result);
    }
}