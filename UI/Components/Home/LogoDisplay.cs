using PBGame.Maps;
using PBGame.Audio;
using PBGame.Animations;
using PBFramework.Animations;
using PBFramework.Dependencies;

namespace PBGame.UI.Components.Home
{
    public class LogoDisplay : Components.LogoDisplay, ILogoDisplay {

        private IAnime pulseAni;

        public float PulseDuration
        {
            get => 1f / pulseAni.Speed;
            set => pulseAni.Speed = 1f / value;
        }

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IMetronome Metronome { get; set; }


        [InitWithDependency]
        private void Init(IAnimePreset animePreset)
        {
            pulseAni = animePreset.GetHomeLogoPulse(this);
        }

        public void SetPulseProgress(float progress)
        {
            pulseAni.Seek(progress);
        }

        public void PlayPulse()
        {
            pulseAni.PlayFromStart();
        }

        public void StopPulse()
        {
            pulseAni.Stop();
        }

        /// <summary>
        /// Binds events to external dependencies.
        /// </summary>
        private void BindEvents()
        {
            Metronome.OnBeat += PlayPulse;
        }

        /// <summary>
        /// Unbinds events hooked to external dependencies.
        /// </summary>
        private void UnbindEvents()
        {
            Metronome.OnBeat -= PlayPulse;
        }

        private void OnEnable()
        {
            BindEvents();
        }

        private void OnDisable()
        {
            UnbindEvents();
        }
    }
}