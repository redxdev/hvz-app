using System;
using System.Net;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace Hvz.Api.Response
{
    public class GameStatusResponse : ApiResponse
    {
        public static GameStatusResponse BuildResponse(IRestResponse response)
        {
            var result = ApiResponse.BuildResponse<GameStatusResponse>(response);
            if(result.StatusCode == HttpStatusCode.OK)
            {
                var json = JObject.Parse(response.Content);

                string status = ((string)json["status"]).ToLower();
                switch(status)
                {
                    default:
                    case "no-game":
                        result.Game = GameStatus.None;
                        break;

                    case "pre-game":
                        result.Game = GameStatus.Pre;
                        break;

                    case "current-game":
                        result.Game = GameStatus.Current;
                        break;
                }

                if (result.Game != GameStatus.None)
                {
                    int start = (int)json["game"]["start"];
                    int end = (int)json["game"]["end"];

                    result.Start = TimeUtils.UnixTimeStampToDateTime(start);
                    result.End = TimeUtils.UnixTimeStampToDateTime(end);
                }
            }

            return result;
        }

        public enum GameStatus
        {
            None,
            Pre,
            Current
        }

        public GameStatus Game { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}

