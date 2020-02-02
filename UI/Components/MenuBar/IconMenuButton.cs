using PBFramework.UI;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.MenuBar
{
    public abstract class IconMenuButton : BaseMenuButton {

        protected ISprite iconSprite;


        /// <summary>
        /// Returns the spritename of the icon.
        /// </summary>
        protected abstract string IconName { get; }


        [InitWithDependency]
        private void Init()
        {
            iconSprite = CreateChild<UguiSprite>("icon");
            {
                iconSprite.Size = new Vector2(36f, 36f);
                iconSprite.SpriteName = IconName;
            }
        }
    }
}