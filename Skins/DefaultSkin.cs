using PBGame.Stores;

namespace PBGame.Skins
{
    public class DefaultSkin : Skin {
    
        public DefaultSkin()
        {
            AssetStore = new DefaultSkinAssetStore(this);
        }
    }
}