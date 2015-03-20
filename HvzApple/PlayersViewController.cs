using Foundation;
using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using UIKit;

using Hvz.Api;
using Hvz.Api.Game;
using Hvz.Api.Response;

namespace Hvz
{
	partial class PlayersViewController : UITableViewController
	{
	    private PlayersTableViewSource source = null;

	    private bool loading = false;

	    protected int currentPage = 0;

		public PlayersViewController (IntPtr handle) : base (handle)
		{
		}

	    public override void ViewDidAppear(bool animated)
	    {
	        base.ViewDidAppear(animated);

	        source = new PlayersTableViewSource {Controller = this};
	        TableView.Source = source;

	        RefreshList();
	    }

	    public override void ViewDidDisappear(bool animated)
	    {
	        base.ViewDidDisappear(animated);

	        source = null;
	        loading = false;
	    }

	    protected void RefreshList()
	    {
	        if (loading || source == null)
	            return;

            source.Players.Clear();

	        currentPage = 0;
            LoadPage(0);
	    }

	    protected void LoadPage(int page)
	    {
	        if (loading || source == null)
	            return;

	        loading = true;

            HvzClient.Instance.GetPlayerList(page, (response) =>
            {
                InvokeOnMainThread(() =>
                {
                    if (source == null || !this.IsViewLoaded || View.Window == null)
                    {
                        loading = false;
                        return;
                    }

                    switch (response.Status)
                    {
                        case ApiResponse.ResponseStatus.Ok:
                            source.Players.AddRange(response.Players);

                            if (page > currentPage)
                                currentPage = page;

                            TableView.ReloadData();
                            break;

                        case ApiResponse.ResponseStatus.Error:
                            var av = new UIAlertView("Error", "There was a problem retrieving the player list", null,
                                "OK", null);
                            break;
                    }

                    loading = false;
                });
            });
	    }

	    private class PlayersTableViewSource : UITableViewSource
	    {
	        private const string cellIdentifier = "playerCell";

            public List<Player> Players { get; set; }

            public PlayersViewController Controller { get; set; }

	        public PlayersTableViewSource()
	        {
                Players = new List<Player>();
	        }

	        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
	        {
	            var cell = tableView.DequeueReusableCell(cellIdentifier);
                if(cell == null)
                    cell = new UITableViewCell(UITableViewCellStyle.Subtitle, cellIdentifier);

	            Player player = Players[indexPath.Row];
	            cell.TextLabel.Text = player.FullName;
	            cell.DetailTextLabel.Text = "Tags: " + player.HumansTagged + ", Clan: " + (string.IsNullOrWhiteSpace(player.Clan) ? "none" : player.Clan);

	            if (!string.IsNullOrWhiteSpace(player.Avatar))
	            {
	                try
	                {
	                    using (var url = new NSUrl(ApiConfiguration.BaseUrl + "/" + player.Avatar))
	                    using (var data = NSData.FromUrl(url))
	                        cell.ImageView.Image = UIImage.LoadFromData(data);
	                }
	                catch (Exception e)
	                {
                        // horrible way to do this, but idk what kind of exceptions the code can throw, and it really
                        // doesn't matter since we would want to just display an empty image anyway.
	                }
	            }

                if(indexPath.Row == Players.Count - 1)
                    Controller.LoadPage(Controller.currentPage + 1);

	            return cell;
	        }

	        public override nint RowsInSection(UITableView tableview, nint section)
	        {
	            return Players.Count;
	        }
	    }
	}
}
