using PBGame.Graphics;
using PBGame.Rulesets.UI.HUD;
using PBGame.Rulesets.Beats.Standard.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Shaders;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.Beats.Standard.UI
{
    public class HudContainer : Rulesets.UI.HudContainer {


        [InitWithDependency]
        private void Init(IRoot3D root3D)
        {
            float labelColor = 0.4f;

            AccuracyDisplay = CreateChild<AccuracyDisplay>("accuracy", 0);
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
            ScoreDisplay = CreateChild<ScoreDisplay>("score", 1);
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
            ComboDisplay = CreateChild<ComboDisplay>("combo", 2);
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
            HealthDisplay = CreateChild<HealthDisplay>("health", 3);
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
                }
                var indicator = HealthDisplay.Indicator;
                {
                    indicator.Anchor = AnchorType.CenterStretch;
                    indicator.X = 0f;
                    indicator.Width = 2f;
                    indicator.SetOffsetVertical(-8f);
                }
            }
        }
    }
}