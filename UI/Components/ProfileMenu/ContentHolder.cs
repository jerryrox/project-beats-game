using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.Data.Users;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Components;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.ProfileMenu
{
    public class ContentHolder : UguiObject {

        private const float LoggedInHeight = 480f;

        private ISprite background;

        private ISprite mask;
        private LoggedInView loggedInView;
        private LoggedOutView loggedOutView;
        private Blocker blocker;

        private IAnime loggedInAni;
        private IAnime loggedOutAni;


        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }

        [ReceivesDependency]
        private ProfileMenuModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            background = CreateChild<UguiSprite>("background", 1);
            {
                background.Anchor = AnchorType.Fill;
                background.Offset = Offset.Zero;
                background.Color = ColorPreset.Background;
            }
            mask = CreateChild<UguiSprite>("mask", 3);
            {
                mask.Anchor = AnchorType.Fill;
                mask.RawSize = Vector2.zero;
                mask.Position = Vector2.zero;
                mask.SpriteName = "box";

                var maskEffect = mask.AddEffect(new MaskEffect());
                maskEffect.Component.showMaskGraphic = false;

                loggedInView = mask.CreateChild<LoggedInView>("logged-in", 0);
                {
                    loggedInView.Anchor = AnchorType.Fill;
                    loggedInView.Offset = Offset.Zero;
                }
                loggedOutView = mask.CreateChild<LoggedOutView>("logged-out", 0);
                {
                    loggedOutView.Anchor = AnchorType.Fill;
                    loggedOutView.Offset = Offset.Zero;

                    loggedOutView.CurLoginView.OnNewValue += OnLoginProviderViewChange;
                }
            }
            blocker = CreateChild<Blocker>("blocker", 4);

            loggedInAni = new Anime();
            loggedInAni.AnimateFloat(alpha => loggedInView.Alpha = alpha)
                .AddTime(0f, () => loggedInView.Alpha)
                .AddTime(0.25f, 1f)
                .Build();
            loggedInAni.AnimateFloat(alpha => loggedOutView.Alpha = alpha)
                .AddTime(0f, () => loggedOutView.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
            loggedInAni.AnimateFloat(height => this.Height = height)
                .AddTime(0f, () => this.Height)
                .AddTime(0.25f, LoggedInHeight)
                .Build();
            loggedInAni.AddEvent(0f, () => loggedInView.Active = blocker.Active = true);
            loggedInAni.AddEvent(loggedInAni.Duration, () => loggedOutView.Active = blocker.Active = false);

            loggedOutAni = new Anime();
            loggedOutAni.AnimateFloat(alpha => loggedInView.Alpha = alpha)
                .AddTime(0f, () => loggedInView.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
            loggedOutAni.AnimateFloat(alpha => loggedOutView.Alpha = alpha)
                .AddTime(0f, () => loggedOutView.Alpha)
                .AddTime(0.25f, 1f)
                .Build();
            loggedOutAni.AnimateFloat(height => this.Height = height)
                .AddTime(0f, () => this.Height)
                .AddTime(0.25f, () => loggedOutView.DesiredHeight)
                .Build();
            loggedOutAni.AddEvent(0f, () => loggedOutView.Active = blocker.Active = true);
            loggedOutAni.AddEvent(loggedInAni.Duration, () => loggedInView.Active = blocker.Active = false);

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.CurrentUser.OnNewValue += OnUserChange;

            SwitchView(Model.CurrentUser.Value != null, false);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            Model.CurrentUser.OnNewValue -= OnUserChange;
        }

        private void SwitchView(bool isLoggedIn, bool animate = true)
        {
            // No animation
            if (!animate)
            {
                if (isLoggedIn)
                {
                    Height = LoggedInHeight;
                    loggedInView.Alpha = 1f;
                    loggedInView.Active = true;
                    loggedOutView.Alpha = 0f;
                    loggedOutView.Active = false;
                }
                else
                {
                    Height = loggedOutView.DesiredHeight;
                    loggedInView.Alpha = 0f;
                    loggedInView.Active = false;
                    loggedOutView.Alpha = 1f;
                    loggedOutView.Active = true;
                }
                blocker.Active = false;
            }
            // Animate
            else
            {
                if (isLoggedIn)
                {
                    if (!loggedInView.Active)
                    {
                        loggedOutAni.Stop();
                        loggedInAni.PlayFromStart();
                    }
                }
                else
                {
                    if (!loggedOutView.Active)
                    {
                        loggedInAni.Stop();
                        loggedOutAni.PlayFromStart();
                    }
                }
            }
        }

        /// <summary>
        /// Event called on online user change.
        /// </summary>
        private void OnUserChange(IUser user)
        {
            SwitchView(user != null);
        }

        /// <summary>
        /// Event called from logged out view when its current login provider view has changed.
        /// </summary>
        private void OnLoginProviderViewChange(BaseLoginView loginView)
        {
            // Animate height change.
            loggedOutAni.PlayFromStart();
        }
    }
}