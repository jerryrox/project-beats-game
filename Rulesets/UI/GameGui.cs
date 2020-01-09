using PBFramework.Graphics;
using PBFramework.Dependencies;

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
            }
            GameplayLayer = CreateGameplayLayer();
            {
                GameplayLayer.Depth = 1;
            }
        }

        protected virtual IStoryboardLayer CreateStoryboardLayer() => new StoryboardLayer();

        protected abstract IGameplayLayer CreateGameplayLayer();
    }
}