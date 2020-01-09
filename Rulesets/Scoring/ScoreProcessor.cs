using System;
using System.Collections.Generic;
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

		public event Action OnFailed;

        public event Func<IScoreProcessor, bool> OnFailConfirmation;


		/// <summary>
		/// Table of counters for each hit result type.
		/// </summary>
		protected Dictionary<HitResults, int> resultCounts = new Dictionary<HitResults, int>();

		/// <summary>
		/// Table of cached health change factors for each hit result type.
		/// </summary>
        protected Dictionary<HitResults, float> healthChangeFactors = new Dictionary<HitResults, float>();

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


        public BindableInt Combo { get; private set; } = new BindableInt(0);

        public BindableInt HighestCombo { get; private set; } = new BindableInt(0);

        public BindableInt Score { get; private set; } = new BindableInt(0);

        public BindableFloat Health { get; private set; } = new BindableFloat(1f);

        public BindableDouble Accuracy { get; private set; } = new BindableDouble(0f);

        public Bindable<RankTypes> Ranking { get; private set; } = new Bindable<RankTypes>(RankTypes.F);

        public virtual bool IsFinished => results.Count == maxJudgements;

        public virtual bool IsFailed { get; protected set; }

		public int JudgeCount => results.Count;


		protected ScoreProcessor()
		{
            Combo.OnValueChanged += (combo, _) => HighestCombo.Value = Math.Max(combo, HighestCombo.Value);
            Accuracy.OnValueChanged += (acc, _) => Ranking.Value = CalculateRank(acc);
            OnFailConfirmation += (processor) => Health.Value <= 0f;

			resultCounts[HitResults.Miss] = 0;
			resultCounts[HitResults.Bad] = 0;
			resultCounts[HitResults.Ok] = 0;
			resultCounts[HitResults.Good] = 0;
			resultCounts[HitResults.Great] = 0;
			resultCounts[HitResults.Perfect] = 0;

            curRawScore = 0;
            maxRawScore = 0;
        }

		public virtual void RetrieveScores(ScoreRecord record)
		{
			record.HighestCombo = HighestCombo.Value;
			record.Score = Score.Value;
			record.Accuracy = Accuracy.Value;
			record.Rank = Ranking.Value;

			for(int i=0; i<results.Count; i++)
			{
				var result = results[i];
				record.Judgements.Add(new JudgementRecord() {
					Result = result.HitResult,
					HitOffset = result.HitOffset,
					Combo = result.ComboAtJudgement,
					IsHit = result.IsHit
				});
			}
		}

        public virtual void ApplyMap(IMap map)
        {
			// Get difficulty multiplier.
			// Converted beatmaps will only have one difficulty info so we'll just refer to the first item.
			difficultyMultiplier = map.Difficulties[0].Scale;

			// TODO: Apply mod multiplier.
			modMultiplier = 1f;

            // Cache health change factor
            foreach (var hitResult in (HitResults[])Enum.GetValues(typeof(HitResults)))
                healthChangeFactors[hitResult] = GetHealthChangeFactor(hitResult);
        }

        public virtual void ProcessJudgement(JudgementResult result)
        {
            ApplyResult(result);
            CheckFail();
            InvokeNewJudgement(result);
        }

        public virtual void Dispose()
        {
            resultCounts = null;
            healthChangeFactors = null;
            results = null;

            OnLastJudgement = null;
            OnNewJudgement = null;
            OnFailed = null;
            OnFailConfirmation = null;
        }

		/// <summary>
		/// Checks for fail condition.
		/// </summary>
        protected void CheckFail()
        {
			if(IsFailed || OnFailConfirmation == null)
				return;
			if(!OnFailConfirmation.Invoke(this))
				return;
				
			IsFailed = true;
			if(OnFailed != null)
				OnFailed();
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
        protected abstract float GetHealthChangeFactor(HitResults hitResult);

        /// <summary>
        /// Calculate ranking type from current score progress.
        /// </summary>
        protected RankTypes CalculateRank(double acc)
		{
			if(acc == 1d)
				return RankTypes.X;
			else if(acc > 0.95)
				return RankTypes.S;
			else if(acc > 0.9)
				return RankTypes.A;
			else if(acc > 0.8)
				return RankTypes.B;
			else if(acc > 0.7)
				return RankTypes.C;
			return RankTypes.D;
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
			if(result.HitResult != HitResults.None)
                resultCounts[result.HitResult] ++;

            // Change combo
            if (result.Judgement.AffectsCombo)
            {
				if(result.HitResult == HitResults.Miss)
                    Combo.Value = 0;
				else if(result.HitResult != HitResults.None)
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

