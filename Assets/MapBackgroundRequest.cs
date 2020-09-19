using PBGame.Maps;
using PBFramework.Networking;

namespace PBGame.Assets
{
    public class MapBackgroundRequest : WrappedWebRequest<TextureRequest, IMapBackground> {

        public MapBackgroundRequest(string url) : base(new TextureRequest(url, false))
        {
        }

        protected override IMapBackground GetOutput(TextureRequest request) => new MapBackground(request.Output);
    }
}