using PBGame.Networking.API.Osu.Responses;
using PBFramework.Networking.API;

namespace PBGame.Networking.API.Osu.Requests
{
    public class LoginRequest : BaseRequest<LoginResponse> {

        private FormPostData form = new FormPostData();


        public LoginRequest(string username, string password)
        {
            form.AddField("username", username);
            form.AddField("password", password);
        }

        protected override IHttpRequest CreateRequest()
        {
            var request = new HttpPostRequest(Api.GetUrl("session"), retryCount: 0);
            request.SetPostData(form);
            return request;
        }

        protected override LoginResponse CreateResponse(IHttpRequest request) => new LoginResponse(request);
    }
}