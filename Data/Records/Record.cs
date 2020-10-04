using System;
using System.Linq;
using System.Collections.Generic;
using PBGame.Data.Users;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Scoring;
using PBGame.Rulesets.Judgements;
using PBFramework.DB.Entities;
using PBFramework.Dependencies;
using Newtonsoft.Json;

namespace PBGame.Data.Records
{
    public class Record : DatabaseEntity, IRecord {

        [JsonProperty]
        private List<JudgementRecord> judgements;

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
        public IReadOnlyList<JudgementRecord> Judgements => judgements.AsReadOnly();

        [JsonIgnore]
        public IReadOnlyDictionary<HitResultType, int> HitResultCounts => hitResultCounts;

        [JsonIgnore]
        public int HitCount => judgements.Where(j => j.IsHit).Count();

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
            return Judgements.Where(j => j.Result == result).Count();
        }

        /// <summary>
        /// Records the specified list of judgements.
        /// </summary>
        private void ExtractJudgements(List<JudgementResult> judgements)
        {
            this.judgements = new List<JudgementRecord>(judgements.Count);
            this.hitResultCounts = new Dictionary<HitResultType, int>();

            if (judgements != null)
            {
                foreach (var j in judgements)
                {
                    this.judgements.Add(new JudgementRecord()
                    {
                        Combo = j.ComboAtJudgement,
                        HitOffset = j.HitOffset,
                        IsHit = j.IsHit,
                        Result = j.HitResult
                    });

                    if(this.hitResultCounts.ContainsKey(j.HitResult))
                        this.hitResultCounts[j.HitResult]++;
                    else
                        this.hitResultCounts[j.HitResult] = 1;

                    AverageOffset += (float)j.HitOffset;
                }

                if(judgements.Count > 0)
                    AverageOffset /= judgements.Count;
            }
        }
    }
}