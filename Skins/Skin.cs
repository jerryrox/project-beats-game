using System.IO;
using System.Collections;
using PBGame.Stores;
using PBFramework.DB.Entities;
using Newtonsoft.Json;

namespace PBGame.Skins
{
    public class Skin : DatabaseEntity, ISkin {

        public int HashCode { get; set; }

        [JsonIgnore]
        public SkinMetadata Metadata { get; private set; } = new SkinMetadata();

        [JsonIgnore]
        public ISkinAssetStore AssetStore { get; protected set; }

        [JsonIgnore]
        public DirectoryInfo Directory { get; set; }

        [JsonIgnore]
        public ISkin Fallback { get; set; }


        public Skin()
        {
            AssetStore = new SkinAssetStore(this);
        }

        public IEnumerable GetHashParams()
        {
            yield return Metadata.Name;
            yield return Metadata.Creator;
        }
    }
}