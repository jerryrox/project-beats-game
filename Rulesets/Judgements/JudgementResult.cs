namespace PBGame.Rulesets.Judgements
{
	/// <summary>
	/// Judgement result made during game play based on the hit object's judgement info.
	/// </summary>
	public class JudgementResult {

        /// <summary>
        /// The result type of the judgement.
        /// </summary>
        public HitResultType HitResult { get; set; }

        /// <summary>
        /// Returns the reference judgement information provided by the hit object.
        /// </summary>
        public JudgementInfo Judgement { get; }

		/// <summary>
		/// Amount of offset from a perfect hit timing.
		/// </summary>
		public float HitOffset { get; set; }

		/// <summary>
		/// The combo before this judgement.
		/// </summary>
		public int ComboAtJudgement { get; set; }

		/// <summary>
		/// The highest combo before this judgement.
		/// </summary>
		public int HighestComboAtJudgement { get; set; }

		/// <summary>
		/// Returns whether judgement result has been evaluated for this object
		/// (Hit or a miss)
		/// </summary>
		public bool HasResult { get { return HitResult != HitResultType.None; } }

		/// <summary>
		/// Returns whether this judgement result was a successful hit.
		/// </summary>
		public bool IsHit { get { return HitResult != HitResultType.None && HitResult != HitResultType.Miss; } }


		public JudgementResult(JudgementInfo judegement)
		{
			Judgement = judegement;
			HitResult = HitResultType.None;

            Reset();
        }

		/// <summary>
		/// Resets the properties to inital values.
		/// </summary>
        public void Reset()
        {
            HitResult = HitResultType.None;
            HitOffset = 0f;
            ComboAtJudgement = 0;
            HighestComboAtJudgement = 0;
        }
    }
}

