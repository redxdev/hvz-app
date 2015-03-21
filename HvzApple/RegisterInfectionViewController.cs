using Foundation;
using System;
using System.CodeDom.Compiler;
using CoreLocation;
using UIKit;

using Hvz.Api;
using Hvz.Api.Game;
using Hvz.Api.Response;

namespace Hvz
{
	partial class RegisterInfectionViewController : UIViewController
	{
	    private bool loading = false;

	    private CLLocationManager locMgr = null;

	    private CLLocation lastLocation = null;

		public RegisterInfectionViewController (IntPtr handle) : base (handle)
		{
		}

	    public override void ViewDidAppear(bool animated)
	    {
	        base.ViewDidAppear(animated);

	        if (CLLocationManager.LocationServicesEnabled)
            {
                if (locMgr == null)
                {
                    locMgr = new CLLocationManager();
                    locMgr.AuthorizationChanged += AuthorizationChanged;

                    if (CLLocationManager.Status == CLAuthorizationStatus.NotDetermined)
                    {
                        locMgr.RequestWhenInUseAuthorization();
                    }
                }
	        }
	        else
	        {
	            LocationLabel.Text = "Location: NOT SENDING";
	        }

	        if (HvzClient.Instance.ApiKey.Length != 32)
	        {
	            var av = new UIAlertView("Error", "You don't have a valid api key set. Head to the settings page!", null,
	                "OK", null);
                av.Show();
	        }

            NavigationController.SetToolbarHidden(true, true);
	    }

	    private void AuthorizationChanged(object sender, CLAuthorizationChangedEventArgs e)
	    {
	        StartLocationUpdates();
	    }

	    private void StartLocationUpdates()
        {
            locMgr.DesiredAccuracy = 1;
            locMgr.LocationsUpdated += (sender, e) =>
            {
                lastLocation = e.Locations[e.Locations.Length - 1];
                LocationLabel.Text = string.Format("Location: SENDING (accuracy: {0} meters)",
                    lastLocation.HorizontalAccuracy);
            };

            locMgr.StartUpdatingLocation();
	    }

	    public override void ViewDidDisappear(bool animated)
	    {
	        base.ViewDidDisappear(animated);

	        if (locMgr != null)
	        {
	            locMgr.StopUpdatingLocation();
	        }
	    }

	    partial void OnSubmitButtonPressed(UIButton sender)
	    {
	        ResignFirstResponder();

	        if (loading)
	            return;

	        if (HvzClient.Instance.ApiKey.Length != 32)
	        {
                var av = new UIAlertView("Error", "You don't have a valid api key set. Head to the settings page!", null,
                    "OK", null);
                av.Show();
	        }
	        else
	        {
	            string humanId = HumanIdText.Text;
	            string zombieId = ZombieIdText.Text;

	            if (humanId.Length != 8)
	            {
	                var humanErr = new UIAlertView("Error", "Invalid human id (must be 8 characters long)", null, "OK",
	                    null);
                    humanErr.Show();
	                return;
	            }

	            if (zombieId.Length != 8)
	            {
	                var zombieErr = new UIAlertView("Error", "Invalid zombie id (must be 8 characters long)", null, "OK",
	                    null);
                    zombieErr.Show();
	                return;
	            }

	            double? latitude = null;
	            double? longitude = null;

	            if (lastLocation != null)
	            {
	                latitude = lastLocation.Coordinate.Latitude;
	                longitude = lastLocation.Coordinate.Longitude;
	            }

	            loading = true;

                HvzClient.Instance.RegisterInfection(humanId, zombieId, response =>
                {
                    InvokeOnMainThread(() =>
                    {
                        if (!this.IsViewLoaded || View.Window == null)
                        {
                            loading = false;
                            return;
                        }

                        switch (response.Status)
                        {
                            case ApiResponse.ResponseStatus.Ok:
                                HumanIdText.Text = string.Empty;
                                var successMsg = new UIAlertView("Success",
                                    string.Format("{0} has joined the horde, courtesy of {1}", response.HumanName,
                                        response.ZombieName), null, "OK", null);
                                successMsg.Show();
                                break;

                            case ApiResponse.ResponseStatus.Error:
                                var infectErr = new UIAlertView("Error",
                                    "Unable to register the infection. Make sure both ids are correct", null, "OK", null);
                                infectErr.Show();
                                break;
                        }
                    });
                }, latitude, longitude);
	        }
	    }
	}
}
