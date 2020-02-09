using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Maps;
using PBGame.Audio;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Songs
{
    public class SortButton : UguiTrigger, ISortButton {

        private const float HighlightWidth = 120f;

        private ISprite hoverSprite;
        private ISprite highlightSprite;
        private ILabel label;

        private IAnime hoverAni;
        private IAnime outAni;
        private IAnime focusAni;
        private IAnime unfocusAni;


        public MapsetSorts SortType { get; set; }

        public string LabelText
        {
            get => label.Text;
            set => label.Text = value;
        }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset, ISoundPooler soundPooler)
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

            hoverSprite = CreateChild<UguiSprite>("hover", 0);
            {
                hoverSprite.Anchor = Anchors.Fill;
                hoverSprite.RawSize = Vector2.zero;
                hoverSprite.Alpha = 0f;
            }
            highlightSprite = CreateChild<UguiSprite>("highlight", 1);
            {
                highlightSprite.Anchor = Anchors.Bottom;
                highlightSprite.Width = HighlightWidth;
                highlightSprite.Height = 20f;
                highlightSprite.Y = 0f;
                highlightSprite.Color = colorPreset.SecondaryFocus;
                highlightSprite.SpriteName = "glow-bar";
                highlightSprite.ImageType = Image.Type.Sliced;
            }
            label = CreateChild<UguiLabel>("label", 2);
            {
                label.Anchor = Anchors.Fill;
                label.RawSize = Vector2.zero;
                label.Alignment = TextAnchor.MiddleCenter;
                label.WrapText = false;
                label.FontSize = 18;
            }

            hoverAni = new Anime();
            hoverAni.AnimateFloat((alpha) => hoverSprite.Alpha = alpha)
                .AddTime(0f, () => hoverSprite.Alpha)
                .AddTime(0.25f, 0.25f)
                .Build();

            outAni = new Anime();
            outAni.AnimateFloat((alpha) => hoverSprite.Alpha = alpha)
                .AddTime(0f, () => hoverSprite.Alpha)
                .AddTime(0.25f, 0f)
                .Build();

            focusAni = new Anime();
            focusAni.AnimateFloat((alpha) => highlightSprite.Alpha = alpha)
                .AddTime(0f, () => highlightSprite.Alpha)
                .AddTime(0.3f, 1f)
                .Build();
            focusAni.AnimateFloat((width) => highlightSprite.Width = width)
                .AddTime(0f, () => highlightSprite.Width)
                .AddTime(0.3f, HighlightWidth)
                .Build();

            unfocusAni = new Anime();
            unfocusAni.AnimateFloat((alpha) => highlightSprite.Alpha = alpha)
                .AddTime(0f, () => highlightSprite.Alpha)
                .AddTime(0.3f, 0f)
                .Build();
            unfocusAni.AnimateFloat((width) => highlightSprite.Width = width)
                .AddTime(0f, () => highlightSprite.Width)
                .AddTime(0.3f, 0f)
                .Build();
        }

        public void SetToggle(bool isToggled)
        {
            if (isToggled)
            {
                outAni.Stop();
                hoverAni.PlayFromStart();
            }
            else
            {
                hoverAni.Stop();
                outAni.PlayFromStart();
            }
        }
    }
}