using System;
using System.Collections.Generic;
using PBGame.Data.Users;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Scoring;
using PBGame.Rulesets.Judgements;
using PBFramework.DB.Entities;
using Newtonsoft.Json;

namespace PBGame.Data.Records
{
    public class Record : DatabaseEntity, IRecord {

        [JsonProperty]
        private Dictionary<HitResultType, int> hitResultCounts;


        [Indexed]
        public Guid UserId { get; set; }

        [Indexed]
        public string MapHash { get; set; }

        [Indexed]
        public GameModeType GameMode { get; set; }

        public string Username { get; set; }

        public string AvatarUrl { get; set; }

        public RankType Rank { get; set; }

        public int Score { get; set; }

        public int MaxCombo { get; set; }

        public float Accuracy { get; set; }

        [JsonIgnore]
        public IReadOnlyDictionary<HitResultType, int> HitResultCounts => hitResultCounts;

        public int HitCount { get; set; }

        public int Time { get; set; }

        public float AverageOffset { get; set; }

        public DateTime Date { get; set; }

        [JsonIgnore]
        public bool IsClear => Rank != RankType.F;


        public Record() { }

        /// <summary>
        /// Constructor for a new record.
        /// </summary>
        public Record(IPlayableMap map, IUser user, IScoreProcessor scoreProcessor, int playTime)
        {
            if(map == null)
                throw new ArgumentNullException(nameof(map));
            if(user == null)
                throw new ArgumentNullException(nameof(user));

            InitializeAsNew();

            UnityEngine.Debug.LogWarning("Created new record with id: " + Id.ToString());

            UserId = user.Id;
            Username = user.Username;
            AvatarUrl = user.OnlineUser?.AvatarImage ?? "";

            MapHash = map.Detail.Hash;
            GameMode = map.PlayableMode;
            Rank = scoreProcessor.Ranking.Value;
            Score = scoreProcessor.Score.Value;
            MaxCombo = scoreProcessor.HighestCombo.Value;
            Accuracy = (float)scoreProcessor.Accuracy.Value;
            Time = playTime;
            Date = DateTime.Now;

            ExtractJudgements(scoreProcessor.Judgements);
        }

        public int GetHitCount(HitResultType result)
        {
            if (HitResultCounts.TryGetValue(result, out int count))
                return count;
            return 0;
        }

        /// <summary>
        /// Extracts certain data from the list of judgements.
        /// </summary>
        private void ExtractJudgements(List<JudgementResult> judgements)
        {
            hitResultCounts = new Dictionary<HitResultType, int>();

            if (judgements != null)
            {
                HitCount = judgements.Count;

                foreach (var j in judgements)
                {
                    if(hitResultCounts.ContainsKey(j.HitResult))
                        hitResultCounts[j.HitResult]++;
                    else
                        hitResultCounts[j.HitResult] = 1;

                    AverageOffset += (float)j.HitOffset;
                }

                if(judgements.Count > 0)
                    AverageOffset /= judgements.Count;
            }
        }
    }
}