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
	[Register ("MissionsViewController")]
	partial class MissionsViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIWebView WebView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (WebView != null) {
				WebView.Dispose ();
				WebView = null;
			}
		}
	}
}
