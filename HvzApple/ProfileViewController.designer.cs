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
	[Register ("ProfileViewController")]
	partial class ProfileViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel ClanLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel EmailLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel NameLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel TeamLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ClanLabel != null) {
				ClanLabel.Dispose ();
				ClanLabel = null;
			}
			if (EmailLabel != null) {
				EmailLabel.Dispose ();
				EmailLabel = null;
			}
			if (NameLabel != null) {
				NameLabel.Dispose ();
				NameLabel = null;
			}
			if (TeamLabel != null) {
				TeamLabel.Dispose ();
				TeamLabel = null;
			}
		}
	}
}
