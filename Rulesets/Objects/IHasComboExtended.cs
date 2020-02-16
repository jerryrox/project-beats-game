namespace PBGame.Rulesets.Objects
{
    /// <summary>
    /// Indicates that the hit object has a combo information with additional
    /// properties representing the relationship between other objects and combo.
    /// </summary>
    public interface IHasComboExtended : IHasCombo {
    
        /// <summary>
        /// Returns the index within a combo.
        /// </summary>
        int IndexInCombo { get; }

        /// <summary>
        /// Returns the index of the combo within the entire map.
        /// </summary>
        int ComboIndex { get; }

        /// <summary>
        /// Returns whether this is the last index within this combo.
        /// </summary>
        bool IsLastInCombo { get; }
    }
}