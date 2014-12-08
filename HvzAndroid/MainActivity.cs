using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Hvz.Api;

namespace Hvz
{
	[Activity (Label = "Humans vs Zombies @ RIT", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
        private HvzClient client = null;

        private TextView humanCount = null;
        private TextView zombieCount = null;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            humanCount = FindViewById<TextView>(Resource.Id.humanCount);
            zombieCount = FindViewById<TextView>(Resource.Id.zombieCount);

            client = new HvzClient() { ApiKey = ApiConfiguration.DevApiKey };

            client.GetTeamStatus((response) =>
                {
                    if(response.Status == Hvz.Api.Response.ApiResponse.ResponseStatus.Ok)
                    {
                        RunOnUiThread(() => {
                            humanCount.Text = response.HumanCount.ToString();
                            zombieCount.Text = response.ZombieCount.ToString();
                        });
                    }
                });
		}
	}
}


