using PBGame.Rulesets.Scoring;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Shaders;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets.UI.Components
{
    public class HealthDisplay : UguiObject {

        private ISprite fgSprite;
        private ISprite thumbSprite;
        private ISprite pinEffectSprite;

        private bool isFailing;
        private IScoreProcessor scoreProcessor;

        private float curHealth;
        private Color barTintFromColor;
        private Color barTintToColor;

        private Color barIncColor;
        private Color barDecColor;

        private IAnime changeAni;
        private IAnime pinAni;


        /// <summary>
        /// The health progress bar.
        /// </summary>
        public IProgressBar ProgressBar { get; private set; }

        /// <summary>
        /// The sprite which indicates the required amount of health to clear the map.
        /// </summary>
        public ISprite Indicator { get; private set; }

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init(IGameSession gameSession)
        {
            gameSession.OnSoftInit += () =>
            {
                scoreProcessor = gameSession.ScoreProcessor;
                scoreProcessor.Health.BindAndTrigger(OnHealthChange);
                SetFailingForce(false);
            };
            gameSession.OnSoftDispose += () =>
            {
                ProgressBar.Value = 0f;
            };

            ProgressBar = CreateChild<UguiProgressBar>("progress", 0);
            {
                fgSprite = ProgressBar.Foreground;
                fgSprite.Color = ColorPreset.PrimaryFocus.Base;

                thumbSprite = ProgressBar.Thumb;
                thumbSprite.Active = true;
                thumbSprite.SpriteName = "glow-128";
                thumbSprite.Color = ColorPreset.PrimaryFocus.Alpha(0f);
                thumbSprite.Size = new Vector2(12f, 56f);

                pinEffectSprite = thumbSprite.CreateChild<UguiSprite>("effect");
                {
                    pinEffectSprite.Color = ColorPreset.PrimaryFocus.Alpha(0f);
                    pinEffectSprite.SpriteName = "glow-128";
                    pinEffectSprite.Size = new Vector2(48f, 224f);

                    pinEffectSprite.AddEffect(new AdditiveShaderEffect());
                }
            }
            Indicator = CreateChild<UguiSprite>("indicator", 1);
            {
                Indicator.Color = ColorPreset.PrimaryFocus.Base;
            }

            changeAni = new Anime();
            changeAni.AnimateFloat((progress) => ProgressBar.Value = progress)
                .AddTime(0f, () => ProgressBar.Value)
                .AddTime(0.2f, () => curHealth)
                .Build();
            changeAni.AnimateColor((tint) => fgSprite.Tint = tint)
                .AddTime(0f, () => barTintFromColor)
                .AddTime(0.35f, () => barTintToColor)
                .Build();

            pinAni = new Anime();
            // Thumb sprite fade and scale
            pinAni.AnimateFloat((alpha) => thumbSprite.Alpha = alpha)
                .AddTime(0f, 0f)
                .AddTime(0.05f, 1f, EaseType.QuadEaseIn)
                .AddTime(0.25f, 0f)
                .Build();
            pinAni.AnimateVector3((scale) => thumbSprite.Scale = scale)
                .AddTime(0f, new Vector3(1.1f, 1.1f), EaseType.QuadEaseIn)
                .AddTime(0.25f, Vector3.one)
                .Build();
            // Pin effect fade and scale
            pinAni.AnimateFloat((alpha) => pinEffectSprite.Alpha = alpha)
                .AddTime(0f, 1f, EaseType.QuadEaseOut)
                .AddTime(0.25f, 0f)
                .Build();
            pinAni.AnimateVector3((scale) => pinEffectSprite.Scale = scale)
                .AddTime(0f, Vector3.zero, EaseType.CubicEaseOut)
                .AddTime(0.25f, Vector3.one)
                .Build();
        }

        /// <summary>
        /// Visually changes the progress bar to indicate whether the player is failing or not.
        /// </summary>
        private void SetFailing(bool isFailing)
        {
            if(this.isFailing == isFailing)
                return;
            SetFailingForce(isFailing);
        }

        /// <summary>
        /// Forcefully manupulates states without any checks, unlike the non-foced version.
        /// </summary>
        private void SetFailingForce(bool isFailing)
        {
            this.isFailing = isFailing;

            Color tint = isFailing ? ColorPreset.Negative.Base : ColorPreset.PrimaryFocus.Base;
            thumbSprite.Tint = tint;
            pinEffectSprite.Tint = tint;

            barIncColor = tint * 1.5f;
            barDecColor = tint * 0.5f;
            barTintToColor = tint;
        }

        /// <summary>
        /// Event called on health change.
        /// </summary>
        private void OnHealthChange(float health, float prevHealth)
        {
            // Change overall color theme of the bar based on health state.
            SetFailing(scoreProcessor.IsFailed);

            if (health < prevHealth)
            {
                barTintFromColor = barDecColor;
            }
            else
            {
                // Show pin ani only on incremental change.
                pinAni.PlayFromStart();

                barTintFromColor = barIncColor;
            }

            // Animate health change
            curHealth = health;
            changeAni.PlayFromStart();
        }
    }
}