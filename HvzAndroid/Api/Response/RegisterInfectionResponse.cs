using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace Hvz.Api.Response
{
    public class RegisterInfectionResponse : ApiResponse
    {
        public static RegisterInfectionResponse BuildResponse(IRestResponse response)
        {
            var result = ApiResponse.BuildResponse<RegisterInfectionResponse>(response);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var json = JObject.Parse(response.Content);

                result.HumanName = (string)json["human_name"];
                result.HumanId = (int)json["human_id"];
                result.ZombieName = (string)json["zombie_name"];
                result.ZombieId = (int)json["zombie_id"];
            }
            else if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                var json = JObject.Parse(response.Content);

                JArray errors = (JArray)json["errors"];
                foreach (JObject obj in errors)
                {
                    result.Errors.Add((string)obj);
                }
            }

            return result;
        }

        public string HumanName { get; set; }

        public int HumanId { get; set; }

        public string ZombieName { get; set; }

        public int ZombieId { get; set; }
    }
}

