using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using Newtonsoft.Json.Linq;
using Hvz.Api.Game;

namespace Hvz.Api.Response
{
    public class PlayerListResponse : ApiResponse
    {
        public static PlayerListResponse BuildResponse(IRestResponse response)
        {
            var result = ApiResponse.BuildResponse<PlayerListResponse>(response);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var json = JObject.Parse(response.Content);

                result.HasMorePages = (bool)json["continues"];

                JArray players = (JArray)json["players"];
                foreach (JObject obj in players)
                {
                    result.Players.Add(Player.BuildFromJson(obj));
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

        public PlayerListResponse()
            : base()
        {
            Players = new List<Player>();
        }

        public bool HasMorePages { get; set; }

        public List<Player> Players { get; set; }
    }
}

