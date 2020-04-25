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
using UnityEngine.UI;

namespace PBGame.UI.Navigations.Overlays
{
    public abstract class BaseSubMenuOverlay : BaseOverlay, ISubMenuOverlay {

        public event Action OnClose;

        protected ISprite darkSprite;
        protected ITrigger closeTrigger;
        protected ITrigger container;
        protected ISprite glowSprite;

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
                darkSprite.Offset = new Offset(0f, MenuBarHeight, 0f, 0f);
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

                    glowSprite = container.CreateChild<UguiSprite>("glow", -1);
                    {
                        glowSprite.Anchor = Anchors.Fill;
                        glowSprite.Offset = new Offset(-15f);
                        glowSprite.SpriteName = "square-32-glow";
                        glowSprite.ImageType = Image.Type.Sliced;
                        glowSprite.Color = Color.black;
                    }
                }
            }

            hoverAni = new Anime();
            hoverAni.AnimateColor(color => glowSprite.Color = color)
                .AddTime(0f, () => glowSprite.Color)
                .AddTime(0.25f, Color.gray)
                .Build();

            outAni = new Anime();
            outAni.AnimateColor(color => glowSprite.Color = color)
                .AddTime(0f, () => glowSprite.Color)
                .AddTime(0.25f, Color.black)
                .Build();
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            OnClose?.Invoke();
            OnClose = null;
        }

        protected override IAnime CreateShowAnime(IDependencyContainer dependencies)
        {
            return dependencies.Get<IAnimePreset>().GetSubMenuOverlayShow(this);
        }

        protected override IAnime CreateHideAnime(IDependencyContainer dependencies)
        {
            return dependencies.Get<IAnimePreset>().GetSubMenuOverlayHide(this);
        }
    }
}