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
            this.client = new RestClient(ApiConfiguration.BaseUrl + ApiConfiguration.ApiUrl);
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

        public void GetPlayerInfo(int id, Action<PlayerInfoResponse> callback)
        {
            var request = new RestRequest(ApiConfiguration.PlayerInfoEndpoint, Method.GET);
            request.AddUrlSegment("id", id.ToString());
            client.ExecuteAsync(request, (response) =>
                {
                    var result = PlayerInfoResponse.BuildResponse(response);
                    callback(result);
                });
        }

        public void GetPlayerList(int page, Action<PlayerListResponse> callback, int maxPerPage = 10, string sort = GameUtils.SortByTeam)
        {
            var request = new RestRequest(ApiConfiguration.PlayerListEndpoint, Method.GET);
            request.AddUrlSegment("page", page.ToString());
            request.AddUrlSegment("maxPerPage", maxPerPage.ToString());
            request.AddUrlSegment("sort", sort);
            client.ExecuteAsync(request, (response) =>
                {
                    var result = PlayerListResponse.BuildResponse(response);
                    callback(result);
                });
        }

        public void SearchPlayerList(string term, Action<PlayerListResponse> callback)
        {
            var request = new RestRequest(ApiConfiguration.PlayerSearchEndpoint, Method.GET);
            request.AddUrlSegment("term", term);
            client.ExecuteAsync(request, (response) =>
                {
                    var result = PlayerListResponse.BuildResponse(response);
                    callback(result);
                });
        }

        public void GetInfectionList(int page, Action<InfectionListResponse> callback, int maxPerPage = 10)
        {
            var request = new RestRequest(ApiConfiguration.InfectionListEndpoint, Method.GET);
            request.AddUrlSegment("page", page.ToString());
            request.AddUrlSegment("maxPerPage", maxPerPage.ToString());
            client.ExecuteAsync(request, (response) =>
                {
                    var result = InfectionListResponse.BuildResponse(response);
                    callback(result);
                });
        }

        public void GetProfile(Action<ProfileResponse> callback)
        {
            var request = new RestRequest(ApiConfiguration.ProfileEndpoint, Method.GET);
            request.AddUrlSegment("apikey", ApiKey);
            client.ExecuteAsync(request, (response) =>
                {
                    var result = ProfileResponse.BuildResponse(response);
                    callback(result);
                });
        }
    }
}

