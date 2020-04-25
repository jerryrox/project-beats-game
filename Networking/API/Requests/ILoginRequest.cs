using PBGame.Networking.API.Responses;

namespace PBGame.Networking.API.Requests
{
    public interface ILoginRequest : IApiRequest<ILoginResponse> {
    
        /// <summary>
        /// Username of the login user.
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// Password of the login user.
        /// </summary>
        string Password { get; set; }
    }
}