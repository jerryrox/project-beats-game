using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.UI.Components.GameLoad;
using PBGame.UI.Navigations.Screens;
using PBGame.Maps;
using PBGame.Rulesets;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Audio;
using PBFramework.Utils;
using PBFramework.Inputs;
using PBFramework.Graphics;
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

        private IKey escapeKey;


        protected override int OverlayDepth => ViewDepths.GameLoadOverlay;

        /// <summary>
        /// Returns the game screen instance.
        /// </summary>
        private GameScreen GameScreen => ScreenNavigator.Get<GameScreen>();

        [ReceivesDependency]
        private IModeManager ModeManager { get; set; }

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }

        [ReceivesDependency]
        private IMusicController MusicController { get; set; }


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

            float showDur = Mathf.Max(infoDisplayer.ShowAniDuration, loadIndicator.ShowAniDuration);
            componentShowAni = new Anime();
            componentShowAni.AddEvent(0f, () =>
            {
                infoDisplayer.Show();
                loadIndicator.Show();
            });
            componentShowAni.AnimateFloat(v => MusicController.SetFade(v))
                .AddTime(0f, 1f, EaseType.QuadEaseOut)
                .AddTime(showDur, 0.5f)
                .Build();
            componentShowAni.AddEvent(showDur + ShowAniEndDelay, OnShowAniEnd);

            float hideDur = Mathf.Max(infoDisplayer.HideAniDuration, loadIndicator.HideAniDuration);
            componentHideAni = new Anime();
            componentHideAni.AddEvent(0f, () =>
            {
                infoDisplayer.Hide();
                loadIndicator.Hide();
            });
            componentHideAni.AnimateFloat(v => MusicController.SetFade(v))
                .AddTime(0f, 0.5f, EaseType.QuadEaseOut)
                .AddTime(hideDur, 0f)
                .Build();
            componentHideAni.AddEvent(hideDur, () =>
            {
                MusicController.SetFade(1f);
                OnHideAniEnd();
            });
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            SetBindEscape(false);
        }

        public override bool ProcessInput()
        {
            if (escapeKey.State.Value == InputState.Press)
            {
                ChangeScreen(false);
            }
            return false;
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();

            componentShowAni.PlayFromStart();

            var modeServicer = ModeManager.GetService(GameConfiguration.RulesetMode.Value);
            var gameScreen = ScreenNavigator.CreateHidden<GameScreen>();

            // Receive an event when the loading is complete.
            SetBindGameScreen(true);

            // Start loading the game.
            gameScreen.PreInitialize(MapSelection.Map, modeServicer);

            // User may escape out of the game at any time.
            SetBindEscape(true);
        }

        /// <summary>
        /// Changes to the next screen based on specified flag.
        /// </summary>
        private void ChangeScreen(bool toGame)
        {
            // Don't listen to escape key anymore.
            SetBindEscape(false);

            if (toGame)
            {
                // If the hide animation is playing, the player must've navigated out before this was executed.
                if (HideAnime.IsPlaying)
                    return;
                componentHideAni.PlayFromStart();
            }
            else
            {
                // Stop everything related to game load.
                GameScreen.CancelLoad();
                componentShowAni.Stop();
                componentHideAni.Stop();

                // Revert volume fade.
                MusicController.SetFade(1f);

                NavigateToScreen<PrepareScreen>();
            }
        }

        /// <summary>
        /// Navigates the specified screen while hiding this overlay.
        /// </summary>
        private void NavigateToScreen<T>()
            where T : BaseScreen
        {
            ScreenNavigator.Show<T>();
            OverlayNavigator.Hide(this);

            // Start the game straight away.
            if(typeof(T) == typeof(GameScreen))
                GameScreen.StartInitialGame();
        }

        /// <summary>
        /// Binds escape key event.
        /// </summary>
        private void SetBindEscape(bool bind)
        {
            if (bind)
            {
                if (escapeKey == null)
                {
                    escapeKey = InputManager.AddKey(KeyCode.Escape);
                    SetReceiveInputs(true);
                }
            }
            else
            {
                if (escapeKey != null)
                {
                    InputManager.RemoveKey(KeyCode.Escape);
                    SetReceiveInputs(false);
                }
                escapeKey = null;
            }
        }

        /// <summary>
        /// Sets binding to game screen.
        /// </summary>
        private void SetBindGameScreen(bool bind)
        {
            if(bind)
                GameScreen.OnPreInit += OnGameLoadEnd;
            else
                GameScreen.OnPreInit -= OnGameLoadEnd;
        }

        /// <summary>
        /// Event called from component show ani when it has finished animating.
        /// </summary>
        private void OnShowAniEnd() => ChangeScreen(GameScreen.IsGameLoaded);

        /// <summary>
        /// Event called from component hide ani when it has finished animating.
        /// </summary>
        private void OnHideAniEnd() => NavigateToScreen<GameScreen>();

        /// <summary>
        /// Event called when the game has been loaded and is ready to play.
        /// </summary>
        private void OnGameLoadEnd(bool isSuccess)
        {
            // Unbind from game screen.
            SetBindGameScreen(false);

            if(componentShowAni.IsPlaying)
                return;
            ChangeScreen(isSuccess);
        }
    }
}