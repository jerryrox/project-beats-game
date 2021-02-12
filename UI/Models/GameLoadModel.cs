using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models.Game;
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

        /// <summary>
        /// Event calld when the game loading has succeeded.
        /// </summary>
        public event Action OnLoadSucceed;

        /// <summary>
        /// Event called when the game loading has failed.
        /// </summary>
        public event Action OnLoadFail;

        private IKey escapeKey;
        private bool isShowAniEnded;

        private Bindable<GameLoadState> loadingState = new Bindable<GameLoadState>();


        /// <summary>
        /// Amount of extra delay to add before considering component show ani as finished playing.
        /// </summary>
        public float MinimumLoadTime => 1.5f;

        /// <summary>
        /// Returns whether the game is currently loading.
        /// </summary>
        public bool IsLoading => loadingState.Value == GameLoadState.Idle || loadingState.Value == GameLoadState.Loading;

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

        /// <summary>
        /// Returns the game screen's model instance.
        /// </summary>
        public GameModel GameModel => GameScreen.Model;

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

            isShowAniEnded = false;

            // Slightly fade out music.
            MusicController.Fade(0.5f);

            // Listen to escape key.
            escapeKey = InputManager.AddKey(KeyCode.Escape);
            escapeKey.State.OnNewValue += OnEscapeKeyState;
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            isShowAniEnded = false;

            loadingState.Value = GameLoadState.Idle;
            loadingState.UnbindFrom(GameModel.LoadState);

            UnbindEscape();

            GameModel.LoadState.OnNewValue -= OnLoadStateChange;
        }

        /// <summary>
        /// Starts loading a new game session instance.
        /// </summary>
        public void StartLoad(GameParameter parameter)
        {
            var gameScreen = ScreenNavigator.CreateHidden<GameScreen>();
            var selectedMap = SelectedMap.Value;
            var modeServicer = ModeManager.GetService(selectedMap.PlayableMode);
            loadingState.BindTo(GameModel.LoadState);

            // Start loading the game.
            GameModel.LoadGame(parameter, modeServicer);

            // Listen to game screen init event.
            GameModel.LoadState.BindAndTrigger(OnLoadStateChange);
        }

        /// <summary>
        /// Event called from view when the show animation is finished.
        /// </summary>
        public void OnShowAniEnd()
        {
            isShowAniEnded = true;
            EvaluateLoadState();
        }

        /// <summary>
        /// Proceeds onward to the game screen.
        /// </summary>
        public void NavigateToGame()
        {
            NavigateToScreen<GameScreen>();
            GameScreen.Model.StartGame();
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
        /// Performs success or cancel actions based on current loading state.
        /// </summary>
        private void EvaluateLoadState()
        {
            if (loadingState.Value == GameLoadState.Success)
            {
                if(IsLoading || !isShowAniEnded)
                    return;
                SucceedLoading();
            }
            else if (loadingState.Value == GameLoadState.Fail)
                CancelLoading();
        }

        /// <summary>
        /// Handles actions for a successful loading of the game session.
        /// </summary>
        private void SucceedLoading()
        {
            // Unbind escape key immediately.
            UnbindEscape();

            OnLoadSucceed?.Invoke();
        }

        /// <summary>
        /// Cancels loading the game.
        /// </summary>
        private void CancelLoading()
        {
            // Unbind escape key immediately.
            UnbindEscape();

            // Make the game screen completely stop and dispose current load session.
            GameModel.CancelLoad();

            NavigateToPrepare();

            OnLoadFail?.Invoke();
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
        /// Event called from GameModel when the game loading state has changed.
        /// </summary>
        private void OnLoadStateChange(GameLoadState loadState) => EvaluateLoadState();
    }
}