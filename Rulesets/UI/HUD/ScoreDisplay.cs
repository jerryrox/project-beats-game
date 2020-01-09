using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets.UI.HUD
{
    public class ScoreDisplay : UguiObject, IScoreDisplay {

        public ILabel ScoreLabel { get; private set; }


        [InitWithDependency]
        private void Init()
        {
            ScoreLabel = CreateChild<UguiLabel>("label");
            {
                ScoreLabel.Anchor = Anchors.Fill;
                ScoreLabel.RawSize = Vector2.zero;
            }
        }
    }
}