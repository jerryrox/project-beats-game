using System;
using PBGame.Graphics;
using PBGame.Notifications;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.CoffeeUI;
using PBFramework.Graphics.Effects.Components;
using PBFramework.Animations;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIExtensions;

namespace PBGame.UI.Components.Common
{
    public class NotificationCell : HoverableTrigger, IRecyclable<NotificationCell>, IHasAlpha
    {
        /// <summary>
        /// The X position from which the cell will slide in on show, or slide to on hide.
        /// </summary>
        private const float SlideInPos = 200f;

        /// <summary>
        /// The duration of message before it automatically hides.
        /// </summary>
        private const float AutoHideDelay = 4f;

        /// <summary>
        /// Event called when this cell is hidden.
        /// </summary>
        public event Action<NotificationCell> OnDismiss;

        private CanvasGroup canvasGroup;

        private IGraphicObject container;
        private ISprite bgSprite;
        private WebTexture coverTexture;
        private ISprite glowSprite;
        private ILabel label;

        private IAnime showAni;
        private IAnime hideAni;
        private IAnime positionAni;
        private float targetY;

        private float curHideDelay;


        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }

        public IRecycler<NotificationCell> Recycler { get; set; }

        /// <summary>
        /// Current displayal scope of the notification.
        /// </summary>
        public NotificationScope CurScope { get; private set; }

        /// <summary>
        /// The notification info bound to this cell.
        /// </summary>
        public INotification Notification { get; private set; }

        /// <summary>
        /// Returns the desired Y position of the cell.
        /// </summary>
        public float TargetY => targetY;

        /// <summary>
        /// Returns the actual height of the cell.
        /// </summary>
        public virtual float CellHeight => this.Height;

        /// <summary>
        /// Returns whether the show/hide animations are playing.
        /// </summary>
        private bool IsAnimating => showAni.IsPlaying || hideAni.IsPlaying;

        /// <summary>
        /// Returns whether auto hiding should be handled.
        /// </summary>
        private bool ShouldAutoHide => CurScope == NotificationScope.Temporary || Notification?.Scope == NotificationScope.Temporary;

        [ReceivesDependency]
        protected IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init()
        {
            OnTriggered += OnCellTrigger;

            canvasGroup = RawObject.AddComponent<CanvasGroup>();

            container = CreateChild("container");
            {
                container.Anchor = AnchorType.Fill;
                container.Offset = new Offset(8f);

                bgSprite = container.CreateChild<UguiSprite>("bg");
                {
                    bgSprite.Anchor = AnchorType.Fill;
                    bgSprite.Offset = Offset.Zero;
                    bgSprite.SpriteName = "circle-16";
                    bgSprite.ImageType = Image.Type.Sliced;

                    bgSprite.AddEffect(new MaskEffect());

                    var gradient = bgSprite.AddEffect(new GradientEffect()).Component;
                    gradient.direction = UIGradient.Direction.Horizontal;
                    gradient.color2 = new Color(0.75f, 0.75f, 0.75f);
                }
                glowSprite = bgSprite.CreateChild<UguiSprite>("glow");
                {
                    glowSprite.Anchor = AnchorType.Fill;
                    glowSprite.Offset = new Offset(-13.5f);
                    glowSprite.SpriteName = "glow-circle-16-x2";
                    glowSprite.ImageType = Image.Type.Sliced;
                }
                label = bgSprite.CreateChild<Label>("label");
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

            hideAni = new Anime();
            hideAni.AnimateFloat(x => this.X = x)
                .AddTime(0f, 0f, EaseType.QuadEaseOut)
                .AddTime(0.25f, SlideInPos)
                .Build();
            hideAni.AnimateFloat(a => this.Alpha = a)
                .AddTime(0f, () => this.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
            hideAni.AddEvent(hideAni.Duration, () => OnDismiss?.Invoke(this));

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
        /// Shows this cell with the specified notification.
        /// </summary>
        public virtual void Show(INotification notification, NotificationScope scope)
        {
            if (!Active || IsAnimating)
                return;

            CurScope = scope;
            curHideDelay = ShouldAutoHide ? AutoHideDelay : float.PositiveInfinity;

            this.Notification = notification;

            var palette = GetColor(notification.Type);
            bgSprite.Color = palette.Weaken(0.3f);
            glowSprite.Color = palette.Base;

            label.Text = notification.Message;
            label.Height = label.PreferredHeight;
            this.Height = label.Height + (-label.Y * 2f) + container.Offset.Vertical;

            hideAni.Stop();
            showAni.PlayFromStart();
        }

        /// <summary>
        /// Hides the cell.
        /// </summary>
        public void Hide()
        {
            if (!Active || IsAnimating)
                return;

            showAni.Stop();
            hideAni.PlayFromStart();

            // If the current displayal scope is Stored, revoke the task.
            if (CurScope == NotificationScope.Stored)
                Notification?.Task?.RevokeTask(true);
        }

        /// <summary>
        /// Makes the cell position its Y value to specified value.
        /// </summary>
        public void PositionTo(float y, bool animate)
        {
            if (!Active || hideAni.IsPlaying)
                return;

            targetY = y;
            if (animate)
                positionAni.PlayFromStart();
            else
                this.Y = y;
        }

        public virtual void OnRecycleNew()
        {
            Active = true;
            Alpha = 0f;
            X = SlideInPos;
        }

        public virtual void OnRecycleDestroy()
        {
            Active = false;
            Notification = null;
            showAni.Stop();
            hideAni.Stop();
            positionAni.Stop();
        }

        protected override void Update()
        {
            if(IsAnimating)
                return;

            base.Update();

            if (curHideDelay > 0f)
            {
                curHideDelay -= Time.deltaTime;
                if (curHideDelay <= 0f)
                    Hide();
            }
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

        /// <summary>
        /// Event called on notification cell button trigger.
        /// </summary>
        private void OnCellTrigger()
        {
            if(Notification == null)
                return;

            if (CurScope == NotificationScope.Temporary)
                Hide();
            else
            {
                // If has actions, display context menu.
                if (Notification.HasActions())
                {
                    // TODO:
                }
                else
                    Hide();
            }
        }
    }
}