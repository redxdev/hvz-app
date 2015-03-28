// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Hvz
{
	[Register ("HomeViewController")]
	partial class HomeViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView MenuTable { get; set; }

		[Action ("OnContactButtonPressed:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void OnContactButtonPressed (UIBarButtonItem sender);

		void ReleaseDesignerOutlets ()
		{
			if (MenuTable != null) {
				MenuTable.Dispose ();
				MenuTable = null;
			}
		}
	}
}
