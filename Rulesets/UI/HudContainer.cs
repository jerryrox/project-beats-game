using PBGame.Rulesets.UI.HUD;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.UI
{
    public abstract class HudContainer : UguiObject
    {
        /// <summary>
        /// Returns the accuracy displayer object.
        /// </summary>
        public AccuracyDisplay AccuracyDisplay { get; protected set; }

        /// <summary>
        /// Returns the score displayer object.
        /// </summary>
        public ScoreDisplay ScoreDisplay { get; protected set; }

        /// <summary>
        /// Returns the combo displayer object.
        /// </summary>
        public ComboDisplay ComboDisplay { get; protected set; }

        /// <summary>
        /// Returns the health displayer object.
        /// </summary>
        public HealthDisplay HealthDisplay { get; protected set; }

        /// <summary>
        /// Returns the effects displayer object.
        /// </summary>
        public TouchEffectDisplay TouchEffectDisplay { get; protected set; }

        /// <summary>
        /// Returns the skip displayer object.
        /// </summary>
        public SkipDisplay SkipDisplay { get; protected set; }


        [InitWithDependency]
        private void Init()
        {
            SkipDisplay = CreateChild<SkipDisplay>("skip", 100);
            {
                SkipDisplay.Anchor = AnchorType.BottomStretch;
                SkipDisplay.Pivot = PivotType.Bottom;
                SkipDisplay.SetOffsetHorizontal(0f);
                SkipDisplay.Y = 0f;
                SkipDisplay.Height = 200f;
            }
        }
    }
}