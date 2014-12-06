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

            Api.HvzClient client = new Hvz.Api.HvzClient();

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
            client.GetPlayerInfo(4, (response) =>
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
		}
	}
}


