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
	partial class InfectionsViewController : UITableViewController
	{
	    private InfectionsTableViewSource source = null;

	    private bool loading = false;

	    protected int currentPage = 0;
	    protected bool lastPage = false;

		public InfectionsViewController (IntPtr handle) : base (handle)
		{
		}

	    public override void ViewDidAppear(bool animated)
	    {
	        base.ViewDidAppear(animated);

	        source = new InfectionsTableViewSource {Controller = this};
	        TableView.Source = source;

            RefreshList();

            NavigationController.SetToolbarHidden(true, true);
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

            source.Infections.Clear();

	        currentPage = 0;
	        lastPage = false;
            LoadPage(0);
	    }

	    protected void LoadPage(int page)
	    {
	        if (loading || source == null || (page >= currentPage && lastPage == true))
	            return;

	        loading = true;

            HvzClient.Instance.GetInfectionList(page, response =>
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
                            source.Infections.AddRange(response.Infections);

                            if (!response.HasMorePages)
                                lastPage = true;

                            if (page > currentPage)
                                currentPage = page;

                            TableView.ReloadData();
                            break;

                        case ApiResponse.ResponseStatus.Error:
                            var av = new UIAlertView("Error", "There was a problem retrieving the infection list", null,
                                "OK", null);
                            av.Show();
                            break;
                    }
                });
            }, 10);
	    }

	    private class InfectionsTableViewSource : UITableViewSource
	    {
	        private const string cellIdentifier = "infectionCell";

            public List<Infection> Infections { get; set; }

            public InfectionsViewController Controller { get; set; }

	        public InfectionsTableViewSource()
	        {
	            Infections = new List<Infection>();
	        }

	        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
	        {
	            var cell = tableView.DequeueReusableCell(cellIdentifier) ??
	                       new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);

	            Infection infection = Infections[indexPath.Row];
	            cell.TextLabel.Text = string.Format(
                    "{0} was infected by {1} {2}",
                    infection.HumanName,
                    infection.ZombieName,
                    TimeUtils.PrettyDate(infection.Time)
	                );
	            cell.TextLabel.Lines = 3;

                if(indexPath.Row == Infections.Count - 1)
                    Controller.LoadPage(Controller.currentPage + 1);

	            return cell;
	        }

	        public override nint RowsInSection(UITableView tableview, nint section)
	        {
	            return Infections.Count;
	        }
	    }
	}
}
