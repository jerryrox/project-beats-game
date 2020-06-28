using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Services;
using PBFramework.Threading;
using PBFramework.Networking;
using PBFramework.Networking.API;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PBGame.Networking.API.Responses
{
    public class ApiResponse : IApiResponse {

        protected IHttpRequest request;


        public bool IsSuccess { get; protected set; }

        public string ErrorMessage { get; protected set; }


        public ApiResponse(IHttpRequest request)
        {
            if(request == null)
                throw new ArgumentNullException(nameof(request));

            this.request = request;
        }

        public Task Evaluate(IEventProgress progress = null)
        {
            var response = request.Response;
            if(response == null)
                throw new Exception("There is no response to evaluate!");

            return Task.Run(() =>
            {
                if (response.IsSuccess)
                    ParseResponse(response, progress);
                else
                {
                    IsSuccess = false;
                    ErrorMessage = response.ErrorMessage;
                }

                UnityThreadService.DispatchUnattended(() =>
                {
                    progress?.Report(1f);
                    progress?.InvokeFinished();
                    return null;
                });
            });
        }

        /// <summary>
        /// Parses the raw response data.
        /// </summary>
        protected virtual void ParseResponse(IWebResponse response, IEventProgress progress)
        {
            JObject json = JsonConvert.DeserializeObject<JObject>(response.TextData);
            if (json.ContainsKey("type"))
            {
                if (json["type"].ToString().Equals("Error", StringComparison.OrdinalIgnoreCase))
                {
                    IsSuccess = false;
                    ErrorMessage = json["message"].ToString() ?? "Response error.";
                }
                else
                {
                    IsSuccess = true;
                    try
                    {
                        ParseResponseData(json["data"], progress);
                    }
                    catch (Exception e)
                    {
                        IsSuccess = false;
                        ErrorMessage = e.Message;
                        // TODO: Output error to logger.
                    }
                }
            }
            else
            {
                IsSuccess = false;
                ErrorMessage = "Unknown response format detected.";
                // TODO: Output response text to logger.
            }
        }

        /// <summary>
        /// Parses the specified response data into the format appropriate for this response.
        /// </summary>
        protected virtual void ParseResponseData(JToken responseData, IEventProgress progress) { }
    }
}