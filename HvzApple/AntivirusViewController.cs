using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

using Hvz.Api;
using Hvz.Api.Response;

namespace Hvz
{
	partial class AntivirusViewController : UIViewController
	{
	    private bool loading = false;

		public AntivirusViewController (IntPtr handle) : base (handle)
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

            NavigationController.SetToolbarHidden(true, true);
	    }

	    partial void OnZombieScanButtonPressed(UIButton sender)
	    {
	        InvokeOnMainThread(async () =>
	        {
	            string result = await HvzClient.Instance.ScanQRId(GameUtils.Team.Zombie);
	            if (result != null)
	                ZombieIdText.Text = result;
	        });
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
	            string antivirusId = AntivirusText.Text;
	            string zombieId = ZombieIdText.Text;

	            if (antivirusId.Length != 8)
	            {
	                var avErr = new UIAlertView("Error", "Invalid antivirus (must be 8 characters long)", null, "OK", null);
                    avErr.Show();
	                return;
	            }

                if (zombieId.Length != 8)
                {
                    var zombieErr = new UIAlertView("Error", "Invalid zombie id (must be 8 characters long)", null, "OK",
                        null);
                    zombieErr.Show();
                    return;
                }

	            loading = true;

                HvzClient.Instance.RegisterAntivirus(antivirusId, zombieId, response =>
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
                                AntivirusText.Text = string.Empty;
                                ZombieIdText.Text = string.Empty;
                                var successMsg = new UIAlertView("Success",
                                    string.Format("{0} has taken an antivirus and become human once more!",
                                        response.ZombieName), null, "OK", null);
                                successMsg.Show();
                                break;

                            case ApiResponse.ResponseStatus.Error:
                                var err = new UIAlertView("Error",
                                    "Unable to register the antivirus. Make sure both ids are correct.", null, "OK", null);
                                err.Show();
                                break;
                        }

                        loading = false;
                    });
                });
	        }
	    }
	}
}
