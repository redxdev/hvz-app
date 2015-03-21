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
	    protected bool lastPage = false;

	    protected string sortBy = "team";

	    protected string currentSearch = "";

		public PlayersViewController (IntPtr handle) : base (handle)
		{
		}

	    public override void ViewDidAppear(bool animated)
	    {
	        base.ViewDidAppear(animated);

	        source = new PlayersTableViewSource {Controller = this};
	        TableView.Source = source;

	        sortBy = "team";
            var button = ToolbarItems[0];
	        button.Title = "Team";

	        RefreshList();

            if (NavigationController != null)
                NavigationController.SetToolbarHidden(false, true);
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
	        lastPage = false;
            LoadPage(0);
	    }

	    protected void LoadPage(int page)
	    {
	        if (loading || source == null || (page >= currentPage && lastPage == true))
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

                            if (response.Players.Count < 10)
                                lastPage = true;

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
            }, 10, sortBy);
	    }

	    private void ChangeSortMethod(string newMethod)
	    {
	        string method = newMethod.ToLower();
	        if (method == sortBy)
	            return;

	        sortBy = method;

            RefreshList();
	    }

        partial void OnSearchButtonPressed(UIBarButtonItem sender)
        {
            var av = new UIAlertView("Search", "Enter your search terms.", null, "Cancel", "Search");
            av.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
            av.GetTextField(0).Text = currentSearch;

            av.Clicked += (o, args) =>
            {
                if(args.ButtonIndex == 1)
                    SearchList(av.GetTextField(0).Text);
            };

            av.Show();
        }

	    private void SearchList(string term)
	    {
	        if (loading || source == null)
	            return;

            currentSearch = term;

	        if (term.Length < 3)
	        {
	            var av = new UIAlertView("Error", "Searches have a minimum of three characters", null, "OK", null);
                av.Show();
	            return;
	        }

	        currentPage = 0;
	        lastPage = false;
	        loading = true;
            sortBy = "search";

            source.Players.Clear();
            TableView.ReloadData();

            HvzClient.Instance.SearchPlayerList(term, (response) =>
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

                            TableView.ReloadData();
                            break;

                        case ApiResponse.ResponseStatus.Error:
                            var av = new UIAlertView("Error", "There was a problem searching the player list.", null, "OK",
                                null);
                            av.Show();
                            break;
                    }

                    loading = false;
                });
            });
	    }

        partial void OnSortButtonPressed(UIBarButtonItem sender)
        {
            var av = new UIAlertView("Sort By", "Select how you would like to sort players", null, "Cancel", "Team", "Clan",
                "Mods");

            av.Clicked += (o, args) =>
            {
                var button = ToolbarItems[0];

                switch (args.ButtonIndex)
                {
                    case 1:
                        button.Title = "Team";
                        ChangeSortMethod(GameUtils.SortByTeam);
                        break;

                    case 2:
                        button.Title = "Clan";
                        ChangeSortMethod(GameUtils.SortByClan);
                        break;

                    case 3:
                        button.Title = "Mods";
                        ChangeSortMethod(GameUtils.SortByMods);
                        break;
                }
            };

            av.Show();
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
	            else
	            {
	                cell.ImageView.Image = null;
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
