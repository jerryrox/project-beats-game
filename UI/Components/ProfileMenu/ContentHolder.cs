using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Data.Users;
using PBGame.Networking.API;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Components;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.ProfileMenu
{
    // TODO: Support for logging in using other API providers.
    public class ContentHolder : UguiObject {

        private const float LoggedInHeight = 480f;
        private const float LoggedOutHeight = 204f;

        private ISprite background;

        private ISprite mask;
        private LoggedInView loggedInView;
        private LoggedOutView loggedOutView;
        private ISprite pointerBlocker;

        private IAnime loggedInAni;
        private IAnime loggedOutAni;


        [ReceivesDependency]
        private IUserManager UserManager { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Height = LoggedOutHeight;

            background = CreateChild<UguiSprite>("background", 1);
            {
                background.Anchor = AnchorType.Fill;
                background.RawSize = Vector2.zero;
                background.Position = Vector2.zero;
                background.Color = new Color(0f, 0f, 0f, 0.25f);
            }
            mask = CreateChild<UguiSprite>("mask", 3);
            {
                mask.Anchor = AnchorType.Fill;
                mask.RawSize = Vector2.zero;
                mask.Position = Vector2.zero;
                mask.SpriteName = "box";

                mask.AddEffect(new MaskEffect());

                loggedInView = mask.CreateChild<LoggedInView>("logged-in", 0);
                {
                    loggedInView.Anchor = AnchorType.BottomStretch;
                    loggedInView.Pivot = PivotType.Bottom;
                    loggedInView.SetOffsetHorizontal(0f);
                    loggedInView.Y = 0f;
                    loggedInView.Height = LoggedInHeight;
                }
                loggedOutView = mask.CreateChild<LoggedOutView>("logged-out", 0);
                {
                    loggedOutView.Anchor = AnchorType.BottomStretch;
                    loggedOutView.Pivot = PivotType.Bottom;
                    loggedOutView.SetOffsetHorizontal(0f);
                    loggedOutView.Y = 0f;
                    loggedOutView.Height = LoggedOutHeight;
                }
            }
            pointerBlocker = CreateChild<UguiSprite>("blocker", 4);
            {
                pointerBlocker.Anchor = AnchorType.Fill;
                pointerBlocker.RawSize = Vector2.zero;
                pointerBlocker.Position = Vector2.zero;
                pointerBlocker.Alpha = 0f;
                pointerBlocker.Active = false;
            }

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
            loggedInAni.AddEvent(0f, () => loggedInView.Active = pointerBlocker.Active = true);
            loggedInAni.AddEvent(loggedInAni.Duration, () => loggedOutView.Active = pointerBlocker.Active = false);

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
                .AddTime(0.25f, LoggedOutHeight)
                .Build();
            loggedOutAni.AddEvent(0f, () => loggedOutView.Active = pointerBlocker.Active = true);
            loggedOutAni.AddEvent(loggedInAni.Duration, () => loggedInView.Active = pointerBlocker.Active = false);

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();
            BindEvents();
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            UnbindEvents();
        }

        /// <summary>
        /// Binds to external dependency events.
        /// </summary>
        private void BindEvents()
        {
            UserManager.CurrentUser.OnValueChanged += OnUserChange;

            SwitchView(UserManager.CurrentUser.Value != null, false);
        }
        
        /// <summary>
        /// Unbinds from external dependency events.
        /// </summary>
        private void UnbindEvents()
        {
            UserManager.CurrentUser.OnValueChanged -= OnUserChange;
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
                    Height = LoggedOutHeight;
                    loggedInView.Alpha = 0f;
                    loggedInView.Active = false;
                    loggedOutView.Alpha = 1f;
                    loggedOutView.Active = true;
                }
                pointerBlocker.Active = false;
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
        private void OnUserChange(IUser user, IUser _ = null)
        {
            SwitchView(user != null);
        }
    }
}