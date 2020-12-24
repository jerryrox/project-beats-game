using PBGame.Graphics;
using PBGame.Rulesets.Judgements;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Animations;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.UI.Components
{
    public class DamagedDisplay : UguiSprite
    {
        private IAnime showAni;


        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init(IGameSession session)
        {
            session.OnSoftInit += () =>
            {
                session.ScoreProcessor.OnNewJudgement += OnNewJudgement;
            };

            SpriteName = "glow-in-square-32";
            Color = ColorPreset.GetHitResultColor(HitResultType.Miss).Alpha(0f);

            showAni = new Anime();
            showAni.AnimateFloat((alpha) => Alpha = alpha)
                .AddTime(0f, () => Alpha)
                .AddTime(0.05f, 0.125f, EaseType.QuadEaseOut)
                .AddTime(0.35f, 0f)
                .Build();
        }

        /// <summary>
        /// Shows the damaged effect.
        /// </summary>
        public void ShowEffect()
        {
            showAni.PlayFromStart();
        }

        /// <summary>
        /// Event called when a new judgement has been made.
        /// </summary>
        private void OnNewJudgement(JudgementResult result)
        {
            if (!result.IsHit)
                ShowEffect();
        }
    }
}