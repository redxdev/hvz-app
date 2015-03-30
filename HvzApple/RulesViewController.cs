using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

using Hvz.Api;
using Hvz.Api.Game;

namespace Hvz
{
	partial class RulesViewController : UIViewController
	{
		public RulesViewController (IntPtr handle) : base (handle)
		{
		}

	    public override void ViewDidAppear(bool animated)
	    {
	        base.ViewDidAppear(animated);

	        Refresh();

            NavigationController.SetToolbarHidden(true, true);
	    }

	    public override void ViewDidDisappear(bool animated)
	    {
	        base.ViewDidDisappear(animated);
	    }

	    private void Refresh()
	    {
	        HvzClient.Instance.GetRulesets(response =>
	        {
                // Building a single webpage by concatinating all the rules seems like it would
                // be really hacky, but it works suprisingly well.

	            string html = string.Empty;

	            foreach (Ruleset ruleset in response.Rulesets)
	            {
	                html += string.Format(
                        "<h1>{0}</h1>{1}<hr>",
                        ruleset.Title,
                        ruleset.Body
	                    );
	            }

                InvokeOnMainThread(() =>
                {
                    WebView.LoadHtmlString(html, null);
                });
	        });
	    }
	}
}
