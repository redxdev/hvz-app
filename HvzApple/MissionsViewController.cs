using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

using Hvz.Api;
using Hvz.Api.Game;

namespace Hvz
{
	partial class MissionsViewController : UIViewController
	{
		public MissionsViewController (IntPtr handle) : base (handle)
		{
		}

	    public override void ViewDidAppear(bool animated)
	    {
	        base.ViewDidAppear(animated);

	        if (HvzClient.Instance.ApiKey.Length != 32)
	        {
	            var av = new UIAlertView("Error", "You don't have a valid api key set. Head to the settings page!", null,
	                "OK", null);
                av.Show();
	        }
	        else
	        {
	            Refresh();
	        }

            NavigationController.SetToolbarHidden(true, true);
	    }

	    private void Refresh()
	    {
	        HvzClient.Instance.GetMissionList(response =>
	        {
                // Build an HTML page from the response

	            string html = string.Empty;

	            foreach (Mission mission in response.Missions)
	            {
	                html += string.Format(
                        "<h1>{0}</h1>{1}",
                        mission.Title,
                        mission.Body
	                    );
	            }

                InvokeOnMainThread(() =>
                {
                    WebView.LoadHtmlString(html, null);
                });
	        });
	    }
	}
}
