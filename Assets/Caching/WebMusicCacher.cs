using PBFramework.Audio;
using PBFramework.Threading;
using PBFramework.Networking;
using PBFramework.Allocation.Caching;

namespace PBGame.Assets.Caching
{
    public class WebMusicCacher : Cacher<string, IMusicAudio>, IWebMusicCacher {

        protected override ITask<IMusicAudio> CreateRequest(string key)
        {
            return new MusicAudioRequest(key);
        }

        protected override void DestroyData(IMusicAudio data) => data.Dispose();
    }
}