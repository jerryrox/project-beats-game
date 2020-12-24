using PBGame.UI;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Shaders;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets.Beats.Standard.UI.Components
{
    public class LaneComboDisplay : UguiObject {

        private static readonly Color MinPersistColor = new Color(0.1f, 0.1f, 0.1f);
        private static readonly Color MaxPersistColor = new Color(0.4f, 0.4f, 0.4f);
        private static readonly Vector3 MaxPersistScale = new Vector3(1.2f, 1.2f, 1f);

        private static readonly Vector3 MinEffectScale = new Vector3(0.9f, 0.9f, 1f);
        private static readonly Vector3 MaxEffectScale = new Vector3(1.75f, 1.75f, 1f);
        private static readonly Color MaxEffectColor = new Color(0.5f, 0.5f, 0.5f);

        private ILabel effectComboLabel;
        private ILabel persistComboLabel;

        private IAnime comboAni;
        private IAnime hideAni;


        [InitWithDependency]
        private void Init(IGameSession gameSession)
        {
            gameSession.OnSoftInit += () =>
            {
                gameSession.ScoreProcessor.Combo.Bind(OnComboChanged);
            };
            gameSession.OnSoftDispose += () =>
            {
                comboAni.Stop();
                hideAni.Stop();
                effectComboLabel.Alpha = 0f;
                persistComboLabel.Alpha = 0f;
            };

            effectComboLabel = CreateChild<Label>("effect");
            {
                effectComboLabel.IsBold = true;
                effectComboLabel.FontSize = 48;
                effectComboLabel.Color = Color.black;

                effectComboLabel.AddEffect(new AdditiveShaderEffect());
            }
            persistComboLabel = CreateChild<Label>("persisting");
            {
                persistComboLabel.IsBold = true;
                persistComboLabel.FontSize = 48;
                persistComboLabel.Color = new Color(0.3f, 0.3f, 0.3f, 1f);

                persistComboLabel.AddEffect(new AdditiveShaderEffect());
            }

            comboAni = new Anime();
            comboAni.AnimateColor((color) => effectComboLabel.Color = color)
                .AddTime(0f, Color.black)
                .AddTime(0.05f, MaxEffectColor, EaseType.QuadEaseOut)
                .AddTime(0.25f, Color.black)
                .Build();
            comboAni.AnimateVector3((scale) => effectComboLabel.Scale = scale)
                .AddTime(0f, MinEffectScale, EaseType.CubicEaseOut)
                .AddTime(0.25f, MaxEffectScale)
                .Build();
            comboAni.AnimateColor((color) => persistComboLabel.Color = color)
                .AddTime(0f, () => persistComboLabel.Color)
                .AddTime(0.04f, MaxPersistColor, EaseType.QuadEaseIn)
                .AddTime(0.2f, MinPersistColor)
                .Build();
            comboAni.AnimateVector3((scale) => persistComboLabel.Scale = scale)
                .AddTime(0f, Vector3.one, EaseType.QuadEaseOut)
                .AddTime(0.1f, MaxPersistScale, EaseType.QuadEaseIn)
                .AddTime(0.2f, Vector3.one)
                .Build();

            hideAni = new Anime();
            hideAni.AnimateColor((color) => effectComboLabel.Color = color)
                .AddTime(0f, () => effectComboLabel.Color)
                .AddTime(0.25f, Color.black)
                .Build();
            hideAni.AnimateColor((color) => persistComboLabel.Color = color)
                .AddTime(0f, () => persistComboLabel.Color)
                .AddTime(0.25f, Color.black)
                .Build();
        }

        /// <summary>
        /// Shows the combo change animation with the specified combo value.
        /// </summary>
        public void Show(int combo)
        {
            if(hideAni.IsPlaying)
                hideAni.Stop();

            string comboString = combo.ToString("N0");;
            effectComboLabel.Text = comboString;
            persistComboLabel.Text = comboString;
            comboAni.PlayFromStart();
        }

        /// <summary>
        /// Hides the combo label immediately.
        /// </summary>
        public void Hide()
        {
            if(comboAni.IsPlaying)
                comboAni.Stop();
            hideAni.PlayFromStart();
        }

        /// <summary>
        /// Event called when the current combo has changed.
        /// </summary>
        private void OnComboChanged(int combo, int prevCombo)
        {
            if(combo > prevCombo)
                Show(combo);
            else
                Hide();
        }
    }
}