using System;
using System.Net;
using System.Threading.Tasks;
using Hvz.Api.Response;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace Hvz.Api
{
    public class HvzClient
    {
        private RestClient client = null;

        public HvzClient()
        {
            this.client = new RestClient(ApiConfiguration.ApiUri);
        }

        public string ApiKey { get; set; }

        public void GetGameStatus(Action<GameStatusResponse> callback)
        {
            var request = new RestRequest(ApiConfiguration.GameStatusEndpoint, Method.GET);
            client.ExecuteAsync(request, (response) => {
                var result = GameStatusResponse.BuildResponse(response);
                callback(result);
            });
        }

        public void GetTeamStatus(Action<TeamStatusResponse> callback)
        {
            var request = new RestRequest(ApiConfiguration.TeamStatusEndpoint, Method.GET);
            client.ExecuteAsync(request, (response) =>
                {
                    var result = TeamStatusResponse.BuildResponse(response);
                    callback(result);
                });
        }
    }
}

