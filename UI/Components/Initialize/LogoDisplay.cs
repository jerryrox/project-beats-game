using System;
using PBGame.Animations;
using PBFramework.Animations;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.Initialize
{
    public class LogoDisplay : Components.LogoDisplay, ILogoDisplay {

        private IAnime startupAnime;
        private IAnime breatheAnime;
        private IAnime endAnime;


        public Action OnStartup { get; set; }

        public Action OnEnd { get; set; }

        [ReceivesDependency]
        private IAnimePreset AnimePreset { get; set; }


        [InitWithDependency]
        private void Init()
        {
            startupAnime = AnimePreset.GetInitLogoStartup(this);
            startupAnime.AddEvent(startupAnime.Duration, () => OnStartup?.Invoke());

            breatheAnime = AnimePreset.GetInitLogoBreathe(this);

            endAnime = AnimePreset.GetInitLogoEnd(this);
            endAnime.AddEvent(endAnime.Duration, () => OnEnd?.Invoke());
        }

        public void PlayStartup()
        {
            breatheAnime.Stop();
            endAnime.Stop();
            startupAnime.PlayFromStart();
        }

        public void PlayBreathe()
        {
            startupAnime.Stop();
            endAnime.Stop();
            breatheAnime.PlayFromStart();
        }

        public void PlayEnd()
        {
            startupAnime.Stop();
            breatheAnime.Stop();
            endAnime.PlayFromStart();
        }
    }
}