using PBFramework.Audio;
using PBFramework.Threading.Futures;
using PBFramework.Networking;
using PBFramework.Allocation.Caching;

namespace PBGame.Assets.Caching
{
    public class WebMusicCacher : Cacher<IMusicAudio>, IWebMusicCacher {

        protected override IControlledFuture<IMusicAudio> CreateRequest(string key)
        {
            return new MusicAudioRequest(key);
        }

        protected override void DestroyData(IMusicAudio data) => data.Dispose();
    }
}