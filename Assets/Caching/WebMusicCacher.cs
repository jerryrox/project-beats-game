using PBFramework;
using PBFramework.Audio;
using PBFramework.Networking;
using PBFramework.Allocation.Caching;

namespace PBGame.Assets.Caching
{
    public class WebMusicCacher : Cacher<IMusicAudio>, IWebMusicCacher {

        protected override IExplicitPromise<IMusicAudio> CreateRequest(string key)
        {
            return new MusicAudioRequest(key);
        }
    }
}