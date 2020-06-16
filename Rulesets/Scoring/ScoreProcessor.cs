using System;
using System.Collections.Generic;
using PBGame.Data.Records;
using PBGame.Rulesets.Judgements;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Objects;
using PBFramework.Data.Bindables;
using UnityEngine;

namespace PBGame.Rulesets.Scoring
{
	/// <summary>
	/// Class which processes base scoring for a game play.
	/// </summary>
	public abstract class ScoreProcessor : IScoreProcessor {

		public event Action OnLastJudgement;

		public event Action<JudgementResult> OnNewJudgement;


		/// <summary>
		/// Table of counters for each hit result type.
		/// </summary>
		protected Dictionary<HitResultType, int> resultCounts = new Dictionary<HitResultType, int>();

        /// <summary>
        /// List of all judgement results recorded for each hit object.
        /// </summary>
        protected List<JudgementResult> results = new List<JudgementResult>();

		/// <summary>
		/// The total number of judgement that can be made for current map.
		/// </summary>
        protected int maxJudgements = 0;

		/// <summary>
		/// Amount of bonus multiplier applied to score based on the map difficulty.
		/// </summary>
        protected float difficultyMultiplier;

		/// <summary>
		/// Amoutn of bonus multiplier applied to score based on the mod usage.
		/// </summary>
        protected float modMultiplier;

        /// <summary>
        /// Current amount of raw score achieved.
        /// </summary>
        protected float curRawScore;

		/// <summary>
		/// Max amount of raw score that can be achieved at certain point of the map.
		/// </summary>
        protected float maxRawScore;


        public IPlayableMap Map { get; private set; }

        public List<JudgementResult> Judgements => results;

        public BindableInt Combo { get; private set; } = new BindableInt(0);

        public BindableInt HighestCombo { get; private set; } = new BindableInt(0);

        public BindableInt Score { get; private set; } = new BindableInt(0);

        public BindableFloat Health { get; private set; } = new BindableFloat(0.5f);

        public BindableFloat Accuracy { get; private set; } = new BindableFloat(0f);

        public Bindable<RankType> Ranking { get; private set; } = new Bindable<RankType>(RankType.F);

        public virtual bool IsFinished => results.Count == maxJudgements;

        public virtual bool IsFailed => Health.Value < 0.5f;

        public int JudgeCount => results.Count;

        /// <summary>
        /// Returns the amount of health increased per perfect hit with health bonus scale of 1.
        /// </summary>
        protected abstract float HealthPerPerfect { get; }

        /// <summary>
        /// Returns the percentage of health decreased on missing a hit object.
        /// </summary>
        protected virtual float HealthDecreaseRatio { get; } = 0.10f;


        protected ScoreProcessor()
		{
            Combo.OnNewValue += (combo) => HighestCombo.Value = Math.Max(combo, HighestCombo.Value);
            Accuracy.OnNewValue += (acc) => Ranking.Value = CalculateRank(acc);

			resultCounts[HitResultType.Miss] = 0;
			resultCounts[HitResultType.Bad] = 0;
			resultCounts[HitResultType.Ok] = 0;
			resultCounts[HitResultType.Good] = 0;
			resultCounts[HitResultType.Great] = 0;
			resultCounts[HitResultType.Perfect] = 0;

            curRawScore = 0;
            maxRawScore = 0;
        }

        public virtual void ApplyMap(IPlayableMap map)
        {
			// Get difficulty multiplier.
			difficultyMultiplier = map.Difficulty.Scale;

			// TODO: Apply mod multiplier.
			modMultiplier = 1f;
        }

        public virtual void ProcessJudgement(JudgementResult result)
        {
            ApplyResult(result);
            InvokeNewJudgement(result);
        }

        public virtual void Dispose()
        {
            resultCounts = null;
            results = null;

            OnLastJudgement = null;
        }

		/// <summary>
		/// Calculates the actual amount of score to be added for specified score.
		/// </summary>
        protected virtual int CalculateScore(int rawScore)
        {
            return rawScore + (int)(rawScore * Math.Max(0, Combo.Value) * difficultyMultiplier * modMultiplier * 0.04f);
        }

        /// <summary>
        /// Calculate ranking type from current score progress.
        /// </summary>
        protected RankType CalculateRank(float acc)
		{
			if(acc == 1d)
				return RankType.X;
			else if(acc > 0.95)
				return RankType.S;
			else if(acc > 0.9)
				return RankType.A;
			else if(acc > 0.8)
				return RankType.B;
			else if(acc > 0.7)
				return RankType.C;
			return RankType.D;
		}

		/// <summary>
		/// Applies state change to the score for specified result.
		/// </summary>
        protected virtual void ApplyResult(JudgementResult result)
        {
			// Statistic values
            result.ComboAtJudgement = Combo.Value;
            result.HighestComboAtJudgement = HighestCombo.Value;

            // Store result
            results.Add(result);

			// Increase the counter for the type of hit achieved.
			if(result.HitResult != HitResultType.None)
                resultCounts[result.HitResult] ++;

            // Change combo
            if (result.Judgement.AffectsCombo)
            {
				if(result.HitResult == HitResultType.Miss)
                    Combo.Value = 0;
				else if(result.HitResult != HitResultType.None)
                    Combo.Value++;
            }

            // Get raw score of the result.
            int rawScore = result.Judgement.GetNumericResult(result);

            // Change score
            Score.Value += CalculateScore(rawScore);

            // Change accuracy
            curRawScore += rawScore;
            maxRawScore += result.Judgement.MaxNumericResult;
            Accuracy.Value = curRawScore / maxRawScore;

            // Change health
            float health = Health.Value;
            if(result.HitResult == HitResultType.Miss)
                health -= HealthDecreaseRatio * result.Judgement.GetHealthBonus(HitResultType.Perfect);
            else
                health += HealthPerPerfect * result.Judgement.GetHealthBonus(result);
            Health.Value = Mathf.Clamp01(health);
        }

        /// <summary>
        /// Invokes events related to new judgements.
        /// </summary>
        protected void InvokeNewJudgement(JudgementResult result)
		{
            OnNewJudgement?.Invoke(result);
			
			if(IsFinished)
				OnLastJudgement?.Invoke();
		}
	}
}

