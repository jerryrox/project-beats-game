using PBGame.Rulesets.Maps;
using PBFramework.UI;

namespace PBGame.UI.Components.Prepare
{
    public interface IVersionButton : IButtonTrigger, IListItem {
    
        /// <summary>
        /// Whether the button can be interacted.
        /// </summary>
        bool IsInteractible { get; set; }


        /// <summary>
        /// Changes the visual state based on the specified map.
        /// </summary>
        void Setup(IPlayableMap map);
    }
}