using System;
using Newtonsoft.Json.Linq;

namespace Hvz.Api.Game
{
    public class Mission
    {
        public static Mission BuildFromJson(JObject json)
        {
            var m = new Mission();

            m.Id = (int)json["id"];
            m.Title = (string)json["title"];
            m.Body = (string)json["body"];

            string team = ((string)json["team"]).ToLower();
            switch (team)
            {
                case "human":
                    m.Team = GameUtils.Team.Human;
                    break;

                case "zombie":
                    m.Team = GameUtils.Team.Zombie;
                    break;
            }

            m.PostDate = TimeUtils.UnixTimeStampToDateTime((int)json["post_date"]);

            return m;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public GameUtils.Team Team { get; set; }

        public DateTime PostDate { get; set; }
    }
}

