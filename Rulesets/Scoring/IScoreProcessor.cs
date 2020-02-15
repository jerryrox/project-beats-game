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
        /// Event called when the user has failed.
        /// </summary>
        event Action OnFailed;

        /// <summary>
        /// Event called to confirm whether current scoring state results in failure.
        /// The registered functions should return whether the user has failed.
        /// </summary>
        event Func<IScoreProcessor, bool> OnFailConfirmation;


        /// <summary>
        /// Returns the map which the score has been processed for.
        /// </summary>
        IMap Map { get; }

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
        BindableDouble Accuracy { get; }

        /// <summary>
        /// Returns the bindable rank value.
        /// </summary>
        Bindable<RankTypes> Ranking { get; }

        /// <summary>
        /// Returns whether all judgements have been made on all hit objects.
        /// </summary>
        bool IsFinished { get; }    

        /// <summary>
        /// Returns whether the user has failed.
        /// </summary>
        bool IsFailed { get; }

        /// <summary>
        /// Returns the current number of judgements made.
        /// </summary>
        int JudgeCount { get; }


        /// <summary>
        /// Apply any changes to the score processing from specified map.
        /// </summary>
        void ApplyMap(IMap map);

        /// <summary>
        /// Processes the specified judgement for scoring.
        /// </summary>
        void ProcessJudgement(JudgementResult result);
    }
}