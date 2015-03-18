using System;
using Newtonsoft.Json.Linq;

namespace Hvz.Api.Game
{
    public class Infection
    {
        public static Infection BuildFromJson(JObject json)
        {
            Infection i = new Infection();

            i.Id = (int)json["id"];
            i.HumanName = (string)json["human"];
            i.HumanId = (int)json["human_id"];
            i.ZombieName = (string)json["zombie"];
            i.ZombieId = (int)json["zombie_id"];
            i.Time = TimeUtils.UnixTimeStampToDateTime((int)json["time"]);
            if (json["latitude"].Type != JTokenType.Null && json["longitude"].Type != JTokenType.Null)
            {
                i.Latitude = (double)json["latitude"];
                i.Longitude = (double)json["longitude"];
            }

            return i;
        }

        public int Id { get; set; }

        public string HumanName { get; set; }

        public int HumanId { get; set; }

        public string ZombieName { get; set; }

        public int ZombieId { get; set; }

        public DateTime Time { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}

