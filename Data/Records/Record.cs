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


        [Indexed]
        public Guid UserId { get; set; }

        [Indexed]
        public string MapHash { get; set; }

        [Indexed]
        public GameModes GameMode { get; set; }

        public string Username { get; set; }

        public RankTypes Rank { get; set; }

        public int Score { get; set; }

        public int MaxCombo { get; set; }

        public float Accuracy { get; set; }

        [JsonIgnore]
        public IReadOnlyList<JudgementRecord> Judgements => judgements.AsReadOnly();

        public int Time { get; set; }

        public float AverageOffset { get; set; }

        public DateTime Date { get; set; }

        [JsonIgnore]
        public bool HasReplay => RecordManager == null ? false : RecordManager.HasReplay(this);

        [JsonIgnore]
        public bool IsClear => Rank != RankTypes.F;

        [JsonIgnore]
        [ReceivesDependency]
        private IRecordManager RecordManager { get; set; }


        public Record() { }

        /// <summary>
        /// Constructor for a new record.
        /// </summary>
        public Record(IMap map, IUser user, IScoreProcessor scoreProcessor, int playTime)
        {
            if(map == null) throw new ArgumentNullException(nameof(map));

            InitializeAsNew();

            if (user == null)
            {
                UserId = Guid.Empty;
                Username = "";
            }
            else
            {
                UserId = user.Id;
                Username = user.Username;
            }

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

        public int GetHitCount(HitResults result)
        {
            return Judgements.Where(j => j.Result == result).Count();
        }

        /// <summary>
        /// Records the specified list of judgements.
        /// </summary>
        private void ExtractJudgements(List<JudgementResult> judgements)
        {
            this.judgements = new List<JudgementRecord>(judgements.Count);

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

                    AverageOffset += (float)j.HitOffset;
                }

                if(judgements.Count > 0)
                    AverageOffset /= judgements.Count;
            }
        }
    }
}