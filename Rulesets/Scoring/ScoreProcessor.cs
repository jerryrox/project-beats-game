using System;
using System.Collections.Generic;
using PBGame.Data.Records;
using PBGame.Rulesets.Judgements;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Objects;
using PBFramework.Data.Bindables;

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
		/// Table of cached health change factors for each hit result type.
		/// </summary>
        protected Dictionary<HitResultType, float> healthChangeFactors = new Dictionary<HitResultType, float>();

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
        protected double difficultyMultiplier;

		/// <summary>
		/// Amoutn of bonus multiplier applied to score based on the mod usage.
		/// </summary>
        protected double modMultiplier;

        /// <summary>
        /// Current amount of raw score achieved.
        /// </summary>
        protected double curRawScore;

		/// <summary>
		/// Max amount of raw score that can be achieved at certain point of the map.
		/// </summary>
        protected double maxRawScore;


        public IPlayableMap Map { get; private set; }

        public List<JudgementResult> Judgements => results;

        public BindableInt Combo { get; private set; } = new BindableInt(0);

        public BindableInt HighestCombo { get; private set; } = new BindableInt(0);

        public BindableInt Score { get; private set; } = new BindableInt(0);

        public BindableFloat Health { get; private set; } = new BindableFloat(0f);

        public BindableDouble Accuracy { get; private set; } = new BindableDouble(0f);

        public Bindable<RankType> Ranking { get; private set; } = new Bindable<RankType>(RankType.F);

        public virtual bool IsFinished => results.Count == maxJudgements;

        public virtual bool IsFailed { get; protected set; } = false;

        public int JudgeCount => results.Count;


		protected ScoreProcessor()
		{
            Combo.OnValueChanged += (combo, _) => HighestCombo.Value = Math.Max(combo, HighestCombo.Value);
            Accuracy.OnValueChanged += (acc, _) => Ranking.Value = CalculateRank(acc);

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

            // Cache health change factor
            foreach (var hitResult in (HitResultType[])Enum.GetValues(typeof(HitResultType)))
                healthChangeFactors[hitResult] = GetHealthChangeFactor(hitResult);
        }

        public virtual void ProcessJudgement(JudgementResult result)
        {
            ApplyResult(result);
            InvokeNewJudgement(result);
        }

        public virtual void Dispose()
        {
            resultCounts = null;
            healthChangeFactors = null;
            results = null;

            OnLastJudgement = null;
            OnNewJudgement = null;
        }

		/// <summary>
		/// Calculates the actual amount of score to be added for specified score.
		/// </summary>
        protected virtual int CalculateScore(int rawScore)
        {
            return rawScore + (int)(rawScore * Math.Max(0, Combo.Value) * difficultyMultiplier * modMultiplier * 0.04f);
        }

		/// <summary>
		/// Returns the amount of health application factor for specified result.
		/// </summary>
        protected abstract float GetHealthChangeFactor(HitResultType hitResult);

        /// <summary>
        /// Calculate ranking type from current score progress.
        /// </summary>
        protected RankType CalculateRank(double acc)
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
            Health.Value = healthChangeFactors[result.HitResult] * result.Judgement.GetHealthIncrease(result);
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

