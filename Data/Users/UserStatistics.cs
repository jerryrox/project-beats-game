using System.Collections.Generic;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Scoring;
using Newtonsoft.Json;
using UnityEngine;

namespace PBGame.Data.Users
{
    public class UserStatistics : IUserStatistics {

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

        // TODO: Receive record manager.
        // [JsonIgnore]
        // [ReceivesDependency]
        // private 



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
            // TODO: Implement using record manager.
            return 0;
        }

        public int GetRankCount(RankTypes type) => RankCounts[type];

        public void RecordIncompletePlay()
        {
        }

        public void RecordPlay()
        {
        }
    }
}