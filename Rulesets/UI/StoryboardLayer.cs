using PBGame.Rulesets.UI.Components;
using PBGame.Rulesets.Storyboarding;
using PBFramework.Audio;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.Rulesets.UI
{
    public class StoryboardLayer : UguiObject {

        private TimingPointProcessor timingPointProcessor;

        private BackgroundDimmer dimmer;


        [ReceivesDependency]
        private IMusicController MusicController { get; set; }


        [InitWithDependency]
        private void Init(IGameSession session)
        {
            session.OnHardInit += () =>
            {
                timingPointProcessor.Initialize(session.CurrentMap);
            };
            session.OnHardDispose += () =>
            {
                timingPointProcessor.Dispose();
            };
            session.OnSoftInit += () =>
            {
                timingPointProcessor.Reset();
            };

            timingPointProcessor = new TimingPointProcessor();

            dimmer = CreateChild<BackgroundDimmer>("dimmer", 0);
            {
                dimmer.Anchor = AnchorType.Fill;
                dimmer.Offset = Offset.Zero;
            }
        }

        protected void Update()
        {
            float currentTime = MusicController.CurrentTime;

            timingPointProcessor.Update(currentTime);
        }
    }
}