using System;

namespace Hvz.Api
{
    public static class GameUtils
    {
        public enum Team
        {
            Human,
            Zombie
        }

        public const string SortByTeam = "team";
        public const string SortByClan = "clan";
        public const string SortByMods = "mods";
    }
}

