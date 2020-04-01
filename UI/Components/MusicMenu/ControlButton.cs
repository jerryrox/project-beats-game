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
    public class ControlButton : ButtonTrigger, IControlButton {

        private ISprite icon;


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

        protected override bool IsClickToTrigger => false;


        [InitWithDependency]
        private void Init(ISoundPooler soundPooler)
        {
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