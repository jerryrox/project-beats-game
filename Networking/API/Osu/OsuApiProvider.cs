using System;
using System.Collections;
using System.Collections.Generic;

namespace PBGame.Networking.API.Osu
{
    public class OsuApiProvider : ApiProvider {

        public override ApiProviderType Type => ApiProviderType.Osu;

        public override bool IsOAuthLogin => true;

        public override string Name => "osu!";


        public OsuApiProvider(IApi api) : base(api)
        {
            
        }
    }
}