using System;
using System.Net;
using System.Json;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace HvZAndroid
{
	[Activity (Label = "Humans vs Zombies @ RIT", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			TextView text = FindViewById<TextView> (Resource.Id.resultView);

			string url = "ChangeMe";
			var httpReq = (HttpWebRequest)HttpWebRequest.Create (new Uri (url));

			httpReq.BeginGetResponse ((ar) => {
				var request = (HttpWebRequest)ar.AsyncState;
				using(var response = (HttpWebResponse)request.EndGetResponse(ar)) {
					var s = response.GetResponseStream();
					var j = (JsonObject)JsonObject.Load(s);
					string status = j["status"];
					RunOnUiThread(() => {text.Text = status;});
				}
			}, httpReq);
		}
	}
}


