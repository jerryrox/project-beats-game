using PBGame.Maps;
using PBFramework.Threading.Futures;
using PBFramework.Networking;
using UnityEngine;

namespace PBGame.Assets
{
    public class MapBackgroundRequest : ProxyFuture<Texture2D, IMapBackground> {

        public MapBackgroundRequest(string url) : base(new TextureRequest(url, false))
        {
        }

        protected override IMapBackground ConvertOutput(Texture2D source) => new MapBackground(source);
    }
}