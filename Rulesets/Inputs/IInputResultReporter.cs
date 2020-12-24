using System;
using PBGame.Rulesets.Judgements;

namespace PBGame.Rulesets.Inputs
{
    /// <summary>
    /// An interface designed to make ruleset-specific inputs report judgement results caused by thar particular input.
    /// </summary>
    public interface IInputResultReporter {

        /// <summary>
        /// Event called when a new result has been made due to this input.
        /// </summary>
        event Action<JudgementResult> OnResult;
    }
}