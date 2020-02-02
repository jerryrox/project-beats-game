using PBGame.UI;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets.UI.HUD
{
    public class AccuracyDisplay : UguiObject, IAccuracyDisplay {

        public ILabel AccuracyLabel { get; private set; }


        [InitWithDependency]
        private void Init()
        {
            AccuracyLabel = CreateChild<Label>("label");
            {
                AccuracyLabel.Anchor = Anchors.Fill;
                AccuracyLabel.RawSize = Vector2.zero;
            }
        }
    }
}