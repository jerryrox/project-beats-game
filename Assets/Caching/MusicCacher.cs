using PBGame.Rulesets.Maps;
using PBFramework;
using PBFramework.Audio;
using PBFramework.Threading;
using PBFramework.Networking;
using PBFramework.Allocation.Caching;

namespace PBGame.Assets.Caching
{
    public class MusicCacher : Cacher<IMusicAudio>, IMusicCacher {

        public uint Request(IMap key, IReturnableProgress<IMusicAudio> progress)
        {
            return base.Request(key.Detail.GetFullAudioPath(), progress);
        }

        public void Remove(IMap key, uint id)
        {
            base.Remove(key.Detail.GetFullAudioPath(), id);
        }

        public void RemoveDelayed(IMap key, uint id, float delay = 2f)
        {
            base.RemoveDelayed(key.Detail.GetFullAudioPath(), id, delay);
        }

        public bool IsCached(IMap key)
        {
            return base.IsCached(key.Detail.GetFullAudioPath());
        }

        protected override IExplicitPromise<IMusicAudio> CreateRequest(string key)
        {
            return new MusicAudioRequest(key, true);
        }
    }
}