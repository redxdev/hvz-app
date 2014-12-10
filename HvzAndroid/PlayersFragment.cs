﻿
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

        private bool loading = false;

        private int currentPage = 0;

        public PlayersFragment(HvzClient client)
        {
            this.client = client;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.players_fragment, container, false);
        }

        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
        }

        public override void OnStart()
        {
            base.OnStart();

            recyclerView = this.View.FindViewById<RecyclerView>(Resource.Id.recycler_view);

            layoutManager = new LinearLayoutManager(this.Activity);
            recyclerView.SetLayoutManager(layoutManager);

            recyclerView.SetItemAnimator(new DefaultItemAnimator());

            adapter = new PlayerAdapter(this.Activity);
            recyclerView.SetAdapter(adapter);

            recyclerView.SetOnScrollListener(new OnScrollListener(this));

            LoadPage(0);
        }

        private void LoadPage(int page, string sort = GameUtils.SortByTeam)
        {
            if (loading)
                return;

            loading = true;

            client.GetPlayerList(page, (response) =>
                {
                    if (Activity == null)
                        return;

                    this.Activity.RunOnUiThread(() =>
                    {
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
                                Toast.MakeText(this.Activity, Resource.String.api_err_player_list, ToastLength.Short);
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

