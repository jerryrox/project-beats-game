using PBFramework.Dependencies;

namespace PBGame.Rulesets.Beats.Standard.UI
{
    public class GameplayLayer : Rulesets.UI.GameplayLayer {

        [InitWithDependency]
        private void Init()
        {
        }

        protected override Rulesets.UI.IPlayAreaContainer CreatePlayArea()
            => CreateChild<PlayAreaContainer>("playarea-container");

        protected override Rulesets.UI.IHudContainer CreateHud()
            => CreateChild<HudContainer>("hud-container");
    }
}