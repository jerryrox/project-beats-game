namespace PBGame.Rulesets.Judgements
{
	/// <summary>
	/// Judgement record used for storing scores.
	/// </summary>
	public class JudgementRecord {

		/// <summary>
		/// The result type of the judgement.
		/// </summary>
		public HitResults Result { get; set; }

		/// <summary>
		/// The amount of offset from a perfect hit timing.
		/// </summary>
		public double HitOffset { get; set; }

		/// <summary>
		/// The combo before the judgement.
		/// </summary>
		public int Combo { get; set; }

		/// <summary>
		/// Whether the result was not a miss.
		/// </summary>
		public bool IsHit { get; set; }
	}
}

