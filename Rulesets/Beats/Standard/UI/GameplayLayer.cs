
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.Beats.Standard.UI
{
    public class GameplayLayer : Rulesets.UI.GameplayLayer {

        [InitWithDependency]
        private void Init()
        {
            Dependencies.Cache(this);
        }

        protected override Rulesets.UI.PlayAreaContainer CreatePlayArea()
        {
            var playArea = CreateChild<PlayAreaContainer>("playarea-container");
            playArea.Anchor = AnchorType.Bottom;
            playArea.Pivot = PivotType.Bottom;
            playArea.Size = new Vector2(1400f, 5000f);
            playArea.RotationX = 55f;
            playArea.Y = 0f;
            return playArea;
        }

        protected override Rulesets.UI.HudContainer CreateHud()
        {
            var hudContainer = CreateChild<HudContainer>("hud-container");
            hudContainer.Anchor = AnchorType.Fill;
            hudContainer.Offset = Offset.Zero;
            return hudContainer;
        }
    }
}