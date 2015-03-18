using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using Newtonsoft.Json.Linq;
using Hvz.Api.Game;

namespace Hvz.Api.Response
{
    public class InfectionListResponse : ApiResponse
    {
        public static InfectionListResponse BuildResponse(IRestResponse response)
        {
            var result = ApiResponse.BuildResponse<InfectionListResponse>(response);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var json = JObject.Parse(response.Content);

                result.HasMorePages = (bool)json["continues"];

                JArray players = (JArray)json["infections"];
                foreach (JObject obj in players)
                {
                    result.Infections.Add(Infection.BuildFromJson(obj));
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

        public InfectionListResponse()
            : base()
        {
            Infections = new List<Infection>();
        }

        public bool HasMorePages { get; set; }

        public List<Infection> Infections { get; set; }
    }
}

