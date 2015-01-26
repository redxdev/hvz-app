using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Hvz.Api.Game
{
    public class Profile : Player
    {
        public static Profile BuildFromJson(JObject json)
        {
            var p = (Profile)Player.BuildFromJson(json, new Profile());

            p.ApiKey = (string)json["apikey"];
            p.Email = (string)json["email"];
            p.ZombieId = (string)json["zombieId"];

            var humanids = (JArray)json["humanIds"];
            foreach (JObject hid in humanids)
            {
                p.HumanIds.Add(HumanId.BuildFromJson(hid));
            }

            var infections = (JArray)json["infections"];
            foreach (JObject inf in infections)
            {
                p.Infections.Add(Infection.BuildFromJson(inf));
            }

            p.QRData = (string)json["qr_data"];

            return p;
        }

        public Profile()
            : base()
        {
            HumanIds = new List<HumanId>();
            Infections = new List<Infection>();
        }

        public string ApiKey { get; set; }

        public string Email { get; set; }

        public string ZombieId { get; set; }

        public List<HumanId> HumanIds { get; set; }

        public List<Infection> Infections { get; set; }

        public string QRData { get; set; }
    }
}

