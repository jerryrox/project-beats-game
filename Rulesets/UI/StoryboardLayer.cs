using PBGame.Rulesets.UI.Components;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.UI
{
    public class StoryboardLayer : UguiObject {

        private BackgroundDimmer dimmer;


        [InitWithDependency]
        private void Init()
        {
            dimmer = CreateChild<BackgroundDimmer>("dimmer", 0);
            {
                dimmer.Anchor = AnchorType.Fill;
                dimmer.Offset = Offset.Zero;
            }

            // TODO: Implement
        }
    }
}