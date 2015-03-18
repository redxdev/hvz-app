using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using Newtonsoft.Json.Linq;
using Hvz.Api.Game;

namespace Hvz.Api.Response
{
    public class MissionListResponse : ApiResponse
    {
        public static MissionListResponse BuildResponse(IRestResponse response)
        {
            var result = ApiResponse.BuildResponse<MissionListResponse>(response);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var json = JObject.Parse(response.Content);

                JArray missions = (JArray)json["missions"];
                foreach (JObject obj in missions)
                {
                    result.Missions.Add(Mission.BuildFromJson(obj));
                }
            }
            else if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                var json = JObject.Parse(response.Content);

                JArray errors = (JArray)json["errors"];
                foreach (JValue obj in errors)
                {
                    result.Errors.Add((string)obj);
                }
            }

            return result;
        }

        public MissionListResponse()
            : base()
        {
            Missions = new List<Mission>();
        }

        public List<Mission> Missions { get; set; }
    }
}

