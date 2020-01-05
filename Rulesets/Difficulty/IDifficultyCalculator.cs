namespace PBGame.Rulesets.Difficulty
{
    public interface IDifficultyCalculator {
    
		/// <summary>
		/// Returns whether the difficulty can be calculated on this object.
		/// </summary>
        bool IsValid { get; }


		/// <summary>
		/// Calculates the difficulty for current map.
		/// </summary>
        DifficultyInfo Calculate();
    }
}