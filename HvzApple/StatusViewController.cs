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
	    private CountdownTimer timer = null;

		public StatusViewController (IntPtr handle) : base (handle)
		{
		}

	    public override void ViewDidAppear(bool animated)
	    {
            base.ViewDidAppear(animated);

	        if (timer != null)
	        {
	            timer.Stop();
	            timer = null;
	        }

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

                                timer = new CountdownTimer();
                                timer.Callback = TimerCallback;
                                timer.Start(response.Days, response.Hours, response.Minutes, response.Seconds);
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

	    public override void ViewDidDisappear(bool animated)
	    {
	        base.ViewDidDisappear(animated);

	        if (timer != null)
	        {
	            timer.Stop();
	            timer = null;
	        }
	    }

        private void TimerCallback(int days, int hours, int minutes, int seconds)
        {
            InvokeOnMainThread(() =>
            {
                DaysCount.Text = days + " DAYS";
                HoursCount.Text = hours + " HOURS";
                MinutesCount.Text = minutes + " MINUTES";
                SecondsCount.Text = seconds + " SECONDS";
            });
        }
	}
}
