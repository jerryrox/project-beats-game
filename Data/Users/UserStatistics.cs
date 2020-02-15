using System.Collections.Generic;
using PBGame.Data.Records;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Scoring;
using PBFramework.Dependencies;
using Newtonsoft.Json;
using UnityEngine;

namespace PBGame.Data.Users
{
    public class UserStatistics : IUserStatistics {

        /// <summary>
        /// Number of plays recorded which affects accuracy.
        /// </summary>
        [JsonProperty]
        private int playsForAccuracy;


        /// <summary>
        /// The user instance which owns this statistical info.
        /// </summary>
        [JsonIgnore]
        public User User { get; set; }

        public GameModes GameMode { get; set; }

        public int Level { get; set; } = 1;

        [JsonIgnore]
        public float ExpProgress => (float)CurExp / (float)MaxExp;

        [JsonIgnore]
        public long CurExp => TotalScore - GetRequiredScore(Level - 1);

        [JsonIgnore]
        public long MaxExp => GetRequiredScore(Level);

        public long TotalScore { get; set; }

        public long RankedScore { get; set; }

        public float Accuracy { get; set; }

        public int CompletedPlay { get; set; }

        public int PlayCount { get; set; }

        public int PlayTime { get; set; }

        public int TotalHits { get; set; }

        public int MaxCombo { get; set; }

        /// <summary>
        /// Table of number of rank types achieved.
        /// </summary>
        [JsonProperty]
        private Dictionary<RankTypes, int> RankCounts { get; set; }

        [JsonIgnore]
        [ReceivesDependency]
        private IRecordManager RecordManager { get; set; }



        public long GetRequiredScore(int level)
        {
            if(level <= 0)
                return 0;
            if(level < 100)
                return (long)(5000f / 3f * (4 * Mathf.Pow(level, 3) - 3 * Mathf.Pow(level, 2) - level) + 1.25f * Mathf.Pow(1.8f, level - 60));
            return 26931190827L + 99999999999L * (level - 100);
        }

        public int GetPlayCount(IMap map)
        {
            if(RecordManager != null && User != null)
                return RecordManager.GetPlayCount(map, User);
            return 0;
        }

        public int GetRankCount(RankTypes type) => RankCounts[type];

        public void RecordIncompletePlay(IRecord record)
        {
            AddTotalScore(record.Score);
            
            PlayCount++;
            PlayTime += record.Time;
            TotalHits += record.HitCount;

            User.Save();
        }

        public void RecordPlay(IRecord newRecord, IRecord bestRecord)
        {
            AddTotalScore(newRecord.Score);

            // If a fresh record
            if(bestRecord == null)
            {
                RankedScore += newRecord.Score;
                Accuracy = (Accuracy * playsForAccuracy + newRecord.Accuracy) / (playsForAccuracy + 1);

                playsForAccuracy++;
            }
            // If new best record
            else if (newRecord.Score > bestRecord.Score)
            {
                RankedScore = RankedScore - bestRecord.Score + newRecord.Score;
                Accuracy = (Accuracy * playsForAccuracy - bestRecord.Accuracy + newRecord.Accuracy) / playsForAccuracy;
            }

            CompletedPlay++;
            PlayCount++;
            PlayTime += newRecord.Time;
            TotalHits += newRecord.HitCount;
            MaxCombo = Mathf.Max(MaxCombo, newRecord.MaxCombo);

            User.Save();
        }

        /// <summary>
        /// Adds the specified amount of score to the total score.
        /// </summary>
        private void AddTotalScore(int score)
        {
            TotalScore += score;

            EvaluateLevel();
        }

        /// <summary>
        /// Evaluates current level.
        /// </summary>
        private void EvaluateLevel()
        {
            while (CurExp >= MaxExp)
                Level++;
        }
    }
}