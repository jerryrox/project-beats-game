using PBGame.UI.Models.QuickMenu;
using PBGame.UI.Components.Common;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.QuickMenu
{
    public class MenuButton : HighlightableTrigger, IHasLabel
    {
        private ILabel label;

        private MenuInfo menuInfo;


        public string LabelText
        {
            get => label.Text;
            set => label.Text = value;
        }


        [InitWithDependency]
        private void Init()
        {
            IsClickToTrigger = true;
            OnTriggered += () =>
            {
                if(!IsFocused)
                    menuInfo?.Action?.Invoke();
            };

            var icon = CreateIconSprite(depth: 4, size: 32f);
            {
                icon.Y = 8f;
            }

            label = CreateChild<Label>("label", 5);
            {
                label.Anchor = AnchorType.MiddleStretch;
                label.SetOffsetHorizontal(0f);
                label.FontSize = 16;
                label.IsBold = true;
                label.Y = -20f;
            }

            UseDefaultFocusAni();
            UseDefaultHighlightAni();
            UseDefaultHoverAni();

            OnEnableInited();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            // Remove hovered state.
            hoverOutAni.Seek(hoverOutAni.Duration);
        }

        /// <summary>
        /// Sets the menu info to associate with this button.
        /// </summary>
        public void SetMenuInfo(MenuInfo menuInfo)
        {
            this.menuInfo = menuInfo;
            if (menuInfo != null)
            {
                LabelText = menuInfo.Label;
                IconName = menuInfo.Icon;
                IsFocused = menuInfo.ShouldHighlight;
            }
        }

        /// <summary>
        /// Triggers menu info's action.
        /// </summary>
        private void TriggerMenuAction()
        {
            if(!IsFocused)
                menuInfo?.Action?.Invoke();
        }
    }
}