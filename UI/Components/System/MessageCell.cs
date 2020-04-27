using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.CoffeeUI;
using PBFramework.Allocation.Recyclers;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIExtensions;

namespace PBGame.UI.Components.System
{
    public class MessageCell : HoverableTrigger, IRecyclable<MessageCell> {

        /// <summary>
        /// The duration of message before it automatically hides.
        /// </summary>
        private const float ShowDuration = 4f;

        /// <summary>
        /// Event called when this cell is hidden.
        /// </summary>
        public event Action<MessageCell> OnHidden;

        private CanvasGroup canvasGroup;

        private ISprite bgSprite;
        private ISprite glowSprite;
        private ILabel label;

        private IAnime showAni;
        private float curDuration;


        public IRecycler<MessageCell> Recycler { get; set; }

        /// <summary>
        /// Returns whether the show/hide animations are playing.
        /// </summary>
        private bool IsAnimating => showAni.IsPlaying || triggerAni.IsPlaying;


        [InitWithDependency]
        private void Init()
        {
            OnTriggered += () =>
            {
                Hide();
            };

            canvasGroup = RawObject.AddComponent<CanvasGroup>();

            bgSprite = CreateChild<UguiSprite>("bg", 0);
            {
                bgSprite.Anchor = Anchors.Fill;
                bgSprite.Offset = new Offset(8f);
                bgSprite.SpriteName = "circle-16";
                bgSprite.ImageType = Image.Type.Sliced;

                var gradient = bgSprite.AddEffect(new GradientEffect()).Component;
                gradient.direction = UIGradient.Direction.Horizontal;
                gradient.color2 = new Color(0.75f, 0.75f, 0.75f);

                glowSprite = bgSprite.CreateChild<UguiSprite>("glow", 0);
                {
                    glowSprite.Anchor = Anchors.Fill;
                    glowSprite.Offset = new Offset(-13.5f);
                    glowSprite.SpriteName = "glow-circle-16-x2";
                    glowSprite.ImageType = Image.Type.Sliced;
                }
                label = bgSprite.CreateChild<Label>("label", 1);
                {
                    label.Anchor = Anchors.TopStretch;
                    label.Pivot = Pivots.Top;
                    label.Y = -12f;
                    label.SetOffsetHorizontal(12f);
                    label.WrapText = true;
                }
            }

            showAni = new Anime();
            showAni.AnimateFloat(x => this.X = x)
                .AddTime(0f, 200f, EaseType.QuadEaseOut)
                .AddTime(0.25f, 0f)
                .Build();
            showAni.AnimateFloat(a => canvasGroup.alpha = a)
                .AddTime(0f, () => canvasGroup.alpha)
                .AddTime(0.25f, 0.5f)
                .Build();

            triggerAni = new Anime();
            triggerAni.AnimateFloat(x => this.X = x)
                .AddTime(0f, 0f, EaseType.QuadEaseOut)
                .AddTime(0.25f, -200f)
                .Build();
            triggerAni.AnimateFloat(a => canvasGroup.alpha = a)
                .AddTime(0f, () => canvasGroup.alpha)
                .AddTime(0.25f, 0f)
                .Build();
            triggerAni.AddEvent(triggerAni.Duration, () => OnHidden?.Invoke(this));

            hoverInAni = new Anime();
            hoverInAni.AnimateFloat(a => canvasGroup.alpha = a)
                .AddTime(0f, () => canvasGroup.alpha)
                .AddTime(0.25f, 1f)
                .Build();

            hoverOutAni = new Anime();
            hoverOutAni.AnimateFloat(a => canvasGroup.alpha = a)
                .AddTime(0f, () => canvasGroup.alpha)
                .AddTime(0.25f, 0.5f)
                .Build();
        }

        /// <summary>
        /// Initializes the cell to display specified data.
        /// </summary>
        public void Show(string text, ColorPalette palette)
        {
            if(Active || IsAnimating)
                return;

            bgSprite.Color = palette.Weaken(0.3f);
            glowSprite.Color = palette.Base;

            label.Text = text;
            this.Height = label.PreferredHeight + label.Offset.Vertical * 2f + bgSprite.Offset.Vertical * 2f;

            triggerAni.Stop();
            showAni.PlayFromStart();

            curDuration = ShowDuration;
        }

        /// <summary>
        /// Hides the cell.
        /// </summary>
        public void Hide()
        {
            if (!Active || IsAnimating)
                return;

            showAni.Stop();
            triggerAni.PlayFromStart();
        }

        public void OnRecycleNew()
        {
            Active = true;
        }

        public void OnRecycleDestroy()
        {
            Active = false;
        }

        private void Update()
        {
            if(IsAnimating)
                return;

            // Handle auto hiding after certain time.
            if (curDuration > 0f)
            {
                curDuration -= Time.deltaTime;
                if (curDuration <= 0f)
                    Hide();
            }
        }
    }
}