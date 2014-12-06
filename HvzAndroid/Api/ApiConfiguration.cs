using System;

namespace Hvz.Api
{
	public static class ApiConfiguration
	{
        public const string BaseUrl = "http://notbatman.student.rit.edu/HVZ/web";

        public const string ApiUrl = "/app.php/api/v1";

        public const string GameStatusEndpoint = "/status";
        public const string TeamStatusEndpoint = "/status/teams";
        public const string PlayerInfoEndpoint = "/player/{0}";
	}
}

