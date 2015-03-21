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
	[Register ("SettingsViewController")]
	partial class SettingsViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField ApiTextField { get; set; }

		[Action ("OnSaveButtonPressed:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void OnSaveButtonPressed (UIButton sender);

		[Action ("OnScanButtonPressed:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void OnScanButtonPressed (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (ApiTextField != null) {
				ApiTextField.Dispose ();
				ApiTextField = null;
			}
		}
	}
}
