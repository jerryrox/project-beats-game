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
using UnityEngine.UI;

namespace PBGame.UI.Components.Offsets
{
    public class MetronomeTick : UguiSprite, IHasTint {

        private ISprite glowSprite;
        private ISprite fillSprite;

        private IAnime tickAni;


        public new Color Tint
        {
            get => fillSprite.Color;
            set
            {
                glowSprite.Tint = value;
                fillSprite.Tint = value;
            }
        }

        [ReceivesDependency]
        private ISoundPool SoundPool { get; set; }


        [InitWithDependency]
        private void Init()
        {
            SpriteName = "circle-32";
            Color = Color.black;

            glowSprite = CreateChild<UguiSprite>("glow", 0);
            {
                glowSprite.Anchor = AnchorType.Fill;
                glowSprite.Offset = new Offset(-10f);
                glowSprite.SpriteName = "glow-circle-32";
            }
            fillSprite = CreateChild<UguiSprite>("fill", 1);
            {
                fillSprite.Anchor = AnchorType.Fill;
                fillSprite.Offset = Offset.Zero;
                fillSprite.SpriteName = "circle-32";
            }

            tickAni = new Anime();
            tickAni.AnimateFloat(a => fillSprite.Alpha = a)
                .AddTime(0f, 1f)
                .AddTime(0.35f, 0f, EaseType.CubicEaseOut)
                .Build();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (tickAni != null)
            {
                tickAni.Stop();
                fillSprite.Alpha = 0f;
            }
        }

        /// <summary>
        /// Animates tick effect.
        /// </summary>
        public void Tick()
        {
            tickAni.PlayFromStart();
            SoundPool.Play("heartbeat");
        }
    }
}