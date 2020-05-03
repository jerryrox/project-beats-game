using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.UI.Components.GameLoad;
using PBGame.UI.Navigations.Screens;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Shaders;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class GameLoadOverlay : BaseOverlay, IGameLoadOverlay {

        /// <summary>
        /// Amount of extra delay to add before considering component show ani as finished playing.
        /// </summary>
        private const float ShowAniEndDelay = 1.5f;

        private InfoDisplayer infoDisplayer;
        private LoadIndicator loadIndicator;

        private IAnime componentShowAni;
        private IAnime componentHideAni;


        protected override int OverlayDepth => ViewDepths.GameLoadOverlay;

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }


        [InitWithDependency]
        private void Init()
        {
            var blur = CreateChild<BlurDisplay>("blur", 0);
            {
                blur.Anchor = AnchorType.Fill;
                blur.Offset = Offset.Zero;

                var dark = blur.CreateChild<UguiSprite>("dark", 0);
                {
                    dark.Anchor = AnchorType.Fill;
                    dark.Offset = Offset.Zero;
                    dark.Color = new Color(0f, 0f, 0f, 0.75f);
                }
            }
            infoDisplayer = CreateChild<InfoDisplayer>("info", 1);
            {
            }
            loadIndicator = CreateChild<LoadIndicator>("load", 2);
            {
                loadIndicator.Position = new Vector3(0f, -260f);
                loadIndicator.Size = new Vector2(88f, 88f);
            }

            componentShowAni = new Anime();
            componentShowAni.AddEvent(0f, () =>
            {
                infoDisplayer.Show();
                loadIndicator.Show();
            });
            componentShowAni.AddEvent(Mathf.Max(infoDisplayer.ShowAniDuration, loadIndicator.ShowAniDuration) + ShowAniEndDelay, OnShowAniEnd);

            componentHideAni = new Anime();
            componentHideAni.AddEvent(0f, () =>
            {
                infoDisplayer.Hide();
                loadIndicator.Hide();
            });
            componentHideAni.AddEvent(Mathf.Max(infoDisplayer.HideAniDuration, loadIndicator.HideAniDuration), OnHideAniEnd);
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();

            componentShowAni.PlayFromStart();
            // TODO: Start loading map assets
        }

        /// <summary>
        /// Start hiding the screen and navigate to game screen.
        /// </summary>
        private void StartHide()
        {
            // If the hide animation is playing, the player must've navigated out before this was executed.
            if(HideAnime.IsPlaying)
                return;
            componentHideAni.PlayFromStart();
        }

        /// <summary>
        /// Event called from component show ani when it has finished animating.
        /// </summary>
        private void OnShowAniEnd()
        {
            // TODO: If loading is finished, start navigating to game.
            StartHide();
        }

        /// <summary>
        /// Event called from component hide ani when it has finished animating.
        /// </summary>
        private void OnHideAniEnd()
        {
            // TODO: Move on to game screen.
            OverlayNavigator.Hide(this);
            ScreenNavigator.Show<PrepareScreen>();
        }

        /// <summary>
        /// Event called when the game has been loaded and is ready to play.
        /// </summary>
        private void OnGameLoadEnd()
        {
            // If show animation is not playing, go straight to navigating to game..
            if(!componentShowAni.IsPlaying)
                StartHide();
        }
    }
}