using PBFramework.UI.Navigations;
using PBFramework.Utils;
using PBFramework.Animations;
using UnityEngine;

namespace PBGame.Animations
{
    public class AnimePreset : IAnimePreset
    {

        public IAnime GetDefaultScreenShow(INavigationView screen)
        {
            IAnime anime = new Anime();
            anime.AnimateVector3((scale) => screen.Scale = scale)
                .AddTime(0f, new Vector3(3f, 3f, 1f), EaseType.QuartEaseOut)
                .AddTime(0.35f, Vector3.one)
                .Build();
            anime.AnimateFloat((alpha) => screen.Alpha = alpha)
                .AddTime(0f, 0f, EaseType.CubicEaseOut)
                .AddTime(0.35f, 1f)
                .Build();
            return anime;
        }

        public IAnime GetDefaultScreenHide(INavigationView screen)
        {
            IAnime anime = new Anime();
            anime.AnimateVector3((scale) => screen.Scale = scale)
                .AddTime(0f, Vector3.one, EaseType.QuartEaseOut)
                .AddTime(0.35f, new Vector3(3f, 3f, 1f))
                .Build();
            anime.AnimateFloat((alpha) => screen.Alpha = alpha)
                .AddTime(0f, 1f, EaseType.CubicEaseOut)
                .AddTime(0.35f, 0f)
                .Build();
            return anime;
        }

        public IAnime GetDefaultOverlayShow(INavigationView overlay)
        {
            IAnime anime = new Anime();
            anime.AnimateFloat((alpha) => overlay.Alpha = alpha)
                .AddTime(0f, 0f, EaseType.Linear)
                .AddTime(0.35f, 1f)
                .Build();
            return anime;
        }

        public IAnime GetDefaultOverlayHide(INavigationView overlay)
        {
            IAnime anime = new Anime();
            anime.AnimateFloat((alpha) => overlay.Alpha = alpha)
                .AddTime(0f, 1f, EaseType.QuadEaseOut)
                .AddTime(0.35f, 0f)
                .Build();
            return anime;
        }

        public IAnime GetInitLogoStartup(UI.Components.Initialize.ILogoDisplay logoDisplay)
        {
            var anime = new Anime();
            anime.AnimateColor((color) => logoDisplay.Glow.Color = color)
                .AddTime(0f, new Color(1f, 1f, 1f, 0f), EaseType.QuadEaseOut)
                .AddTime(0.1f, new Color(1f, 1f, 1f, 1f), EaseType.QuadEaseIn)
                .AddTime(1f, new Color(0f, 0f, 0f, 1f))
                .Build();
            anime.AnimateColor((color) => logoDisplay.Outer.Color = color)
                .AddTime(0f, new Color(1f, 1f, 1f, 0f), EaseType.QuadEaseOut)
                .AddTime(0.1f, new Color(1f, 1f, 1f, 1f), EaseType.QuadEaseIn)
                .AddTime(1f, new Color(0.25f, 0.25f, 0.25f, 1f))
                .Build();
            anime.AnimateColor((color) => logoDisplay.Inner.Color = color)
                .AddTime(0.1f, new Color(1f, 1f, 1f, 0f), EaseType.QuadEaseIn)
                .AddTime(1f, new Color(0.25f, 0.25f, 0.25f, 1f))
                .Build();
            anime.AnimateColor((color) => logoDisplay.Title.Color = color)
                .AddTime(0.5f, new Color(), EaseType.QuartEaseIn)
                .AddTime(1f, new Color(0.75f, 0.75f, 0.75f, 1f))
                .Build();
            return anime;
        }

        public IAnime GetInitLogoBreathe(UI.Components.Initialize.ILogoDisplay logoDisplay)
        {
            var anime = new Anime();
            anime.WrapMode = WrapModes.Loop;
            anime.AnimateColor((color) => logoDisplay.Glow.Color = color)
                .AddTime(0f, Color.black, EaseType.SineEaseOut)
                .AddTime(1.1f, Color.gray, EaseType.SineEaseIn)
                .AddTime(2.2f, Color.black)
                .Build();
            anime.AnimateColor((color) => logoDisplay.Title.Color = color)
                .AddTime(0f, new Color(0.75f, 0.75f, 0.75f, 1f), EaseType.SineEaseOut)
                .AddTime(1.1f, Color.white, EaseType.SineEaseIn)
                .AddTime(2.2f, new Color(0.75f, 0.75f, 0.75f, 1f))
                .Build();
            return anime;
        }

        public IAnime GetInitLogoEnd(UI.Components.Initialize.ILogoDisplay logoDisplay)
        {
            var anime = new Anime();
            anime.AnimateColor((color) => logoDisplay.Glow.Color = color)
                .AddTime(0f, () => logoDisplay.Glow.Color, EaseType.QuartEaseIn)
                .AddTime(1.5f, Color.white)
                .Build();
            anime.AnimateColor((color) => logoDisplay.Outer.Color = color)
                .AddTime(0f, () => logoDisplay.Outer.Color, EaseType.QuartEaseIn)
                .AddTime(1.5f, Color.white)
                .Build();
            anime.AnimateColor((color) => logoDisplay.Inner.Color = color)
                .AddTime(0f, () => logoDisplay.Inner.Color, EaseType.QuartEaseIn)
                .AddTime(1.5f, Color.white)
                .Build();
            anime.AnimateColor((color) => logoDisplay.Title.Color = color)
                .AddTime(0f, () => logoDisplay.Title.Color, EaseType.QuartEaseIn)
                .AddTime(1.5f, Color.white)
                .Build();
            anime.AnimateVector3((scale) => logoDisplay.Scale = scale)
                .AddTime(0f, Vector3.one, EaseType.SineEaseOut)
                .AddTime(1.5f, new Vector3(1.1f, 1.1f, 1.1f))
                .Build();
            return anime;
        }

        public IAnime GetHomeLogoPulse(UI.Components.Home.ILogoDisplay logoDisplay)
        {
            // Animation is created with an assumption of 60 bpm.
            var anime = new Anime();
            anime.AnimateVector3((scale) => logoDisplay.Scale = scale)
                .AddTime(0f, new Vector3(1.1f, 1.1f, 1f), EaseType.SineEaseOut)
                .AddTime(1.5f, Vector3.one)
                .Build();
            return anime;
        }

        public IAnime GetHomeLogoHover(UI.Components.Home.ILogoDisplay logoDisplay)
        {
            var anime = new Anime();
            anime.AnimateFloat((alpha) => logoDisplay.Alpha = alpha)
                .AddTime(0f, () => logoDisplay.Alpha)
                .AddTime(0.5f, 0.5f)
                .Build();
            return anime;
        }

        public IAnime GetHomeLogoExit(UI.Components.Home.ILogoDisplay logoDisplay)
        {
            var anime = new Anime();
            anime.AnimateFloat((alpha) => logoDisplay.Alpha = alpha)
                .AddTime(0f, () => logoDisplay.Alpha)
                .AddTime(0.5f, 1f)
                .Build();
            return anime;
        }

        public IAnime GetHomeLogoZoomIn(UI.Components.Home.ILogoDisplay logoDisplay)
        {
            var anime = new Anime();
            anime.AnimateVector2((size) => logoDisplay.Size = size)
                .AddTime(0f, () => logoDisplay.Size, EaseType.QuartEaseIn)
                .AddTime(0.35f, new Vector2(700f, 700f))
                .Build();
            return anime;
        }

        public IAnime GetHomeLogoZoomOut(UI.Components.Home.ILogoDisplay logoDisplay)
        {
            var anime = new Anime();
            anime.AnimateVector2((size) => logoDisplay.Size = size)
                .AddTime(0f, () => logoDisplay.Size, EaseType.QuartEaseIn)
                .AddTime(0.5f, new Vector2(352f, 352f))
                .Build();
            return anime;
        }
    }
}