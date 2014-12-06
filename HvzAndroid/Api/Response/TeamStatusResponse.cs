using System;
using System.Net;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace Hvz.Api.Response
{
    public class TeamStatusResponse : ApiResponse
    {
        public static TeamStatusResponse BuildResponse(IRestResponse response)
        {
            var result = ApiResponse.BuildResponse<TeamStatusResponse>(response);
            if(result.StatusCode == HttpStatusCode.OK)
            {
                var json = JObject.Parse(response.Content);

                result.HumanCount = (int)json["humans"];
                result.ZombieCount = (int)json["zombies"];
            }

            return result;
        }

        public int HumanCount { get; set; }

        public int ZombieCount { get; set; }
    }
}

