using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Animations;
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

        [ReceivesDependency]
        private IAnimePreset AnimePreset { get; set; }


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
                        OverlayNavigator.Hide(this);
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

        protected override void OnPreHide()
        {
            base.OnPreHide();

            OnClose?.Invoke();
            OnClose = null;
        }

        protected override IAnime CreateShowAnime(IDependencyContainer dependencies)
        {
            return AnimePreset.GetSubMenuOverlayShow(this);
        }

        protected override IAnime CreateHideAnime(IDependencyContainer dependencies)
        {
            return AnimePreset.GetSubMenuOverlayHide(this);
        }
    }
}