using PBGame.UI;
using PBGame.Assets.Fonts;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets.UI.HUD
{
    public class ScoreDisplay : UguiObject, IScoreDisplay {

        public ILabel ScoreLabel { get; private set; }


        [InitWithDependency]
        private void Init(IFontManager fontManager)
        {
            ScoreLabel = CreateChild<Label>("label");
            {
                ScoreLabel.Anchor = Anchors.Fill;
                ScoreLabel.RawSize = Vector2.zero;
                ScoreLabel.Font = fontManager.DefaultFont;
            }
        }
    }
}