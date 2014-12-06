using System;
using Newtonsoft.Json.Linq;

namespace Hvz.Api.Game
{
    public class HumanId
    {
        public static HumanId BuildFromJson(JObject json)
        {
            var hid = new HumanId();

            hid.Id = (string)json["id_string"];
            hid.Active = (bool)json["active"];

            return hid;
        }

        public string Id { get; set; }

        public bool Active { get; set; }
    }
}

