using PBFramework.Graphics;

namespace PBGame.Rulesets.UI
{
    public interface IGameGui : IGraphicObject {

        /// <summary>
        /// Returns the storyboard layer object.
        /// </summary>
        IStoryboardLayer StoryboardLayer { get; }

        /// <summary>
        /// Returns the gameplay layer object.
        /// </summary>
        IGameplayLayer GameplayLayer { get; }
    }
}