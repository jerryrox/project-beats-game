using PBGame.UI.Models.MenuBar;
using PBGame.Rulesets;

namespace PBGame.UI.Components.MenuBar
{
    public class ModeMenuButton : BaseMenuButton
    {
        protected override MenuType Type => MenuType.Mode;

        protected override string IconSpritename => null;

        
        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.GameMode.BindAndTrigger(OnGameModeChange);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            Model.GameMode.Unbind(OnGameModeChange);
        }

        /// <summary>
        /// Event called when the current game mode changes.
        /// </summary>
        private void OnGameModeChange(GameModeType type)
        {
            IconName = Model.GetModeService()?.GetIconName(64);
        }
    }
}