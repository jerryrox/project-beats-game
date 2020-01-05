using System;
using System.Collections.Generic;
using PBGame.Rulesets.Judgements;

namespace PBGame.Rulesets.Scoring
{
	/// <summary>
	/// Represents a recorded score detail extracted from a game play.
	/// </summary>
	public class ScoreRecord {

		/// <summary>
		/// The highest combo achieved in the play.
		/// </summary>
		public int HighestCombo { get; set; }

		/// <summary>
		/// The total score earned.
		/// </summary>
		public int Score { get; set; }

		/// <summary>
		/// Overall hit accuracy.
		/// </summary>
		public double Accuracy { get; set; }

		/// <summary>
		/// The timestamp which the record was made on.
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// The rank type achieved for the judgement.
		/// </summary>
		public RankTypes Rank { get; set; }

        /// <summary>
        /// List of all judgement records on play.
        /// </summary>
        public List<JudgementRecord> Judgements { get; private set; } = new List<JudgementRecord>();

        // TODO: Implement when replay should be supported.
        /// <summary>
        /// List of frames recorded to be used for replays.
        /// </summary>
        //		public List<ReplayFrame> Frames;


        public ScoreRecord()
		{
			Date = DateTime.Now;
		}
	}
}