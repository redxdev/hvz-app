using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

using Hvz.Api;
using Hvz.Api.Response;
using ZXing;
using ZXing.Common;

namespace Hvz
{
	partial class ProfileViewController : UIViewController
	{
	    private bool loading = false;

		public ProfileViewController (IntPtr handle) : base (handle)
		{
		}

	    public override void ViewDidAppear(bool animated)
	    {
	        base.ViewDidAppear(animated);

	        Refresh();

            NavigationController.SetToolbarHidden(true, true);
	    }

	    private void Refresh()
	    {
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
	            loading = true;

	            HvzClient.Instance.GetProfile(response =>
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
	                            NameLabel.Text = response.Profile.FullName;
	                            EmailLabel.Text = "Email: " + response.Profile.Email;
	                            ClanLabel.Text = "Clan: " +
	                                             (string.IsNullOrWhiteSpace(response.Profile.Clan)
	                                                 ? "none"
	                                                 : response.Profile.Clan);

	                            if (response.Profile.Team == GameUtils.Team.Human)
	                            {
	                                TeamLabel.Text = "Team: Human";
	                            }
	                            else
	                            {
	                                TeamLabel.Text = "Team: Zombie";
	                            }

	                            ZombieIdText.Text = "Zombie id: " + response.Profile.ZombieId;

	                            if (response.Profile.HumanIds.Count >= 1)
	                                HumanId1Text.Text = "Human id: " + response.Profile.HumanIds[0].Id;
	                            if (response.Profile.HumanIds.Count >= 2)
	                                HumanId2Text.Text = "Human id: " + response.Profile.HumanIds[1].Id;

	                            var writer = new ZXing.BarcodeWriter
	                            {
                                    Format = BarcodeFormat.QR_CODE,
                                    Options = new EncodingOptions
                                    {
                                        Height = 240,
                                        Width = 240
                                    }
	                            };

	                            var bitmap = writer.Write(response.Profile.QRData);
	                            QRImage.Image = UIImage.LoadFromData(NSData.FromStream(bitmap.AsPNG().AsStream()));
	                            break;

                            case ApiResponse.ResponseStatus.Error:
	                            var profileErr = new UIAlertView("Error", "Unable to retrieve profile. Is your api key correct?", null, "OK", null);
                                profileErr.Show();
	                            break;
	                    }

	                    loading = false;
	                });
	            });
	        }
	    }
	}
}
