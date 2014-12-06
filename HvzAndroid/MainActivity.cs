using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Hvz
{
	[Activity (Label = "Humans vs Zombies @ RIT", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            Api.HvzClient client = new Hvz.Api.HvzClient() { ApiKey = Api.ApiConfiguration.DevApiKey };

			TextView statusText = FindViewById<TextView>(Resource.Id.statusText);
            client.GetGameStatus((response) =>
                {
                    RunOnUiThread(() => {
                        statusText.Text = string.Format(
                            "Response: {0}\n" +
                            "Game status: {1}\n" +
                            "Start time: {2}\n" +
                            "End time: {3}",
                            response.StatusCode,
                            response.Game,
                            response.Start,
                            response.End
                        );
                    });
                });

			TextView teamStatusText = FindViewById<TextView>(Resource.Id.teamStatusText);
			client.GetTeamStatus((response) =>
			    {
			        RunOnUiThread(() => {
			            teamStatusText.Text = string.Format(
			                "Humans: {0}\n" +
			                "Zombies: {1}",
			                response.HumanCount,
			                response.ZombieCount
			            );
			        });
			    });

            TextView playerinfoText = FindViewById<TextView>(Resource.Id.playerInfoText);
            client.GetPlayerInfo(1, (response) =>
                {
                    RunOnUiThread(() => {
                        playerinfoText.Text = string.Format(
                            "Id: {0}\n" +
                            "Name: {1}\n" +
                            "Team: {2}\n" +
                            "Tags: {3}\n" +
                            "Clan: {4}\n" +
                            "Avatar: {5}\n" +
                            "Badge Count: {6}",
                            response.Player.Id,
                            response.Player.FullName,
                            response.Player.Team,
                            response.Player.HumansTagged,
                            response.Player.Clan,
                            response.Player.Avatar,
                            response.Player.Badges.Count
                        );
                    });
                });

            TextView playerListText = FindViewById<TextView>(Resource.Id.playerListText);
            client.GetPlayerList(0, (response) =>
                {
                    RunOnUiThread(() => {
                        playerListText.Text = string.Format(
                            "Players on this page: {0}\n" +
                            "Has more pages: {1}",
                            response.Players.Count,
                            response.HasMorePages
                        );
                    });
                });

            TextView playerSearchText = FindViewById<TextView>(Resource.Id.playerSearchText);
            client.SearchPlayerList("sam", (response) =>
                {
                    RunOnUiThread(() => {
                        playerSearchText.Text = string.Format(
                            "Players on this page: {0}\n" +
                            "Has more pages: {1}",
                            response.Players.Count,
                            response.HasMorePages
                        );
                    });
                });

            TextView infectionListText = FindViewById<TextView>(Resource.Id.infectionListText);
            client.GetInfectionList(0, (response) =>
                {
                    RunOnUiThread(() => {
                        infectionListText.Text = string.Format(
                            "Infections on this page: {0}\n" +
                            "Has more pages: {1}",
                            response.Infections.Count,
                            response.HasMorePages
                        );
                    });
                });

            TextView profileText = FindViewById<TextView>(Resource.Id.profileText);
            client.GetProfile((response) =>
                {
                    RunOnUiThread(() => {
                        profileText.Text = string.Format(
                            "Id: {0}\n" +
                            "Name: {1}\n" +
                            "Team: {2}\n" +
                            "Tags: {3}\n" +
                            "Clan: {4}\n" +
                            "Avatar: {5}\n" +
                            "Badge Count: {6}\n" +
                            "Api Key: {7}\n" +
                            "Email: {8}\n" +
                            "Zombie Id: {9}\n" +
                            "Human Id Count: {10}\n" +
                            "Infection Count: {11}",
                            response.Profile.Id,
                            response.Profile.FullName,
                            response.Profile.Team,
                            response.Profile.HumansTagged,
                            response.Profile.Clan,
                            response.Profile.Avatar,
                            response.Profile.Badges.Count,
                            response.Profile.ApiKey,
                            response.Profile.Email,
                            response.Profile.ZombieId,
                            response.Profile.HumanIds.Count,
                            response.Profile.Infections.Count
                        );
                    });
                });

            TextView clanText = FindViewById<TextView>(Resource.Id.clanText);
            client.SetClan("API Clan!", (response) =>
                {
                    RunOnUiThread(() => {
                        clanText.Text = string.Format(
                            "Clan result: {0}",
                            response.Status
                        );
                    });
                });
		}
	}
}


