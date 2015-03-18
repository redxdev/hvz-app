using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using Newtonsoft.Json.Linq;
using Hvz.Api.Game;

namespace Hvz.Api.Response
{
    public class RulesetListResponse : ApiResponse
    {
        public static RulesetListResponse BuildResponse(IRestResponse response)
        {
            var result = ApiResponse.BuildResponse<RulesetListResponse>(response);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var json = JObject.Parse(response.Content);

                JArray players = (JArray)json["rulesets"];
                foreach (JObject obj in players)
                {
                    result.Rulesets.Add(Ruleset.BuildFromJson(obj));
                }
            }

            return result;
        }

        public RulesetListResponse()
            : base()
        {
            Rulesets = new List<Ruleset>();
        }

        public List<Ruleset> Rulesets { get; set; }
    }
}

