using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

using Hvz.Api;
using Hvz.Api.Response;

namespace Hvz
{
	partial class StatusViewController : UIViewController
	{
		public StatusViewController (IntPtr handle) : base (handle)
		{
		}

	    public override void ViewDidAppear(bool animated)
	    {
            base.ViewDidAppear(animated);

            HvzClient.Instance.GetTeamStatus((response) =>
            {
                InvokeOnMainThread(() =>
                {
                    if (!this.IsViewLoaded || View.Window == null)
                        return;

                    switch (response.Status)
                    {
                        case ApiResponse.ResponseStatus.Ok:
                            HumanCount.Text = response.HumanCount.ToString();
                            ZombieCount.Text = response.ZombieCount.ToString();
                            break;

                        case ApiResponse.ResponseStatus.Error:
                            HumanCount.Text = "Err";
                            ZombieCount.Text = "Err";

                            var av = new UIAlertView("Error", "There was a problem retreiving team status", null, "OK", null);
                            av.Show();
                            break;
                    }
                });
            });

            HvzClient.Instance.GetGameStatus((response) =>
            {
                InvokeOnMainThread(() =>
                {
                    if (!this.IsViewLoaded || View.Window == null)
                        return;

                    switch (response.Status)
                    {
                        case ApiResponse.ResponseStatus.Ok:
                            if (response.Game != GameStatusResponse.GameStatus.None)
                            {
                                DaysCount.Text = response.Days.ToString() + " DAYS";
                                HoursCount.Text = response.Hours.ToString() + " HOURS";
                                MinutesCount.Text = response.Minutes.ToString() + " MINUTES";
                                SecondsCount.Text = response.Seconds.ToString() + " SECONDS";
                            }
                            else
                            {
                                DaysCount.Text = "- DAYS";
                                HoursCount.Text = "- HOURS";
                                MinutesCount.Text = "- MINUTES";
                                SecondsCount.Text = "- SECONDS";
                            }
                            break;

                        case ApiResponse.ResponseStatus.Error:
                            DaysCount.Text = "? DAYS";
                            HoursCount.Text = "? HOURS";
                            MinutesCount.Text = "? MINUTES";
                            SecondsCount.Text = "? SECONDS";

                            var av = new UIAlertView("Error", "There was a problem retrieving game status", null, "OK",
                                null);
                            av.Show();
                            break;
                    }
                });
            });

            NavigationController.SetToolbarHidden(true, true);
	    }
	}
}
