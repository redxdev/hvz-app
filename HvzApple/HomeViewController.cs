using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Hvz
{
	partial class HomeViewController : UITableViewController
	{
		public HomeViewController (IntPtr handle) : base (handle)
		{
		}

	    public override void ViewDidAppear(bool animated)
	    {
	        base.ViewDidAppear(animated);

            NavigationController.SetToolbarHidden(false, true);
	    }

	    partial void OnContactButtonPressed(UIBarButtonItem sender)
	    {
	        var url = new NSUrl("mailto:hvz@rit.edu");
	        if (!UIApplication.SharedApplication.OpenUrl(url))
	        {
	            var av = new UIAlertView("Error",
	                "There was a problem opening your email app. Try sending an email yourself to hvz@rit.edu", null, "OK",
	                null);
                av.Show();
	        }
	    }
	}
}
