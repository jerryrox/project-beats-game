using PBFramework.Dependencies;

namespace PBGame.UI.Components.Common
{
    /// <summary>
    /// HoverableTrigger with just an icon.
    /// </summary>
    public class IconButton : HoverableTrigger {
    
        [InitWithDependency]
        private void Init()
        {
            CreateIconSprite(spriteName: "null", size: 24);
            UseDefaultHoverAni();

            IsClickToTrigger = true;
        }
    }
}