using System;

namespace PBGame.UI.Components.QuickMenu
{
    public class BasicMenuButton : BaseMenuButton {

        /// <summary>
        /// Sets the action to invoke on button trigger.
        /// </summary>
        public void SetAction(Action action) => this.triggerAction = action;

        public override void OnShowQuickMenu() {}
    }
}