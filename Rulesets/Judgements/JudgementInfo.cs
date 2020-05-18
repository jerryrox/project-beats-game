namespace PBGame.Rulesets.Judgements
{
	/// <summary>
	/// Information about the judgement provided by a hit object.
	/// </summary>
	public class JudgementInfo {

		/// <summary>
		/// Returns the max possible hit result from this judgement.
		/// </summary>
		public virtual HitResultType MaxResult { get { return HitResultType.Perfect; } }

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
		/// Returns the amount of scale to boost health increase for specified judgement result.
		/// </summary>
		public virtual float GetHealthBonus(JudgementResult result) { return GetHealthBonus(result.HitResult); }

		/// <summary>
		/// Returns the amount of numeric value the specified hit result type accounts for.
		/// </summary>
		public virtual int GetNumericResult(HitResultType result) { return 0; }

		/// <summary>
		/// Returns the amount of scale to boost health increase for specified hit result type.
		/// </summary>
		public virtual float GetHealthBonus(HitResultType result) { return 0; }
	}
}

