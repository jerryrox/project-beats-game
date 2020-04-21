using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Audio;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.MusicMenu
{
    public class TimeBar : UguiSlider {

        private bool isControlling;

        private IAnime downAni;
        private IAnime upAni;

        [ReceivesDependency]
        private IMusicController MusicController { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            OnPointerDown += () =>
            {
                isControlling = true;

                upAni.Stop();
                downAni.PlayFromStart();
            };
            OnPointerUp += () =>
            {
                isControlling = false;

                downAni.Stop();
                upAni.PlayFromStart();
            };
            OnChange += (value) =>
            {
                var audio = MusicController.Audio;
                if (isControlling && audio != null)
                    MusicController.Seek(value * audio.Duration);
            };

            background.Color = Color.black;
            foreground.Color = colorPreset.SecondaryFocus;
            thumb.Active = false;

            downAni = new Anime();
            downAni.AnimateFloat(y => ScaleY = y)
                .AddTime(0f, () => ScaleY)
                .AddTime(0.25f, 2f)
                .Build();

            upAni = new Anime();
            upAni.AnimateFloat(y => ScaleY = y)
                .AddTime(0f, () => ScaleY)
                .AddTime(0.25f, 1f)
                .Build();
        }

        private void Update()
        {
            if (isControlling) return;

            var audio = MusicController.Audio;
            if (audio == null)
                Value = 0f;
            else
                Value = MusicController.CurrentTime / audio.Duration;
        }
    }
}