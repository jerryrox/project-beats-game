using System.Collections.Generic;
using PBGame.Rulesets;

namespace PBGame.Networking.API.Osu
{
    public class OsuApi : BaseApi {

        public override string BaseUrl => "https://osu.ppy.sh";

        public override API.ApiProviders ApiType => API.ApiProviders.Osu;


        public override IEnumerable<GameModes> GetGameModes()
        {
            yield return GameModes.OsuStandard;
        }
    }
}