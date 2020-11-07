using System;
using System.Linq;
using System.Collections.Generic;
using PBGame.UI.Components.Common.Dropdown;
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

using Logger = PBFramework.Debugging.Logger;

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
        private ISprite actionSprite;
        private IProgressBar taskProgressBar;

        private IAnime showAni;
        private IAnime hideAni;
        private IAnime positionAni;
        private float targetY;

        private DropdownContext dropdownContext;

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
        private IColorPreset ColorPreset { get; set; }

        [ReceivesDependency]
        private IDropdownProvider DropdownProvider { get; set; }
        

        [InitWithDependency]
        private void Init()
        {
            OnTriggered += OnCellTrigger;

            canvasGroup = RawObject.AddComponent<CanvasGroup>();
            dropdownContext = new DropdownContext();
            dropdownContext.OnSelection += OnDropdownSelection;

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

                    coverTexture = bgSprite.CreateChild<WebTexture>("cover");
                    {
                        coverTexture.Anchor = AnchorType.Fill;
                        coverTexture.Offset = Offset.Zero;
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
                    actionSprite = bgSprite.CreateChild<UguiSprite>("actions");
                    {
                        actionSprite.Anchor = AnchorType.Right;
                        actionSprite.X = -8;
                        actionSprite.Size = new Vector2(24f, 24f);
                        actionSprite.SpriteName = "icon-actions";
                    }
                    taskProgressBar = bgSprite.CreateChild<UguiProgressBar>("progress");
                    {
                        taskProgressBar.Anchor = AnchorType.BottomStretch;
                        taskProgressBar.Pivot = PivotType.Bottom;
                        taskProgressBar.Y = 0f;
                        taskProgressBar.SetOffsetHorizontal(0f);
                        taskProgressBar.Height = 4;

                        taskProgressBar.Background.Color = new Color(0f, 0f, 0f, 0.5f);
                        taskProgressBar.Foreground.Color = ColorPreset.PrimaryFocus;
                        taskProgressBar.Foreground.SetOffsetTop(1f);
                    }
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
        public void Show(INotification notification, NotificationScope scope)
        {
            if (!Active || IsAnimating)
                return;

            CurScope = scope;
            curHideDelay = ShouldAutoHide ? AutoHideDelay : float.PositiveInfinity;

            this.Notification = notification;

            // Display progress bar if there is an associated task.
            BindListener();

            // Load cover image.
            coverTexture.Load(notification.CoverImage);

            // Display action sprites if there are any actions associated.
            actionSprite.Active = notification.HasActions();

            // Set color theme of cell.
            var palette = GetColor(notification.Type);
            bgSprite.Color = palette.Weaken(0.3f);
            glowSprite.Color = palette.Base;

            // Adjust height based on label height.
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

            dropdownContext.Datas.Clear();

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
            UnbindListener();
            coverTexture.Unload();

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
        /// Binds listener events to cell UI.
        /// </summary>
        private void BindListener()
        {
            var listener = Notification?.Listener;
            if (listener == null)
            {
                taskProgressBar.Active = false;
                return;
            }

            taskProgressBar.Active = true;
            taskProgressBar.Value = listener.Progress;
            listener.OnProgress += OnListenerProgress;
        }

        /// <summary>
        /// Unbinds listener events from cell UI.
        /// </summary>
        private void UnbindListener()
        {
            var listener = Notification?.Listener;
            if (listener != null)
                listener.OnProgress -= OnListenerProgress;
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
            Logger.LogWarning($"Unknown notification type: {type}");
            return ColorPreset.Passive;
        }

        /// <summary>
        /// Event called on selection of a dropdown menu item.
        /// </summary>
        private void OnDropdownSelection(DropdownData data)
        {
            var action = data.ExtraData as NotificationAction;
            if(action == null)
                return;

            action.Action?.Invoke();
            if(action.ShouldDismiss)
                Hide();
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
                    dropdownContext.Datas.Clear();
                    dropdownContext.Datas.AddRange(Notification.Actions.Select((a) => new DropdownData(a.Name, a)));

                    DropdownProvider.OpenAt(dropdownContext, GetPositionAtCorner(PivotType.Center, Space.World));
                }
                else
                    Hide();
            }
        }

        /// <summary>
        /// Event called when the task listener reports a new progress.
        /// </summary>
        private void OnListenerProgress(float progress)
        {
            taskProgressBar.Value = progress;
        }
    }
}