using PBFramework.Dependencies;

namespace PBGame.Rulesets.Beats.Standard.UI
{
    public class GameGui : Rulesets.UI.GameGui {

        [InitWithDependency]
        private void Init()
        {
        }

        protected override Rulesets.UI.IGameplayLayer CreateGameplayLayer()
            => CreateChild<GameplayLayer>("gameplay-layer");
    }
}