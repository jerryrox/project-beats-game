namespace PBGame.Rulesets.Judgements
{
	/// <summary>
	/// Information about the judgement provided by a hit object.
	/// </summary>
	public class JudgementInfo {

		/// <summary>
		/// Returns the max possible hit result from this judgement.
		/// </summary>
		public virtual HitResults MaxResult { get { return HitResults.Perfect; } }

		/// <summary>
		/// Returns whether this judgement should affect the combo.
		/// </summary>
		public virtual bool AffectsCombo { get { return true; } }

		/// <summary>
		/// Returns the numeric value awarded for the max possible hit result.
		/// </summary>
		public int MaxNumericResult { get { return GetNumericResult(MaxResult); } }


		/// <summary>
		/// Returns the amount of numeric value accounted for specified judgement result.
		/// </summary>
		public virtual int GetNumericResult(JudgementResult result) { return GetNumericResult(result.HitResult); }

		/// <summary>
		/// Returns the amount of health to increase for specified judgement result.
		/// </summary>
		public virtual float GetHealthIncrease(JudgementResult result) { return GetHealthIncrease(result.HitResult); }

		/// <summary>
		/// Returns the amount of numeric value the specified hit result type accounts for.
		/// </summary>
		protected virtual int GetNumericResult(HitResults result) { return 0; }

		/// <summary>
		/// Returns the amount of health to increase for specified hit result type.
		/// </summary>
		protected virtual float GetHealthIncrease(HitResults result) { return 0; }
	}
}

