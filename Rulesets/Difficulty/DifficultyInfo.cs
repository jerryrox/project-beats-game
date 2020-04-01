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

		/// <summary>
		/// Returns the categorization type of the difficulty scale.
		/// </summary>
		public DifficultyTypes Type
        {
            get
            {
				if(Scale < 2f) return DifficultyTypes.Easy;
				if(Scale < 3.25f) return DifficultyTypes.Normal;
				if(Scale < 4.5f) return DifficultyTypes.Hard;
				if(Scale < 5.75f) return DifficultyTypes.Insane;
                return DifficultyTypes.Extreme;
            }
        }
    }
}