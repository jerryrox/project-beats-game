using System;
using System.Collections;
using System.Collections.Generic;

namespace PBGame.Networking.API.Bloodcat
{
    public class BloodcatApiProvider : ApiProvider {

        public override ApiProviderType Type => ApiProviderType.Bloodcat;

        public override string Name => "BloodCat";
        

        public BloodcatApiProvider(IApi api) : base(api)
        {
    
        }
    }
}