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


        public MapBackgroundRequest(string url, bool nonReadable = true)
        {
            textureRequest = new TextureRequest(url, nonReadable);

            startAction = textureRequest.Start;
            revokeAction = textureRequest.Revoke;

            textureRequest.OnProgress += SetProgress;
            textureRequest.OnFinishedResult += (texture) => Resolve(new MapBackground(texture));
        }
    }
}