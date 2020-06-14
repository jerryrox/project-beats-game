using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Common
{
    public class BasicToggle : FocusableTrigger, IHasTint {

        private static readonly Vector2 DefaultIconHolderSize = new Vector2(36f, 16f);

        protected IGraphicObject iconHolder;
        protected ISprite backgroundSprite;
        protected ISprite outlineSprite;

        private Color tint;


        /// <summary>
        /// Anchor of the toggle icon.
        /// </summary>
        public AnchorType IconAnchor
        {
            get => iconHolder.Anchor;
            set => SetIconAnchor(value);
        }

        /// <summary>
        /// The transform scale of the icon.
        /// </summary>
        public float IconScale
        {
            get => iconHolder.ScaleX;
            set => iconHolder.Scale = new Vector3(value, value, 1f);
        }

        /// <summary>
        /// Whether the toggle should be focused automatically on trigger.
        /// </summary>
        public bool AutoFocus { get; set; } = true;

        public virtual Color Tint
        {
            get => tint;
            set
            {
                outlineSprite.Tint = value;
                focusSprite.Tint = value;
                hoverSprite.Tint = value;
            }
        }


        protected override int FocusSpriteDepth => 2;

        protected override int HoverSpriteDepth => 3;


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            OnTriggered += () =>
            {
                if(AutoFocus)
                    IsFocused = !IsFocused;
            };

            tint = colorPreset.PrimaryFocus;

            var touchArea = CreateChild<UguiSprite>("touch-area", -100);
            {
                touchArea.Anchor = AnchorType.Fill;
                touchArea.Offset = Offset.Zero;
                touchArea.Alpha = 0f;
            }
            iconHolder = CreateChild("icon-holder", 0);
            {
                iconHolder.Size = DefaultIconHolderSize;

                backgroundSprite = iconHolder.CreateChild<UguiSprite>("background", 0);
                {
                    backgroundSprite.Anchor = AnchorType.Fill;
                    backgroundSprite.Offset = Offset.Zero;
                    backgroundSprite.SpriteName = "circle-16";
                    backgroundSprite.ImageType = Image.Type.Sliced;
                    backgroundSprite.Color = Color.black;
                }

                outlineSprite = iconHolder.CreateChild<UguiSprite>("outline", 1);
                {
                    outlineSprite.Anchor = AnchorType.Fill;
                    outlineSprite.Offset = Offset.Zero;
                    outlineSprite.SpriteName = "outline-circle-16";
                    outlineSprite.ImageType = Image.Type.Sliced;
                    outlineSprite.Color = tint;
                }

                focusSprite.SetParent(iconHolder);
                focusSprite.Offset = Offset.Zero;
                focusSprite.SpriteName = "circle-16";
                focusSprite.ImageType = Image.Type.Sliced;
                focusSprite.Color = tint;
                focusSprite.Alpha = 0f;

                hoverSprite.SetParent(iconHolder);
                hoverSprite.Offset = new Offset(-13.5f);
                hoverSprite.SpriteName = "glow-circle-16-x2";
                hoverSprite.ImageType = Image.Type.Sliced;
                hoverSprite.Color = tint;
                hoverSprite.Alpha = 0f;
            }

            hoverInAni = new Anime();
            hoverInAni.AnimateFloat(a => hoverSprite.Alpha = a)
                .AddTime(0f, () => hoverSprite.Alpha)
                .AddTime(0.25f, 1f)
                .Build();

            hoverOutAni = new Anime();
            hoverOutAni.AnimateFloat(a => hoverSprite.Alpha = a)
                .AddTime(0f, () => hoverSprite.Alpha)
                .AddTime(0.25f, 0f)
                .Build();

            focusAni = new Anime();
            focusAni.AnimateFloat(w => focusSprite.Width = w)
                .AddTime(0f, () => focusSprite.Width)
                .AddTime(0.25f, () => backgroundSprite.Width)
                .Build();
            focusAni.AnimateFloat(a => focusSprite.Alpha = a)
                .AddTime(0f, () => focusSprite.Alpha)
                .AddTime(0.25f, 1f)
                .Build();

            unfocusAni = new Anime();
            unfocusAni.AnimateFloat(w => focusSprite.Width = w)
                .AddTime(0f, () => focusSprite.Width)
                .AddTime(0.25f, 0f)
                .Build();
            unfocusAni.AnimateFloat(a => focusSprite.Alpha = a)
                .AddTime(0f, () => focusSprite.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
        }

        /// <summary>
        /// Internally handles icon anchor assignment.
        /// </summary>
        protected virtual void SetIconAnchor(AnchorType anchor)
        {
            iconHolder.Anchor = anchor;

            float halfWidth = iconHolder.Width * 0.5f;
            float halfHeight = iconHolder.Height * 0.5f;

            switch (anchor)
            {
                case AnchorType.Bottom:
                    iconHolder.Position = new Vector2(0f, halfHeight);
                    break;
                case AnchorType.BottomLeft:
                    iconHolder.Position = new Vector2(halfWidth, halfHeight);
                    break;
                case AnchorType.BottomRight:
                    iconHolder.Position = new Vector2(-halfWidth, halfHeight);
                    break;
                case AnchorType.Center:
                    iconHolder.Position = Vector2.zero;
                    break;
                case AnchorType.Left:
                    iconHolder.Position = new Vector2(halfWidth, 0f);
                    break;
                case AnchorType.Right:
                    iconHolder.Position = new Vector2(-halfWidth, 0f);
                    break;
                case AnchorType.Top:
                    iconHolder.Position = new Vector2(0f, -halfHeight);
                    break;
                case AnchorType.TopLeft:
                    iconHolder.Position = new Vector2(halfWidth, -halfHeight);
                    break;
                case AnchorType.TopRight:
                    iconHolder.Position = new Vector2(-halfWidth, -halfHeight);
                    break;
                default:
                    throw new ArgumentException("Unsupported anchor type: " + anchor);
            }
        }
    }
}