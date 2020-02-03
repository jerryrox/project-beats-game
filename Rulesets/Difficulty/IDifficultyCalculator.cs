namespace PBGame.Rulesets.Difficulty
{
    public interface IDifficultyCalculator {
    
		/// <summary>
		/// Calculates the difficulty for current map.
		/// </summary>
        DifficultyInfo Calculate();
    }
}