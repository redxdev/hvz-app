using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using UIKit;

using Hvz.Api;
using Hvz.Api.Response;
using Xamarin;

namespace Hvz
{
	partial class SettingsViewController : UIViewController
	{
		public SettingsViewController (IntPtr handle) : base (handle)
		{
		}

	    public override void ViewDidAppear(bool animated)
	    {
	        base.ViewDidAppear(animated);

            string apikey = NSUserDefaults.StandardUserDefaults.StringForKey("apikey") ?? string.Empty;
	        HvzClient.Instance.ApiKey = apikey;

	        ApiTextField.Text = apikey;

            NavigationController.SetToolbarHidden(true, true);
	    }

	    partial void OnScanButtonPressed(UIButton sender)
	    {
            InvokeOnMainThread(async () =>
            {
                string result = await HvzClient.Instance.ScanQRApiKey();
                if (result != null)
                    ApiTextField.Text = result;
            });
	    }

	    partial void OnSaveButtonPressed(UIButton sender)
	    {
	        ResignFirstResponder();
	        Save();
	    }

	    private void Save()
	    {
	        string apikey = ApiTextField.Text.Trim();
	        if (apikey.Length != 32)
	        {
	            var av = new UIAlertView("Error", "Api key must be 32 characters long", null, "OK", null);
                av.Show();
	            return;
	        }

            NSUserDefaults.StandardUserDefaults.SetString(apikey, "apikey");
	        HvzClient.Instance.ApiKey = apikey;

            HvzClient.Instance.TestApiKey(response =>
            {
                InvokeOnMainThread(() =>
                {
                    switch (response.Status)
                    {
                        case ApiResponse.ResponseStatus.Ok:
                            var successAv = new UIAlertView("Success", "Your api key has been saved!", null, "OK", null);
                            successAv.Show();

                            HvzClient.Instance.GetProfile(r =>
                            {
                                Insights.Identify(HvzClient.Instance.ApiKey, new Dictionary<string, string>
                                {
                                    {Insights.Traits.Name, r.Profile.FullName},
                                    {Insights.Traits.Email, r.Profile.Email},
                                    {"Team", r.Profile.Team.ToString()}
                                });
                            });
                            break;

                        case ApiResponse.ResponseStatus.Error:
                            var errorAv = new UIAlertView("Error", "The entered api key is invalid", null, "OK", null);
                            errorAv.Show();
                            break;
                    }
                });
            });
	    }
	}
}
