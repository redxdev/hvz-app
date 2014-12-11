using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using Newtonsoft.Json.Linq;
using Hvz.Api.Game;

namespace Hvz.Api.Response
{
    public class ProfileResponse : ApiResponse
    {
        public static ProfileResponse BuildResponse(IRestResponse response)
        {
            var result = ApiResponse.BuildResponse<ProfileResponse>(response);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var json = JObject.Parse(response.Content);

                result.Profile = Profile.BuildFromJson((JObject)json["profile"]);
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

        public Profile Profile { get; set; }
    }
}

