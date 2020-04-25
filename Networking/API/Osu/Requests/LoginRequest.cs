using PBGame.Networking.API.Osu.Responses;
using PBGame.Networking.API.Requests;
using PBGame.Networking.API.Responses;
using PBFramework.Networking.API;

namespace PBGame.Networking.API.Osu.Requests
{
    public class LoginRequest : BaseRequest<ILoginResponse>, ILoginRequest {

        private FormPostData form = new FormPostData();


        public string Username { get; set; }

        public string Password { get; set; }


        protected override IHttpRequest CreateRequest()
        {
            form.AddField("username", Username);
            form.AddField("password", Password);

            var request = new HttpPostRequest(Api.GetUrl("session"), retryCount: 0);
            request.SetPostData(form);
            return request;
        }

        protected override ILoginResponse CreateResponse(IHttpRequest request) => new LoginResponse(request);
    }
}