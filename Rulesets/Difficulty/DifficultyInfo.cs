namespace PBGame.Rulesets.Difficulty
{
	public abstract class DifficultyInfo {
		
		/// <summary>
		/// The game mode which the difficulty scale was calculated for.
		/// </summary>
		public abstract GameModes GameMode { get; }

		/// <summary>
		/// The actual difficulty value of the beatmap.
		/// </summary>
		public float Scale { get; set; }
	}
}