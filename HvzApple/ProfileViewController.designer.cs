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
		UILabel HumanId1Text { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel HumanId2Text { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel NameLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView QRImage { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel TeamLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel ZombieIdText { get; set; }

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
			if (HumanId1Text != null) {
				HumanId1Text.Dispose ();
				HumanId1Text = null;
			}
			if (HumanId2Text != null) {
				HumanId2Text.Dispose ();
				HumanId2Text = null;
			}
			if (NameLabel != null) {
				NameLabel.Dispose ();
				NameLabel = null;
			}
			if (QRImage != null) {
				QRImage.Dispose ();
				QRImage = null;
			}
			if (TeamLabel != null) {
				TeamLabel.Dispose ();
				TeamLabel = null;
			}
			if (ZombieIdText != null) {
				ZombieIdText.Dispose ();
				ZombieIdText = null;
			}
		}
	}
}
