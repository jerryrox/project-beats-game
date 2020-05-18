using PBGame.Maps;
using PBFramework;
using PBFramework.Networking;

namespace PBGame.Assets
{
    public class MapBackgroundRequest : ProxyPromise<IMapBackground> {

        /// <summary>
        /// The inner request being wrapped over.
        /// </summary>
        private TextureRequest textureRequest;


        public MapBackgroundRequest(string url)
        {
            textureRequest = new TextureRequest(url, false);

            StartAction = (promise) => textureRequest.Start();
            RevokeAction = textureRequest.Revoke;

            textureRequest.OnProgress += SetProgress;
            textureRequest.OnFinishedResult += (texture) => Resolve(new MapBackground(texture));
        }
    }
}