using PBGame.UI.Models;
using PBGame.UI.Models.MenuBar;
using PBGame.UI.Components.Common;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.MenuBar
{
    public abstract class BaseMenuButton : FocusableTrigger {

        /// <summary>
        /// Returns the spritename of the icon.
        /// </summary>
        protected abstract string IconSpritename { get; }

        /// <summary>
        /// Returns the type of the menu this button represents.
        /// </summary>
        protected abstract MenuType Type { get; }

        /// <summary>
        /// Returns whether the deriving class with handle on enable init call.
        /// </summary>
        protected virtual bool OverrideEnableInitCall => false;

        [ReceivesDependency]
        protected MenuBarModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            CreateIconSprite(spriteName: IconSpritename, size: 28);

            UseDefaultHoverAni();
            UseDefaultFocusAni();

            if(!OverrideEnableInitCall)
                OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.FocusedMenu.OnNewValue += OnMenuFocusChange;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Model.FocusedMenu.OnNewValue -= OnMenuFocusChange;
        }

        protected override void OnClickTriggered()
        {
            base.OnClickTriggered();
            Model.SetMenu(Type);
        }

        /// <summary>
        /// Event called when the currently focused menu is changed.
        /// </summary>
        protected virtual void OnMenuFocusChange(MenuType type)
        {
            this.IsFocused = (this.Type == type);
        }
    }
}