﻿using System;
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

        public void SetClan(string clan, Action<ApiResponse> callback)
        {
            var request = new RestRequest(ApiConfiguration.SetClanEndpoint, Method.POST);
            request.AddUrlSegment("apikey", ApiKey);
            request.AddParameter("clan", clan);
            client.ExecuteAsync(request, (response) =>
                {
                    var result = ApiResponse.BuildResponse<ApiResponse>(response);
                    callback(result);
                });
        }

        public void GetRulesets(Action<RulesetListResponse> callback)
        {
            var request = new RestRequest(ApiConfiguration.RulesetListEndpoint, Method.GET);
            client.ExecuteAsync(request, (response) =>
                {
                    var result = RulesetListResponse.BuildResponse(response);
                    callback(result);
                });
        }

        public void GetMissionList(Action<MissionListResponse> callback)
        {
            var request = new RestRequest(ApiConfiguration.MissionListEndpoint, Method.GET);
            request.AddUrlSegment("apikey", ApiKey);
            client.ExecuteAsync(request, (response) =>
                {
                    var result = MissionListResponse.BuildResponse(response);
                    callback(result);
                });
        }

        public void RegisterInfection(string humanId, string zombieId, Action<RegisterInfectionResponse> callback, double? latitude = null, double? longitude = null)
        {
            var request = new RestRequest(ApiConfiguration.RegisterInfectionEndpoint, Method.POST);
            request.AddUrlSegment("apikey", ApiKey);
            request.AddParameter("human", humanId);
            request.AddParameter("zombie", zombieId);

            if (latitude.HasValue && longitude.HasValue)
            {
                request.AddParameter("latitude", latitude.Value);
                request.AddParameter("longitude", longitude.Value);
            }

            client.ExecuteAsync(request, (response) =>
                {
                    var result = RegisterInfectionResponse.BuildResponse(response);
                    callback(result);
                });
        }

        public void RegisterAntivirus(string antivirus, string zombieId, Action<AntivirusResponse> callback)
        {
            var request = new RestRequest(ApiConfiguration.AntivirusEndpoint, Method.POST);
            request.AddUrlSegment("apikey", ApiKey);
            request.AddParameter("antivirus", antivirus);
            request.AddParameter("zombie", zombieId);

            client.ExecuteAsync(request, (response) =>
                {
                    var result = AntivirusResponse.BuildResponse(response);
                    callback(result);
                });
        }

        public void TestApiKey(Action<ApiResponse> callback)
        {
            var request = new RestRequest(ApiConfiguration.TestApiKeyEndpoint, Method.GET);
            request.AddUrlSegment("apikey", ApiKey);

            client.ExecuteAsync(request, response =>
            {
                var result = ApiResponse.BuildResponse<ApiResponse>(response);
                callback(result);
            });
        }
    }
}

