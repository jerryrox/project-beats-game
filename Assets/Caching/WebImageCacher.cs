using PBFramework.Threading;
using PBFramework.Networking;
using PBFramework.Allocation.Caching;
using UnityEngine;

namespace PBGame.Assets.Caching
{
    public class WebImageCacher : Cacher<string, Texture2D>, IWebImageCacher
    {
        protected override ITask<Texture2D> CreateRequest(string key)
        {
            return new TextureRequest(key);
        }

        protected override void DestroyData(Texture2D data)
        {
            if(data != null)
                Object.Destroy(data);
        }
    }
}