using System;
using System.Collections.Generic;
using PBGame.Data.Records;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Judgements;
using PBFramework.Data.Bindables;

namespace PBGame.Rulesets.Scoring
{
    public interface IScoreProcessor : IDisposable {

        /// <summary>
        /// Event called when the last judgement has been made.
        /// </summary>
        event Action OnLastJudgement;

        /// <summary>
        /// Event called when a new judgement has been made.
        /// </summary>
        event Action<JudgementResult> OnNewJudgement;


        /// <summary>
        /// Returns the map which the score has been processed for.
        /// </summary>
        IPlayableMap Map { get; }

        /// <summary>
        /// Returns the list of all judgements currently made.
        /// </summary>
        List<JudgementResult> Judgements { get; }

        /// <summary>
        /// Returns the bindable combo value.
        /// </summary>
        BindableInt Combo { get; }

        /// <summary>
        /// Returns the bindable highest combo value.
        /// </summary>
        BindableInt HighestCombo { get; }

        /// <summary>
        /// Returns the bindable highest score value.
        /// </summary>
        BindableInt Score { get; }

        /// <summary>
        /// Returns the bindable health value.
        /// </summary>
        BindableFloat Health { get; }

        /// <summary>
        /// Returns the bindable accuracy value.
        /// </summary>
        BindableFloat Accuracy { get; }

        /// <summary>
        /// Returns the bindable rank value.
        /// </summary>
        Bindable<RankType> Ranking { get; }

        /// <summary>
        /// Returns whether all judgements have been made on all hit objects.
        /// </summary>
        bool IsFinished { get; }

        /// <summary>
        /// Returns the whether the player has failed to achieve the success criteria.
        /// </summary>
        bool IsFailed { get; }

        /// <summary>
        /// Returns the current number of judgements made.
        /// </summary>
        int JudgeCount { get; }


        /// <summary>
        /// Returns the minimum accuracy required to achieve the specified rank.
        /// </summary>
        float GetRankAccuracy(RankType type);

        /// <summary>
        /// Apply any changes to the score processing from specified map.
        /// </summary>
        void ApplyMap(IPlayableMap map);

        /// <summary>
        /// Processes the specified judgement for scoring.
        /// </summary>
        void ProcessJudgement(JudgementResult result);

        /// <summary>
        /// Returns the rank type for the specified accuracy.
        /// </summary>
        RankType CalculateRank(float acc);
    }
}