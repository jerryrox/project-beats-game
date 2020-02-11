using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Audio;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.MusicMenu
{
    public class ControlButton : UguiTrigger, IControlButton {

        private ISprite icon;

        private IAnime hoverAni;
        private IAnime outAni;


        public string IconName
        {
            get => icon.SpriteName;
            set => icon.SpriteName = value;
        }

        public float IconSize
        {
            get => icon.Width;
            set => icon.Size = new Vector2(value, value);
        }


        [InitWithDependency]
        private void Init(ISoundPooler soundPooler)
        {
            OnPointerEnter += () =>
            {
                soundPooler.Play("menuhit");

                outAni.Stop();
                hoverAni.PlayFromStart();
            };
            OnPointerExit += () =>
            {
                hoverAni.Stop();
                outAni.PlayFromStart();
            };
            OnPointerDown += () =>
            {
                soundPooler.Play("menuclick");
            };

            icon = CreateChild<UguiSprite>("icon", 0);
            {
                icon.Alpha = 0.5f;;
            }

            hoverAni = new Anime();
            hoverAni.AnimateFloat(alpha => icon.Alpha = alpha)
                .AddTime(0f, () => icon.Alpha)
                .AddTime(0.25f, 1f)
                .Build();

            outAni = new Anime();
            outAni.AnimateFloat(alpha => icon.Alpha = alpha)
                .AddTime(0f, () => icon.Alpha)
                .AddTime(0.25f, 0.5f)
                .Build();
        }
    }
}