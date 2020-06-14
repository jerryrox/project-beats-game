using System;
using PBGame.Animations;
using PBFramework.Utils;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Initialize
{
    public class LogoDisplay : Components.LogoDisplay {

        private IAnime startupAnime;
        private IAnime breatheAnime;
        private IAnime endAnime;


        /// <summary>
        /// Event called when the startup animation has finished.
        /// </summary>
        public Action OnStartup { get; set; }

        /// <summary>
        /// Event called when the end animation has finished.
        /// </summary>
        public Action OnEnd { get; set; }

        [ReceivesDependency]
        private IAnimePreset AnimePreset { get; set; }


        [InitWithDependency]
        private void Init()
        {
            startupAnime = new Anime();
            startupAnime.AnimateColor((color) => this.Glow.Color = color)
                .AddTime(0f, new Color(1f, 1f, 1f, 0f), EaseType.QuadEaseOut)
                .AddTime(0.1f, new Color(1f, 1f, 1f, 1f), EaseType.QuadEaseIn)
                .AddTime(1f, new Color(0f, 0f, 0f, 1f))
                .Build();
            startupAnime.AnimateColor((color) => this.Outer.Color = color)
                .AddTime(0f, new Color(1f, 1f, 1f, 0f), EaseType.QuadEaseOut)
                .AddTime(0.1f, new Color(1f, 1f, 1f, 1f), EaseType.QuadEaseIn)
                .AddTime(1f, new Color(0.25f, 0.25f, 0.25f, 1f))
                .Build();
            startupAnime.AnimateColor((color) => this.Inner.Color = color)
                .AddTime(0.1f, new Color(1f, 1f, 1f, 0f), EaseType.QuadEaseIn)
                .AddTime(1f, new Color(0.25f, 0.25f, 0.25f, 1f))
                .Build();
            startupAnime.AnimateColor((color) => this.Title.Color = color)
                .AddTime(0.5f, new Color(), EaseType.QuartEaseIn)
                .AddTime(1f, new Color(0.75f, 0.75f, 0.75f, 1f))
                .Build();
            startupAnime.AddEvent(startupAnime.Duration, () => OnStartup?.Invoke());

            breatheAnime = new Anime();
            breatheAnime.WrapMode = WrapModeType.Loop;
            breatheAnime.AnimateColor((color) => this.Glow.Color = color)
                .AddTime(0f, Color.black, EaseType.SineEaseOut)
                .AddTime(1.1f, Color.gray, EaseType.SineEaseIn)
                .AddTime(2.2f, Color.black)
                .Build();
            breatheAnime.AnimateColor((color) => this.Title.Color = color)
                .AddTime(0f, new Color(0.75f, 0.75f, 0.75f, 1f), EaseType.SineEaseOut)
                .AddTime(1.1f, Color.white, EaseType.SineEaseIn)
                .AddTime(2.2f, new Color(0.75f, 0.75f, 0.75f, 1f))
                .Build();

            endAnime = new Anime();
            endAnime.AnimateColor((color) => this.Glow.Color = color)
                .AddTime(0f, () => this.Glow.Color, EaseType.QuartEaseIn)
                .AddTime(1.5f, Color.white)
                .Build();
            endAnime.AnimateColor((color) => this.Outer.Color = color)
                .AddTime(0f, () => this.Outer.Color, EaseType.QuartEaseIn)
                .AddTime(1.5f, Color.white)
                .Build();
            endAnime.AnimateColor((color) => this.Inner.Color = color)
                .AddTime(0f, () => this.Inner.Color, EaseType.QuartEaseIn)
                .AddTime(1.5f, Color.white)
                .Build();
            endAnime.AnimateColor((color) => this.Title.Color = color)
                .AddTime(0f, () => this.Title.Color, EaseType.QuartEaseIn)
                .AddTime(1.5f, Color.white)
                .Build();
            endAnime.AnimateVector3((scale) => this.Scale = scale)
                .AddTime(0f, Vector3.one, EaseType.SineEaseOut)
                .AddTime(1.5f, new Vector3(1.1f, 1.1f, 1.1f))
                .Build();
            endAnime.AddEvent(endAnime.Duration, () => OnEnd?.Invoke());
        }

        /// <summary>
        /// Plays the initial logo animation.
        /// </summary>
        public void PlayStartup()
        {
            breatheAnime.Stop();
            endAnime.Stop();
            startupAnime.PlayFromStart();
        }

        /// <summary>
        /// Plays the looping animation on the logo.
        /// </summary>
        public void PlayBreathe()
        {
            startupAnime.Stop();
            endAnime.Stop();
            breatheAnime.PlayFromStart();
        }

        /// <summary>
        /// Plays the logo hide animation.
        /// </summary>
        public void PlayEnd()
        {
            startupAnime.Stop();
            breatheAnime.Stop();
            endAnime.PlayFromStart();
        }
    }
}