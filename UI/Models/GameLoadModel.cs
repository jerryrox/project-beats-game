using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models.GameLoad;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using PBGame.Maps;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Data.Bindables;
using PBFramework.Audio;
using PBFramework.Inputs;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Models
{
    public class GameLoadModel : BaseModel {
    
        private IKey escapeKey;

        private Bindable<GameLoadState> loadingState = new Bindable<GameLoadState>();


        /// <summary>
        /// Amount of extra delay to add before considering component show ani as finished playing.
        /// </summary>
        public float MinimumLoadTime => 1.5f;

        /// <summary>
        /// Returns the current game loading state.
        /// </summary>
        public IReadOnlyBindable<GameLoadState> LoadingState => loadingState;

        /// <summary>
        /// Returns whether unicode text is preferred.
        /// </summary>
        public IReadOnlyBindable<bool> PreferUnicode => GameConfiguration.PreferUnicode;

        /// <summary>
        /// Returns the currently selected map.
        /// </summary>
        public IReadOnlyBindable<IPlayableMap> SelectedMap => MapSelection.Map;

        /// <summary>
        /// Returns the currently selected map's background.
        /// </summary>
        public IReadOnlyBindable<IMapBackground> Background => MapSelection.Background;

        [ReceivesDependency]
        public IMusicController MusicController { get; set; }

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
        private IInputManager InputManager { get; set; }


        protected override void OnPreShow()
        {
            base.OnPreShow();

            loadingState.Value = GameLoadState.Loading;

            var modeServicer = ModeManager.GetService(GameConfiguration.RulesetMode.Value);
            var gameScreen = ScreenNavigator.CreateHidden<GameScreen>();

            // Start loading the game.
            gameScreen.PreInitialize(MapSelection.Map.Value, modeServicer);

            // Slightly fade out music.
            MusicController.Fade(0.5f);

            // Listen to escape key.
            escapeKey = InputManager.AddKey(KeyCode.Escape);
            escapeKey.State.OnNewValue += OnEscapeKeyState;

            // Listen to game screen init event.
            GameScreen.OnPreInit += OnGamePreInited;
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            loadingState.Value = GameLoadState.Loading;

            UnbindEscape();
            
            GameScreen.OnPreInit -= OnGamePreInited;
        }

        /// <summary>
        /// Event called from view when the show animation is finished.
        /// </summary>
        public void OnShowAniEnd()
        {
            SucceedLoading();
        }

        /// <summary>
        /// Proceeds onward to the game screen.
        /// </summary>
        public void NavigateToGame()
        {
            NavigateToScreen<GameScreen>();
            GameScreen.StartInitialGame();
        }

        /// <summary>
        /// Navigates back to the prepare screen.
        /// </summary>
        public void NavigateToPrepare()
        {
            NavigateToScreen<PrepareScreen>();
            // Revert music fade.
            MusicController.SetFade(1f);
        }

        /// <summary>
        /// Navigates to the next specified screen.
        /// </summary>
        private void NavigateToScreen<T>()
            where T : MonoBehaviour, INavigationView
        {
            ScreenNavigator.Show<T>();
            OverlayNavigator.Hide<GameLoadOverlay>();
        }

        /// <summary>
        /// Handles actions for a successful loading of the game session.
        /// </summary>
        private void SucceedLoading()
        {
            // Unbind escape key immediately.
            UnbindEscape();

            loadingState.Value = GameLoadState.Success;
        }

        /// <summary>
        /// Cancels loading the game.
        /// </summary>
        private void CancelLoading()
        {
            // Unbind escape key immediately.
            UnbindEscape();

            // Make the game screen completely stop and dispose current load session.
            GameScreen.CancelLoad();

            loadingState.Value = GameLoadState.Fail;
            NavigateToPrepare();
        }

        /// <summary>
        /// Disposes current escape key.
        /// </summary>
        private void UnbindEscape()
        {
            if(escapeKey == null)
                return;

            escapeKey.State.OnNewValue -= OnEscapeKeyState;
            InputManager.RemoveKey(KeyCode.Escape);
            escapeKey = null;
        }

        /// <summary>
        /// Event called on escape key input state change.
        /// </summary>
        private void OnEscapeKeyState(InputState state)
        {
            if (escapeKey.State.Value == InputState.Press)
                CancelLoading();
        }

        /// <summary>
        /// Event called on game screen pre init call.
        /// </summary>
        private void OnGamePreInited(bool isLoaded)
        {
            if(isLoaded)
                SucceedLoading();
            else
                CancelLoading();
        }
    }
}