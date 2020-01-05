using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBFramework.Allocation.Caching;

namespace PBGame.Assets.Caching
{
    public interface IBackgroundCacher : ICacher<IMapBackground>, ICacher<IMap, IMapBackground> {
    
    }
}