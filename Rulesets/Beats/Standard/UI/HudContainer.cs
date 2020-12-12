using PBGame.Graphics;
using PBGame.Rulesets.UI.Components;
using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBGame.Rulesets.Beats.Standard.Maps;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Shaders;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets.Beats.Standard.UI
{
    public class HudContainer : Rulesets.UI.HudContainer {

        [InitWithDependency]
        private void Init(IRoot3D root3D)
        {
            float labelColor = 0.4f;

            AccuracyDisplay = CreateChild<AccuracyDisplay>("accuracy");
            {
                AccuracyDisplay.Anchor = AnchorType.Bottom;
                AccuracyDisplay.Position = new Vector3(-600f, 64f);

                var label = AccuracyDisplay.Label;
                {
                    label.Alignment = TextAnchor.MiddleLeft;
                    label.FontSize = 32;
                    label.IsBold = true;
                    label.Color = new Color(labelColor, labelColor, labelColor);
                    label.AddEffect(new AdditiveShaderEffect());
                }
            }
            ScoreDisplay = CreateChild<ScoreDisplay>("score");
            {
                ScoreDisplay.Anchor = AnchorType.Bottom;
                ScoreDisplay.Position = new Vector3(0, 64f);

                var label = ScoreDisplay.Label;
                {
                    label.Alignment = TextAnchor.MiddleCenter;
                    label.FontSize = 40;
                    label.IsBold = true;
                    label.Color = new Color(labelColor, labelColor, labelColor);
                    label.AddEffect(new AdditiveShaderEffect());
                }
            }
            ComboDisplay = CreateChild<ComboDisplay>("combo");
            {
                ComboDisplay.Anchor = AnchorType.Bottom;
                ComboDisplay.Position = new Vector3(600f, 64f);

                var label = ComboDisplay.Label;
                {
                    label.Alignment = TextAnchor.MiddleRight;
                    label.FontSize = 32;
                    label.IsBold = true;
                    label.Color = new Color(labelColor, labelColor, labelColor);
                    label.AddEffect(new AdditiveShaderEffect());
                }
            }
            HealthDisplay = CreateChild<HealthDisplay>("health");
            {
                HealthDisplay.Anchor = AnchorType.Bottom;
                HealthDisplay.Size = new Vector2(
                    Mathf.Min(root3D.Resolution.x, PixelDefinition.PlayAreaWidth),
                    16
                );

                var progressBar = HealthDisplay.ProgressBar;
                {
                    progressBar.Anchor = AnchorType.Fill;
                    progressBar.Offset = Offset.Zero;

                    var bg = progressBar.Background;
                    {
                        bg.Color = new Color(0f, 0f, 0f, 0.75f);
                    }
                    var fg = progressBar.Foreground;
                    {
                        fg.Offset = new Offset(0f, 0f, 0f, 12f);
                    }
                    var thumb = progressBar.Thumb;
                    {
                        thumb.Parent.Offset = new Offset(0f, -6f, 0f, 6f);
                    }
                }
                var indicator = HealthDisplay.Indicator;
                {
                    indicator.Anchor = AnchorType.CenterStretch;
                    indicator.X = 0f;
                    indicator.Width = 2f;
                    indicator.SetOffsetVertical(-8f);
                }
            }
            var laneComboDisplay = CreateChild<LaneComboDisplay>("lane-combo");
            {
                laneComboDisplay.Anchor = AnchorType.Top;
                laneComboDisplay.Position = new Vector3(0f, -150f);
                laneComboDisplay.RotationX = 25f;
            }
            TouchEffectDisplay = CreateChild<TouchEffectDisplay>("touch-effect");
            {
                TouchEffectDisplay.Anchor = AnchorType.Fill;
                TouchEffectDisplay.Offset = Offset.Zero;
            }
        }
    }
}