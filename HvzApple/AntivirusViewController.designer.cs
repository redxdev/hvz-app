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
	[Register ("AntivirusViewController")]
	partial class AntivirusViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField AntivirusText { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField ZombieIdText { get; set; }

		[Action ("OnSubmitButtonPressed:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void OnSubmitButtonPressed (UIButton sender);

		[Action ("OnZombieScanButtonPressed:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void OnZombieScanButtonPressed (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (AntivirusText != null) {
				AntivirusText.Dispose ();
				AntivirusText = null;
			}
			if (ZombieIdText != null) {
				ZombieIdText.Dispose ();
				ZombieIdText = null;
			}
		}
	}
}
