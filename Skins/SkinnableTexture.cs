using UnityEngine;

namespace PBGame.Skins
{
    public class SkinnableTexture : Skinnable<Texture2D> {

        public override void Dispose()
        {
            if(IsDefaultAsset || Element == null) return;

            Object.Destroy(Element);
            Element = null;
        }
    }
}