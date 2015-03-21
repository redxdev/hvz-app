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
	[Register ("RegisterInfectionViewController")]
	partial class RegisterInfectionViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField HumanIdText { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel LocationLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField ZombieIdText { get; set; }

		[Action ("OnSubmitButtonPressed:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void OnSubmitButtonPressed (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (HumanIdText != null) {
				HumanIdText.Dispose ();
				HumanIdText = null;
			}
			if (LocationLabel != null) {
				LocationLabel.Dispose ();
				LocationLabel = null;
			}
			if (ZombieIdText != null) {
				ZombieIdText.Dispose ();
				ZombieIdText = null;
			}
		}
	}
}
