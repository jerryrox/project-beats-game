using System;
using PBFramework.UI.Navigations;
using PBFramework.Utils;
using PBFramework.Graphics;
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

        public IAnime GetSubMenuOverlayShow(INavigationView overlay)
        {
            var anime = new Anime();
            anime.AnimateFloat(alpha => overlay.Alpha = alpha)
                .AddTime(0f, 0f, EaseType.CubicEaseOut)
                .AddTime(0.25f, 1f)
                .Build();
            return anime;
        }

        public IAnime GetSubMenuOverlayHide(INavigationView overlay)
        {
            var anime = new Anime();
            anime.AnimateFloat(alpha => overlay.Alpha = alpha)
                .AddTime(0f, 1f, EaseType.CubicEaseOut)
                .AddTime(0.25f, 0f)
                .Build();
            return anime;
        }

        public IAnime GetSubMenuOverlayPopupShow(INavigationView overlay, Func<IGraphicObject> getContainer)
        {
            IGraphicObject container = null;
            var anime = GetSubMenuOverlayShow(overlay);
            anime.AnimateFloat(y =>
            {
                if(container == null)
                    container = getContainer();
                container.Y = y;
            })
                .AddTime(0f, -36f, EaseType.CubicEaseOut)
                .AddTime(anime.Duration, -16f)
                .Build();
            return anime;
        }

        public IAnime GetSubMenuOverlayPopupHide(INavigationView overlay, Func<IGraphicObject> getContainer)
        {
            IGraphicObject container = null;
            var anime = GetSubMenuOverlayHide(overlay);
            anime.AnimateFloat(y =>
            {
                if(container == null)
                    container = getContainer();
                container.Y = y;
            })
                .AddTime(0f, -16f, EaseType.CubicEaseOut)
                .AddTime(anime.Duration, -36f)
                .Build();
            return anime;
        }
    }
}