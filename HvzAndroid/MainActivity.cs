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

            TextView text = FindViewById<TextView>(Resource.Id.statusText);

            Api.HvzClient client = new Hvz.Api.HvzClient();
            /*
            client.GetGameStatus((response) =>
                {
                    RunOnUiThread(() => {
                        text.Text = string.Format(
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
                });*/

            client.GetTeamStatus((response) =>
                {
                    RunOnUiThread(() => {
                        text.Text = string.Format(
                            "Humans: {0}\n" +
                            "Zombies: {1}",
                            response.HumanCount,
                            response.ZombieCount
                        );
                    });
                });
		}
	}
}


