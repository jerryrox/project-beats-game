using System.Collections.Generic;
using PBGame.Rulesets;

namespace PBGame.Networking.API.Osu
{
    public class OsuApi : BaseApi {

        private OsuAdaptor adaptor = new OsuAdaptor();
        private OsuRequestFactory factory = new OsuRequestFactory();


        public override string BaseUrl => "https://osu.ppy.sh";

        public override API.ApiProviderType ApiType => API.ApiProviderType.Osu;

        public override string Name => "osu!";

        public override string IconName => "icon-provider-osu";

        public override IApiAdaptor Adaptor => adaptor;

        public override IRequestFactory RequestFactory => factory;


        public override IEnumerable<GameModeType> GetGameModes()
        {
            yield return GameModeType.OsuStandard;
        }
    }
}