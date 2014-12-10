using System;

namespace Hvz.Api
{
	public static class ApiConfiguration
	{
        public const string BaseUrl = "http://notbatman.student.rit.edu/HVZ/web";
        public const string ApiUrl = "/app.php/api/v1";
        public const string BadgeUrl = "/assets/images/badges";

        public const string GameStatusEndpoint = "/status";
        public const string TeamStatusEndpoint = "/status/teams";
        public const string PlayerInfoEndpoint = "/player/{id}";
        public const string PlayerListEndpoint = "/players/{page}?maxPerPage={maxPerPage}&sort={sort}";
        public const string PlayerSearchEndpoint = "/players/search?term={term}";
        public const string InfectionListEndpoint = "/infections/{page}?maxPerPage={maxPerPage}";

        public const string ProfileEndpoint = "/profile?apikey={apikey}";
        public const string SetClanEndpoint = "/profile/clan?apikey={apikey}";

        public const string RulesetListEndpoint = "/rules";
        public const string MissionListEndpoint = "/missions?apikey={apikey}";

        public const string RegisterInfectionEndpoint = "/register_infection?apikey={apikey}";
        public const string AntivirusEndpoint = "/antivirus?apikey={apikey}";
	}
}