using System;
using PBGame.Networking.API.Responses;
using PBFramework.Debugging;
using PBFramework.Networking.API;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PBGame.Networking.API.Osu.Responses
{
    public class LoginResponse : BaseResponse, ILoginResponse {
    
        public LoginResponse(IHttpRequest request) : base(request) {}

        public override void Evaluate()
        {
            IsSuccess = request.Response.Code == 200;

            // Try parsing error message passed from osu as json.
            try
            {
                var json = JsonConvert.DeserializeObject<JObject>(request.Response.TextData);
                ErrorMessage = json["error"].ToString();
            }
            catch (Exception)
            {
                ErrorMessage = request.Response.ErrorMessage;
            }
        }

        public override void ApplyResponse(IApi api)
        {
            try
            {
                var json = JsonConvert.DeserializeObject<JObject>(request.Response.TextData);
                {
                    var user = json["user"].ToObject<JObject>();
                    {
                        var onlineUser = new OsuUser()
                        {
                            Api = api,
                            Id = user["id"].ToString(),
                            Username = user["username"].ToString(),
                            AvatarImage = user["avatar_url"].ToString(),
                            CoverImage = user["cover_url"].ToString(),
                            Status = user["interests"].ToString(),
                            ProfilePage = $"https://osu.ppy.sh/users/{user["id"].ToString()}",
                        };
                        api.User.Value = onlineUser;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError($"LoginResponse.ApplyResponse - Error while parsing user data from response: {e.Message}\n{e.StackTrace}");
            }
        }
    }
}