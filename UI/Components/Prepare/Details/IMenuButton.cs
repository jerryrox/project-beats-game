namespace PBGame.UI.Components.Prepare.Details
{
    public interface IMenuButton {
    
        /// <summary>
        /// Sprite name of the icon.
        /// </summary>
        string IconName { get; set; }

        /// <summary>
        /// Text displayed on the label.
        /// </summary>
        string LabelText { get; set; }
    }
}