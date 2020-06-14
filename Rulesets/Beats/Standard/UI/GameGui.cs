using PBFramework.Dependencies;

namespace PBGame.Rulesets.Beats.Standard.UI
{
    public class GameGui : Rulesets.UI.GameGui {

        protected override Rulesets.UI.GameplayLayer CreateGameplayLayer()
            => CreateChild<GameplayLayer>("gameplay-layer");
    }
}