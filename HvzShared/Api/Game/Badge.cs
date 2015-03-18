using System;
using Newtonsoft.Json.Linq;

namespace Hvz.Api.Game
{
    public class Badge
    {
        public static Badge BuildFromJson(JObject json)
        {
            Badge b = new Badge();

            b.Id = (string)json["id"];
            b.Name = (string)json["name"];
            b.Image = (string)json["image"];
            b.Description = (string)json["description"];
            b.Access = (string)json["access"];

            return b;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public string Access { get; set; }
    }
}

