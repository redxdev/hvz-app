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

            NavigationController.SetToolbarHidden(true, true);
	    }
	}
}
