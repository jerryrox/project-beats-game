using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBFramework.Threading;
using PBFramework.Allocation.Caching;

namespace PBGame.Assets.Caching
{
    public class BackgroundCacher : Cacher<IMap, IMapBackground>, IBackgroundCacher {

        protected override object ConvertKey(IMap key) => key.Detail.GetFullBackgroundPath();

        protected override ITask<IMapBackground> CreateRequest(IMap key)
        {
            return new MapBackgroundRequest(key.Detail.GetFullBackgroundPath());
        }

        protected override void DestroyData(IMapBackground data) => data.Dispose();
    }
}