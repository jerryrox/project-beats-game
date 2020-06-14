using PBGame.Networking.API.Osu.Requests;
using PBGame.Networking.API.Requests;

namespace PBGame.Networking.API.Osu
{
    public class OsuRequestFactory : IRequestFactory {

        public ILoginRequest GetLogin() => new LoginRequest();

        public IMapDownloadRequest GetMapDownload() => new MapDownloadRequest();

        public IMapsetListRequest GetMapsetList() => new MapsetListRequest();
    }
}