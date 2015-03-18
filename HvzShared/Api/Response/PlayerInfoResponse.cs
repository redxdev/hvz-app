using System;
using System.Net;
using RestSharp;
using Newtonsoft.Json.Linq;
using Hvz.Api.Game;

namespace Hvz.Api.Response
{
    public class PlayerInfoResponse : ApiResponse
    {
        public static PlayerInfoResponse BuildResponse(IRestResponse response)
        {
            var result = ApiResponse.BuildResponse<PlayerInfoResponse>(response);
            if(result.StatusCode == HttpStatusCode.OK)
            {
                var json = JObject.Parse(response.Content);

                result.Player = Player.BuildFromJson(json);
            }

            return result;
        }

        public Player Player { get; set; }
    }
}

