using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBGame.Notifications;
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
    public class MessageCell : HoverableTrigger, IRecyclable<MessageCell>, IHasAlpha {

        /// <summary>
        /// The duration of message before it automatically hides.
        /// </summary>
        private const float ShowDuration = 4f;

        /// <summary>
        /// The X position from which the cell will slide in on show, or slide to on hide.
        /// </summary>
        private const float SlideInPos = 200f;

        /// <summary>
        /// Event called when this cell is hidden.
        /// </summary>
        public event Action<MessageCell> OnHidden;

        private CanvasGroup canvasGroup;

        private ISprite bgSprite;
        private ISprite glowSprite;
        private ILabel label;

        private IAnime showAni;
        private IAnime positionAni;
        private float curDuration;
        private float targetY;


        public IRecycler<MessageCell> Recycler { get; set; }

        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }

        /// <summary>
        /// The notification info bound to this cell.
        /// </summary>
        public INotification Notification { get; private set; }

        /// <summary>
        /// Returns the desired Y position of the cell.
        /// </summary>
        public float TargetY => targetY;

        /// <summary>
        /// Returns whether the show/hide animations are playing.
        /// </summary>
        private bool IsAnimating => showAni.IsPlaying || triggerAni.IsPlaying;

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


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
                bgSprite.Anchor = AnchorType.Fill;
                bgSprite.Offset = new Offset(8f);
                bgSprite.SpriteName = "circle-16";
                bgSprite.ImageType = Image.Type.Sliced;

                var gradient = bgSprite.AddEffect(new GradientEffect()).Component;
                gradient.direction = UIGradient.Direction.Horizontal;
                gradient.color2 = new Color(0.75f, 0.75f, 0.75f);

                glowSprite = bgSprite.CreateChild<UguiSprite>("glow", 0);
                {
                    glowSprite.Anchor = AnchorType.Fill;
                    glowSprite.Offset = new Offset(-13.5f);
                    glowSprite.SpriteName = "glow-circle-16-x2";
                    glowSprite.ImageType = Image.Type.Sliced;
                }
                label = bgSprite.CreateChild<Label>("label", 1);
                {
                    label.Anchor = AnchorType.TopStretch;
                    label.Pivot = PivotType.Top;
                    label.Y = -12f;
                    label.FontSize = 16;
                    label.Alignment = TextAnchor.MiddleCenter;
                    label.SetOffsetHorizontal(12f);
                    label.WrapText = true;
                }
            }

            showAni = new Anime();
            showAni.AnimateFloat(x => this.X = x)
                .AddTime(0f, SlideInPos, EaseType.QuadEaseOut)
                .AddTime(0.25f, 0f)
                .Build();
            showAni.AnimateFloat(a => this.Alpha = a)
                .AddTime(0f, () => this.Alpha)
                .AddTime(0.25f, 0.75f)
                .Build();

            triggerAni = new Anime();
            triggerAni.AnimateFloat(x => this.X = x)
                .AddTime(0f, 0f, EaseType.QuadEaseOut)
                .AddTime(0.25f, SlideInPos)
                .Build();
            triggerAni.AnimateFloat(a => this.Alpha = a)
                .AddTime(0f, () => this.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
            triggerAni.AddEvent(triggerAni.Duration, () => OnHidden?.Invoke(this));

            hoverInAni = new Anime();
            hoverInAni.AnimateFloat(a => this.Alpha = a)
                .AddTime(0f, () => this.Alpha)
                .AddTime(0.25f, 1f)
                .Build();

            hoverOutAni = new Anime();
            hoverOutAni.AnimateFloat(a => this.Alpha = a)
                .AddTime(0f, () => this.Alpha)
                .AddTime(0.25f, 0.75f)
                .Build();

            positionAni = new Anime();
            positionAni.AnimateFloat(y => this.Y = y)
                .AddTime(0f, () => this.Y)
                .AddTime(0.25f, () => targetY)
                .Build();
        }

        /// <summary>
        /// Initializes the cell to display specified data.
        /// </summary>
        public void Show(INotification notification)
        {
            if(!Active || IsAnimating)
                return;

            this.Notification = notification;

            var palette = GetColor(notification.Type);
            bgSprite.Color = palette.Weaken(0.3f);
            glowSprite.Color = palette.Base;

            label.Text = notification.Message;
            label.Height = label.PreferredHeight;
            this.Height = label.Height + (-label.Y * 2f) + bgSprite.Offset.Vertical;

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

        /// <summary>
        /// Makes the cell position its Y value to specified value.
        /// </summary>
        public void PositionTo(float y, bool animate)
        {
            if(!Active || triggerAni.IsPlaying)
                return;

            targetY = y;
            if(animate)
                positionAni.PlayFromStart();
            else
                this.Y = y;
        }

        public void OnRecycleNew()
        {
            Active = true;
            Alpha = 0f;
            X = SlideInPos;
        }

        public void OnRecycleDestroy()
        {
            Active = false;
            Notification = null;
            showAni.Stop();
            positionAni.Stop();
        }

        /// <summary>
        /// Returns the color palette matching the specified type.
        /// </summary>
        private ColorPalette GetColor(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Passive: return ColorPreset.Passive;
                case NotificationType.Positive: return ColorPreset.Positive;
                case NotificationType.Negative: return ColorPreset.Negative;
                case NotificationType.Warning: return ColorPreset.Warning;
            }
            Debug.LogWarning($"MessageCell.GetColor - Unknown notification type: " + type);
            return ColorPreset.Passive;
        }

        protected override void Update()
        {
            if(IsAnimating)
                return;

            base.Update();

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