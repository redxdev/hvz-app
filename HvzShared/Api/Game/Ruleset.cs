using System;
using Newtonsoft.Json.Linq;

namespace Hvz.Api.Game
{
    public class Ruleset
    {
        public static Ruleset BuildFromJson(JObject json)
        {
            var r = new Ruleset();

            r.Id = (int)json["id"];
            r.Title = (string)json["title"];
            r.Body = (string)json["body"];

            return r;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }
}

