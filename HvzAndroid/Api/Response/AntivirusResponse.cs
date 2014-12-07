using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace Hvz.Api.Response
{
    public class AntivirusResponse : ApiResponse
    {
        public static AntivirusResponse BuildResponse(IRestResponse response)
        {
            var result = ApiResponse.BuildResponse<AntivirusResponse>(response);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var json = JObject.Parse(response.Content);

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

        public string ZombieName { get; set; }

        public int ZombieId { get; set; }
    }
}

