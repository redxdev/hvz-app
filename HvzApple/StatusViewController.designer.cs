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
	[Register ("StatusViewController")]
	partial class StatusViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel DaysCount { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel HoursCount { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel HumanCount { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel MinutesCount { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel SecondsCount { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel ZombieCount { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (DaysCount != null) {
				DaysCount.Dispose ();
				DaysCount = null;
			}
			if (HoursCount != null) {
				HoursCount.Dispose ();
				HoursCount = null;
			}
			if (HumanCount != null) {
				HumanCount.Dispose ();
				HumanCount = null;
			}
			if (MinutesCount != null) {
				MinutesCount.Dispose ();
				MinutesCount = null;
			}
			if (SecondsCount != null) {
				SecondsCount.Dispose ();
				SecondsCount = null;
			}
			if (ZombieCount != null) {
				ZombieCount.Dispose ();
				ZombieCount = null;
			}
		}
	}
}
