namespace PBGame.Rulesets.Difficulty
{
	public abstract class DifficultyInfo {
		
		/// <summary>
		/// The game mode which the difficulty scale was calculated for.
		/// </summary>
		public abstract GameModeType GameMode { get; }

		/// <summary>
		/// The actual difficulty value of the beatmap.
		/// </summary>
		public float Scale { get; set; }

		/// <summary>
		/// Returns the categorization type of the difficulty scale.
		/// </summary>
		public DifficultyType Type
        {
            get
            {
				if(Scale < 2f) return DifficultyType.Easy;
				if(Scale < 3.25f) return DifficultyType.Normal;
				if(Scale < 4.5f) return DifficultyType.Hard;
				if(Scale < 5.75f) return DifficultyType.Insane;
                return DifficultyType.Extreme;
            }
        }
    }
}