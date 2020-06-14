using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBFramework;
using PBFramework.Threading;
using PBFramework.Allocation.Caching;

namespace PBGame.Assets.Caching
{
    public class BackgroundCacher : Cacher<IMapBackground>, IBackgroundCacher {

        public uint Request(IMap key, IReturnableProgress<IMapBackground> progress)
        {
            return base.Request(key.Detail.GetFullBackgroundPath(), progress);
        }

        public void Remove(IMap key, uint id)
        {
            base.Remove(key.Detail.GetFullBackgroundPath(), id);
        }

        public void RemoveDelayed(IMap key, uint id, float delay = 2f)
        {
            base.RemoveDelayed(key.Detail.GetFullBackgroundPath(), id, delay);
        }

        public bool IsCached(IMap key)
        {
            return base.IsCached(key.Detail.GetFullBackgroundPath());
        }

        protected override IExplicitPromise<IMapBackground> CreateRequest(string key)
        {
            return new MapBackgroundRequest(key);
        }

        protected override void DestroyData(IMapBackground data) => data.Dispose();
    }
}