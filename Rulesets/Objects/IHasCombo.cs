namespace PBGame.Rulesets.Objects
{
	/// <summary>
	/// Interface indicating that the implementing hit object has a combo.
	/// </summary>
	public interface IHasCombo {

		/// <summary>
		/// Returns whether this object is a new combo.
		/// </summary>
		bool IsNewCombo { get; }

		/// <summary>
		/// Returns the number of combo colors to skip.
		/// </summary>
		int ComboOffset { get; }
	}
}

