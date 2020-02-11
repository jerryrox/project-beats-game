using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public abstract class BaseSubMenuOverlay : BaseOverlay, ISubMenuOverlay {

        public event Action OnClose;

        protected ISprite darkSprite;
        protected ITrigger closeTrigger;
        protected ITrigger container;

        protected IAnime hoverAni;
        protected IAnime outAni;

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }


        [InitWithDependency]
        private void Init()
        {
            darkSprite = CreateChild<UguiSprite>("dark", 0);
            {
                darkSprite.Anchor = Anchors.Fill;
                darkSprite.OffsetTop = 64f;
                darkSprite.OffsetLeft = 0f;
                darkSprite.OffsetRight = 0f;
                darkSprite.OffsetBottom = 0f;
                darkSprite.Color = new Color(0f, 0f, 0f, 0.5f);

                closeTrigger = darkSprite.CreateChild<UguiTrigger>("close", 0);
                {
                    closeTrigger.Anchor = Anchors.Fill;
                    closeTrigger.RawSize = Vector2.zero;

                    closeTrigger.OnPointerDown += () =>
                    {
                        CloseOverlay();
                    };
                }

                container = darkSprite.CreateChild<UguiTrigger>("container", 1);
                {
                    container.OnPointerEnter += () =>
                    {
                        outAni?.Stop();
                        hoverAni?.PlayFromStart();
                    };
                    container.OnPointerExit += () =>
                    {
                        hoverAni?.Stop();
                        outAni?.PlayFromStart();
                    };
                }
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            OnClose = null;
        }

        protected override IAnime CreateShowAnime(IDependencyContainer dependencies)
        {
            var anime = new Anime();
            anime.AnimateFloat(alpha => Alpha = alpha)
                .AddTime(0f, 0f, EaseType.QuadEaseOut)
                .AddTime(0.35f, 1f)
                .Build();
            return anime;
        }

        protected override IAnime CreateHideAnime(IDependencyContainer dependencies)
        {
            var anime = new Anime();
            anime.AnimateFloat(alpha => Alpha = alpha)
                .AddTime(0f, 1f, EaseType.QuadEaseOut)
                .AddTime(0.35f, 0f)
                .Build();
            return anime;
        }

        /// <summary>
        /// Closes the overlay with event invocation.
        /// </summary>
        protected virtual void CloseOverlay()
        {
            OnClose?.Invoke();
            OverlayNavigator.Hide(this);
        }
    }
}