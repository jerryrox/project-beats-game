using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Networking;
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
    public class ContentHolder : UguiObject, IContentHolder {

        private const float LoggedInHeight = 480f;
        private const float LoggedOutHeight = 204f;

        private ISprite glow;
        private ISprite background;

        private ISprite mask;
        private ILoggedInView loggedInView;
        private ILoggedOutView loggedOutView;
        private ISprite pointerBlocker;

        private IAnime loggedInAni;
        private IAnime loggedOutAni;


        public ISprite GlowSprite => glow;

        /// <summary>
        /// Returns the osu api provider from manager.
        /// </summary>
        private IApi OsuApi => ApiManager.GetApi(ApiProviders.Osu);

        [ReceivesDependency]
        private IApiManager ApiManager { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Height = LoggedOutHeight;

            glow = CreateChild<UguiSprite>("glow", 0);
            {
                glow.Anchor = Anchors.Fill;
                glow.RawSize = new Vector2(30f, 30f);
                glow.SpriteName = "square-32-glow";
                glow.ImageType = Image.Type.Sliced;
                glow.Color = Color.black;
            }
            background = CreateChild<UguiSprite>("background", 1);
            {
                background.Anchor = Anchors.Fill;
                background.RawSize = Vector2.zero;
                background.Position = Vector2.zero;
                background.Color = new Color(0f, 0f, 0f, 0.25f);
            }
            mask = CreateChild<UguiSprite>("mask", 3);
            {
                mask.Anchor = Anchors.Fill;
                mask.RawSize = Vector2.zero;
                mask.Position = Vector2.zero;
                mask.SpriteName = "box";

                mask.AddEffect(new MaskEffect());

                loggedInView = mask.CreateChild<LoggedInView>("logged-in", 0);
                {
                    loggedInView.Anchor = Anchors.BottomStretch;
                    loggedInView.Pivot = Pivots.Bottom;
                    loggedInView.OffsetLeft = loggedInView.OffsetRight = 0f;
                    loggedInView.Y = 0f;
                    loggedInView.Height = LoggedInHeight;
                }
                loggedOutView = mask.CreateChild<LoggedOutView>("logged-out", 0);
                {
                    loggedOutView.Anchor = Anchors.BottomStretch;
                    loggedOutView.Pivot = Pivots.Bottom;
                    loggedOutView.OffsetLeft = loggedOutView.OffsetRight = 0f;
                    loggedOutView.Y = 0f;
                    loggedOutView.Height = LoggedOutHeight;
                }
            }
            pointerBlocker = CreateChild<UguiSprite>("blocker", 4);
            {
                pointerBlocker.Anchor = Anchors.Fill;
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
            OsuApi.User.OnValueChanged += OnUserChange;

            SwitchView(OsuApi.User.Value.IsOnline, false);
        }
        
        /// <summary>
        /// Unbinds from external dependency events.
        /// </summary>
        private void UnbindEvents()
        {
            OsuApi.User.OnValueChanged -= OnUserChange;
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
        private void OnUserChange(IOnlineUser user, IOnlineUser _ = null)
        {
            SwitchView(user.IsOnline);
        }
    }
}