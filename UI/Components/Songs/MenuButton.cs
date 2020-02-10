using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Audio;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Songs
{
    public class MenuButton : UguiTrigger, IMenuButton {

        private ISprite iconSprite;
        private ISprite pulseSprite;
        private ISprite hoverSprite;

        private IAnime hoverAni;
        private IAnime outAni;
        private IAnime pulseAni;


        public string IconName
        {
            get => iconSprite.SpriteName;
            set => iconSprite.SpriteName = pulseSprite.SpriteName = value;
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

            iconSprite = CreateChild<UguiSprite>("icon", 0);
            {
                iconSprite.Width = iconSprite.Height = 36f;
            }
            pulseSprite = CreateChild<UguiSprite>("pulse", 1);
            {
                pulseSprite.Width = pulseSprite.Height = 36f;
                pulseSprite.Alpha = 0f;
            }
            hoverSprite = CreateChild<UguiSprite>("hover", 2);
            {
                hoverSprite.Anchor = Anchors.Fill;
                hoverSprite.RawSize = Vector2.zero;
                hoverSprite.Alpha = 0f;
            }

            hoverAni = new Anime();
            hoverAni.AnimateFloat(alpha => hoverSprite.Alpha = alpha)
                .AddTime(0f, () => hoverSprite.Alpha)
                .AddTime(0.25f, 0.25f)
                .Build();

            outAni = new Anime();
            outAni.AnimateFloat(alpha => hoverSprite.Alpha = alpha)
                .AddTime(0f, () => hoverSprite.Alpha)
                .AddTime(0.25f, 0f)
                .Build();

            pulseAni = new Anime();
            pulseAni.AnimateFloat(alpha => pulseSprite.Alpha = alpha)
                .AddTime(0f, 1f, EaseType.QuadEaseOut)
                .AddTime(0.5f, 0f)
                .Build();
            pulseAni.AnimateVector3(scale => pulseSprite.Scale = scale)
                .AddTime(0f, Vector3.one, EaseType.CubicEaseOut)
                .AddTime(0.5f, new Vector3(4f, 4f, 4f))
                .Build();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            hoverAni.Stop();
            outAni.Stop();
            pulseAni.Stop();

            hoverSprite.Alpha = 0f;
            pulseSprite.Alpha = 0f;
            pulseSprite.Scale = Vector3.one;
        }
    }
}