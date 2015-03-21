using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Hvz
{
	partial class InfectionsViewController : UITableViewController
	{
		public InfectionsViewController (IntPtr handle) : base (handle)
		{
		}

	    public override void ViewDidAppear(bool animated)
	    {
	        base.ViewDidAppear(animated);

            NavigationController.SetToolbarHidden(true, true);
	    }
	}
}
