using PBGame.Rulesets.Maps;
using PBFramework.Audio;
using PBFramework.Threading;
using PBFramework.Networking;
using PBFramework.Allocation.Caching;

namespace PBGame.Assets.Caching
{
    public class MusicCacher : Cacher<IMap, IMusicAudio>, IMusicCacher {

        protected override object ConvertKey(IMap key) => key.Detail.GetFullAudioPath();

        protected override ITask<IMusicAudio> CreateRequest(IMap key)
        {
            return new MusicAudioRequest(key.Detail.GetFullAudioPath(), true);
        }

        protected override void DestroyData(IMusicAudio data) => data.Dispose();
    }
}