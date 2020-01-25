using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI.Navigations;
using PBFramework.Utils;
using PBFramework.Animations;
using UnityEngine;

namespace PBGame.Animations
{
    public class AnimePreset : IAnimePreset {

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
                .AddTime(0f, 0f, EaseType.CubicEaseOut)
                .AddTime(0.35f, 1f)
                .Build();
            return anime;
        }

        public IAnime GetDefaultOverlayShow(INavigationView overlay)
        {
            IAnime anime = new Anime();
            anime.AnimateFloat((alpha) => overlay.Alpha = alpha)
                .AddTime(0f, 0f, EaseType.QuadEaseOut)
                .AddTime(0.35f, 1f)
                .Build();
            return anime;
        }

        public IAnime GetDefaultOverlayHIde(INavigationView overlay)
        {
            IAnime anime = new Anime();
            anime.AnimateFloat((alpha) => overlay.Alpha = alpha)
                .AddTime(0f, 0f, EaseType.QuadEaseOut)
                .AddTime(0.35f, 1f)
                .Build();
            return anime;
        }
    }
}