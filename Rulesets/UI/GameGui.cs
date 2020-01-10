using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets.UI
{
    public abstract class GameGui : UguiObject, IGameGui {
    
        public IStoryboardLayer StoryboardLayer { get; private set; }

        public IGameplayLayer GameplayLayer { get; private set; }


        [InitWithDependency]
        private void Init()
        {
            StoryboardLayer = CreateStoryboardLayer();
            {
                StoryboardLayer.Depth = 0;
                StoryboardLayer.Anchor = Anchors.Fill;
                StoryboardLayer.RawSize = Vector2.zero;
            }
            GameplayLayer = CreateGameplayLayer();
            {
                GameplayLayer.Depth = 1;
                GameplayLayer.Anchor = Anchors.Fill;
                GameplayLayer.RawSize = Vector2.zero;
            }
        }

        protected virtual IStoryboardLayer CreateStoryboardLayer() => new StoryboardLayer();

        protected abstract IGameplayLayer CreateGameplayLayer();
    }
}