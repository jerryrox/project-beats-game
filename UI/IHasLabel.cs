namespace PBGame.UI
{
    /// <summary>
    /// Indicates that a text label representing the object exists.
    /// </summary>
    public interface IHasLabel {
    
        /// <summary>
        /// The text value displayed on the label.
        /// </summary>
        string LabelText { get; set; }
    }
}