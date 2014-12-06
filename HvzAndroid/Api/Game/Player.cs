using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Hvz.Api.Game
{
    public class Player
    {
        public static Player BuildFromJson(JObject json, Player build = null)
        {
            Player p = build ?? new Player();
            p.Id = (int)json["id"];
            p.FullName = (string)json["fullname"];

            string team = ((string)json["team"]).ToLower();
            switch (team)
            {
                case "human":
                    p.Team = GameUtils.Team.Human;
                    break;

                case "zombie":
                    p.Team = GameUtils.Team.Zombie;
                    break;
            }

            p.HumansTagged = (int)json["humansTagged"];
            p.Clan = (string)json["clan"];
            p.Avatar = (string)json["avatar"];

            JArray badges = (JArray)json["badges"];
            foreach (JObject obj in badges)
            {
                p.Badges.Add(Badge.BuildFromJson(obj));
            }

            return p;
        }

        public Player()
        {
            this.Badges = new List<Badge>();
        }

        public int Id { get; set; }

        public string FullName { get; set; }

        public GameUtils.Team Team { get; set; }

        public int HumansTagged { get; set; }

        public string Clan { get; set; }

        public List<Badge> Badges { get; set; }

        public string Avatar { get; set; }
    }
}

