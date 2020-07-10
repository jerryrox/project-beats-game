using PBGame.UI.Models;
using PBGame.Rulesets.Maps;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.GameLoad
{
    public class InfoDisplayer : UguiObject, IGameLoadComponent {

        private ILabel titleLabel;
        private ILabel artistLabel;
        private ILabel versionLabel;
        private ILabel mapperLabel;
        private ThumbDisplayer thumbDisplayer;

        private IAnime showAni;
        private IAnime hideAni;


        public float ShowAniDuration => showAni.Duration;

        public float HideAniDuration => hideAni.Duration;

        [ReceivesDependency]
        private GameLoadModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            titleLabel = CreateChild<Label>("title", 0);
            {
                titleLabel.FontSize = 22;
                titleLabel.IsBold = true;
                titleLabel.Alpha = 0;
            }
            artistLabel = CreateChild<Label>("artist", 1);
            {
                artistLabel.FontSize = 16;
                artistLabel.Alpha = 0;
            }
            versionLabel = CreateChild<Label>("version", 2);
            {
                versionLabel.FontSize = 20;
                versionLabel.Alpha = 0;
            }
            mapperLabel = CreateChild<Label>("mapper", 3);
            {
                mapperLabel.FontSize = 16;
                mapperLabel.Alpha = 0;
            }
            thumbDisplayer = CreateChild<ThumbDisplayer>("thumb", 4);
            {
                thumbDisplayer.Size = new Vector2(400f, 72f);
                thumbDisplayer.Alpha = 0;
            }

            showAni = new Anime();
            AddShowAniFrame(titleLabel, 0.3f, new Vector2(0f, 112f));
            AddShowAniFrame(artistLabel, 0.4f, new Vector2(0f, 88f));
            AddShowAniFrame(thumbDisplayer, 0.5f, new Vector2(0f, 16f));
            AddShowAniFrame(versionLabel, 0.6f, new Vector2(0f, -60f));
            AddShowAniFrame(mapperLabel, 0.7f, new Vector2(0f, -86f));

            hideAni = new Anime();
            hideAni.AddEvent(0f, () => showAni.Stop());
            AddHideAniFrame(titleLabel, 0f);
            AddHideAniFrame(artistLabel, 0.1f);
            AddHideAniFrame(thumbDisplayer, 0.2f);
            AddHideAniFrame(versionLabel, 0.3f);
            AddHideAniFrame(mapperLabel, 0.4f);

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.PreferUnicode.OnNewValue += OnPreferUnicodeChange;
            Model.SelectedMap.OnNewValue += OnMapChange;

            SetupDisplays();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Model.PreferUnicode.OnNewValue -= OnPreferUnicodeChange;
            Model.SelectedMap.OnNewValue -= OnMapChange;

            showAni.Stop();
            hideAni.Stop();
        }

        public void Show() => showAni.PlayFromStart();

        public void Hide() => hideAni.PlayFromStart();

        /// <summary>
        /// Initializes displays to reflect currently selected map.
        /// </summary>
        private void SetupDisplays()
        {
            var map = Model.SelectedMap.Value;
            if (map == null)
            {
                titleLabel.Text = "";
                artistLabel.Text = "";
                versionLabel.Text = "";
                mapperLabel.Text = "";
            }
            else
            {
                var preferUnicode = Model.PreferUnicode.Value;
                titleLabel.Text = map.Metadata.GetTitle(preferUnicode);
                artistLabel.Text = map.Metadata.GetArtist(preferUnicode);
                versionLabel.Text = map.Detail.Version;
                mapperLabel.Text = $"mapped by {map.Metadata.Creator}";
            }
        }

        /// <summary>
        /// Adds common show animation effect.
        /// </summary>
        private void AddShowAniFrame<T>(T component, float targetTime, Vector2 targetPos)
            where T : IHasTransform, IHasAlpha
        {
            showAni.AnimateFloat(a => component.Alpha = a)
                .AddTime(targetTime, 0f, EaseType.QuadEaseOut)
                .AddTime(targetTime + 0.25f, 1f)
                .Build();
            showAni.AnimateVector2(pos => component.Position = pos)
                .AddTime(0f, targetPos)
                .AddTime(targetTime, targetPos.GetTranslated(0f, -20f), EaseType.QuadEaseOut)
                .AddTime(targetTime + 0.25f, targetPos)
                .Build();
        }

        /// <summary>
        /// Adds common hide animation effect.
        /// </summary>
        private void AddHideAniFrame<T>(T component, float targetTime)
            where T : IHasTransform, IHasAlpha
        {
            hideAni.AnimateFloat(a => component.Alpha = a)
                .AddTime(targetTime, 1f, EaseType.QuadEaseOut)
                .AddTime(targetTime + 0.25f, 0f)
                .Build();
            hideAni.AnimateVector2(scale => component.Scale = scale)
                .AddTime(targetTime, Vector2.one, EaseType.QuadEaseOut)
                .AddTime(targetTime + 0.25f, new Vector2(1.25f, 1.25f))
                .Build();
        }

        /// <summary>
        /// Event called on prefer unicode preference change.
        /// </summary>
        private void OnPreferUnicodeChange(bool preferUnicode) => SetupDisplays();

        /// <summary>
        /// Event called on selected map change.
        /// </summary>
        private void OnMapChange(IPlayableMap map) => SetupDisplays();
    }
}