using System;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets.UI
{
    public abstract class GameGui : UguiObject {

        private Action onShowAction;
        private Action onHideAction;


        /// <summary>
        /// Returns the storyboard layer object.
        /// </summary>
        public StoryboardLayer StoryboardLayer { get; private set; }

        /// <summary>
        /// Returns the gameplay layer object.
        /// </summary>
        public GameplayLayer GameplayLayer { get; private set; }

        [ReceivesDependency]
        protected IGameSession GameSession { get; set; }


        [InitWithDependency]
        private void Init()
        {
            StoryboardLayer = CreateStoryboardLayer();
            {
                StoryboardLayer.Depth = 0;
                StoryboardLayer.Anchor = AnchorType.Fill;
                StoryboardLayer.Offset = Offset.Zero;
            }
            GameplayLayer = CreateGameplayLayer();
            {
                GameplayLayer.Depth = 1;
                GameplayLayer.Anchor = AnchorType.Fill;
                GameplayLayer.Offset = Offset.Zero;

                GameplayLayer.ShowAni.AddEvent(GameplayLayer.ShowAni.Duration, () =>
                {
                    onShowAction?.Invoke();
                    onShowAction = null;
                });
                GameplayLayer.HideAni.AddEvent(GameplayLayer.HideAni.Duration, () =>
                {
                    onHideAction?.Invoke();
                    onHideAction = null;
                });
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            StopAni();
        }

        public void ShowGame(Action onShow)
        {
            StopAni();

            onShowAction = onShow;
            GameplayLayer.ShowAni.PlayFromStart();
        }

        public void HideGame(Action onHide)
        {
            StopAni();

            onHideAction = onHide;
            GameplayLayer.HideAni.PlayFromStart();
        }

        protected virtual StoryboardLayer CreateStoryboardLayer() => CreateChild<StoryboardLayer>("sb-layer");

        protected abstract GameplayLayer CreateGameplayLayer();

        /// <summary>
        /// Stops show/hide animations.
        /// </summary>
        private void StopAni()
        {
            onShowAction = null;
            onHideAction = null;
            if (GameplayLayer != null)
            {
                GameplayLayer.ShowAni.Stop();
                GameplayLayer.HideAni.Stop();
            }
        }
    }
}