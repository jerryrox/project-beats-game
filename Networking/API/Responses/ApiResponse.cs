using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Services;
using PBFramework.Debugging;
using PBFramework.Networking;
using PBFramework.Networking.API;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PBGame.Networking.API.Responses
{
    public class ApiResponse : IApiResponse {

        public event Action OnEvaluated;

        protected IWebResponse response;


        public bool IsSuccess { get; protected set; }

        public string ErrorMessage { get; protected set; }


        public ApiResponse(IWebResponse response)
        {
            if(response == null)
                throw new ArgumentNullException(nameof(response));

            this.response = response;
        }

        public void Evaluate()
        {
            if(response == null)
                throw new Exception("There is no response to evaluate!");

            try
            {
                if (response.IsSuccess)
                {
                    ParseResponse();
                }
                else
                {
                    EvaluateFail(response.ErrorMessage);
                }
            }
            catch (Exception e)
            {
                EvaluateFail(e.Message);
                Logger.LogError(e.ToString());
            }
        }

        /// <summary>
        /// Parses the raw response data.
        /// </summary>
        protected virtual void ParseResponse()
        {
            JObject json = UnityThreadService.Dispatch(() => JsonConvert.DeserializeObject<JObject>(response.TextData)) as JObject;
            if (json.ContainsKey("type"))
            {
                if (json["type"].ToString().Equals("Error", StringComparison.OrdinalIgnoreCase))
                {
                    EvaluateFail(json["message"].ToString() ?? "Response error.");
                }
                else
                {
                    ParseResponseData(json["data"]);
                }
            }
            else
            {
                EvaluateFail("Unknown response format detected.");
                // TODO: Output response text to logger.
            }
        }

        /// <summary>
        /// Parses the specified response data into the format appropriate for this response.
        /// </summary>
        protected virtual void ParseResponseData(JToken responseData) { }

        /// <summary>
        /// Evaluates a failed response and fires event.
        /// </summary>
        protected void EvaluateFail(string message)
        {
            IsSuccess = false;
            ErrorMessage = message;
            OnEvaluated?.Invoke();
        }

        /// <summary>
        /// Evaluates a success response and fires event.
        /// </summary>
        protected void EvaluateSuccess()
        {
            IsSuccess = true;
            OnEvaluated?.Invoke();
        }
    }
}