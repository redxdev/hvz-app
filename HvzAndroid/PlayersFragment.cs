
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;

using Hvz.Api;
using Hvz.Api.Game;
using Hvz.Api.Response;
using Hvz.Ui;

namespace Hvz
{
    public class PlayersFragment : Fragment
    {
        private HvzClient client = null;

        private RecyclerView recyclerView = null;

        private LinearLayoutManager layoutManager = null;

        private PlayerAdapter adapter = null;

        private Button sortButton = null;

        private string sortBy = "team";

        private string currentSearch = "";

        private bool loading = false;

        private int currentPage = 0;

        public PlayersFragment()
        {
            this.client = HvzClient.Instance;
            RetainInstance = true;
        }

        public PlayersFragment(HvzClient client)
        {
            this.client = client;
            RetainInstance = true;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.players_fragment, container, false);

            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recycler_view);

            layoutManager = new LinearLayoutManager(this.Activity);
            recyclerView.SetLayoutManager(layoutManager);

            recyclerView.SetItemAnimator(new DefaultItemAnimator());

            adapter = new PlayerAdapter(this.Activity);
            recyclerView.SetAdapter(adapter);

            recyclerView.SetOnScrollListener(new OnScrollListener(this));

            sortButton = view.FindViewById<Button>(Resource.Id.sort_button);
            sortButton.Click += (sender, args) => new AlertDialog.Builder(this.Activity)
                .SetTitle("Sort Method")
                .SetItems(new string[] { "Team", "Clan", "Mods" }, (o, eventArgs) =>
                {
                    switch (eventArgs.Which)
                    {
                        case 0:
                            sortButton.Text = "Team";
                            ChangeSortMethod(GameUtils.SortByTeam);
                            break;

                        case 1:
                            sortButton.Text = "Clan";
                            ChangeSortMethod(GameUtils.SortByClan);
                            break;

                        case 2:
                            sortButton.Text = "Mods";
                            ChangeSortMethod(GameUtils.SortByMods);
                            break;
                    }
                })
                .Show();

            var searchButton = view.FindViewById<Button>(Resource.Id.search_button);
            searchButton.Click += (sender, args) =>
            {
                EditText input = new EditText(this.Activity);
                input.Text = currentSearch;

                new AlertDialog.Builder(this.Activity)
                    .SetTitle("Search")
                    .SetView(input)
                    .SetPositiveButton("Search", (o, eventArgs) => SearchList(input.Text))
                    .Show();
            };

            return view;
        }

        public override void OnStart()
        {
            base.OnStart();

            RefreshList();
        }

        private void ChangeSortMethod(string newMethod)
        {
            string method = newMethod.ToLower();
            if (method == sortBy)
                return;

            sortBy = method;

            RefreshList();
        }

        private void LoadPage(int page)
        {
            if (loading)
                return;

            loading = true;

            if (client == null)
                client = HvzClient.Instance;

            client.GetPlayerList(page, (response) =>
                {
                    if (Activity == null)
                        return;

                    this.Activity.RunOnUiThread(() =>
                    {
                        if (this.Activity == null)
                            return;

                        switch (response.Status)
                        {
                            case ApiResponse.ResponseStatus.Ok:
                                int start = adapter.Players.Count;
                                adapter.Players.AddRange(response.Players);
                                adapter.NotifyItemRangeInserted(start, response.Players.Count);

                                if (page > currentPage)
                                    currentPage = page;
                                break;

                            case ApiResponse.ResponseStatus.Error:
                                new AlertDialog.Builder(this.Activity)
                                    .SetTitle("Error")
                                    .SetMessage(Resource.String.api_err_player_list)
                                    .SetPositiveButton("OK", (s, a) => { })
                                    .Show();
                                break;
                        }

                        loading = false;
                    });
                }, 10, sortBy);
        }

        private void RefreshList()
        {
            if (loading)
                return;

            int count = adapter.Players.Count;
            adapter.Players.Clear();
            adapter.NotifyItemRangeRemoved(0, count);

            currentPage = 0;
            LoadPage(0);
        }

        private void SearchList(string term)
        {
            if (loading)
                return;

            currentSearch = term;

            if (term.Length < 3)
            {
                new AlertDialog.Builder(this.Activity)
                    .SetTitle("Error")
                    .SetMessage("Searches have a minimum of three characters")
                    .SetPositiveButton("OK", (s, a) => { })
                    .Show();
                return;
            }

            int count = adapter.Players.Count;
            adapter.Players.Clear();
            adapter.NotifyItemRangeRemoved(0, count);

            currentPage = 0;
            loading = true;

            sortButton.Text = "---";
            sortBy = "search";

            client.SearchPlayerList(term, (response) =>
            {
                if (Activity == null)
                    return;

                this.Activity.RunOnUiThread(() =>
                {

                    if (this.Activity == null)
                    {
                        return;
                    }

                    switch (response.Status)
                    {
                        case ApiResponse.ResponseStatus.Ok:
                            int start = adapter.Players.Count;
                            adapter.Players.AddRange(response.Players);
                            adapter.NotifyItemRangeInserted(start, response.Players.Count);
                            break;

                        case ApiResponse.ResponseStatus.Error:
                            new AlertDialog.Builder(this.Activity)
                                .SetTitle("Error")
                                .SetMessage(Resource.String.api_err_player_list)
                                .SetPositiveButton("OK", (s, a) => { })
                                .Show();
                            break;
                    }

                    loading = false;
                });
            });
        }

        private class OnScrollListener : RecyclerView.OnScrollListener
        {
            private PlayersFragment fragment = null;

            public OnScrollListener(PlayersFragment fragment)
                : base()
            {
                this.fragment = fragment;
            }

            public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
            {
                if (fragment.layoutManager.ChildCount + fragment.layoutManager.FindFirstVisibleItemPosition() >=
                    fragment.layoutManager.ItemCount)
                {
                    fragment.LoadPage(fragment.currentPage + 1);
                }
            }
        }
    }
}

