using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Beats.Standard.Objects;

namespace PBGame.Rulesets.Beats.Standard.Maps
{
    public class Map : PlayableMap<HitObject> {

        public Map(IOriginalMap map) : base(map) {}
    }
}