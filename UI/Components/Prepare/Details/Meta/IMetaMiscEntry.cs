using PBFramework.Graphics;

namespace PBGame.UI.Components.Prepare.Details.Meta
{
    public interface IMetaMiscEntry : IGraphicObject {
    
        /// <summary>
        /// Text displayed on the section text.
        /// </summary>
        string LabelText { get; set; }

        /// <summary>
        /// Text displayed as content.
        /// </summary>
        string Content { get; set; }
    }
}