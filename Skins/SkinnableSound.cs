using PBFramework.Audio;

namespace PBGame.Skins
{
    public class SkinnableSound : Skinnable<IEffectAudio> {

        public override void Dispose()
        {
            if (IsDefaultAsset || Element == null) return;

            Element.Dispose();
            Element = null;
        }
    }
}