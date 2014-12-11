
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
    public class InfectionsFragment : Fragment
    {
        private HvzClient client = null;

        private RecyclerView recyclerView = null;

        private LinearLayoutManager layoutManager = null;

        private InfectionAdapter adapter = null;

        private bool loading = false;

        private int currentPage = 0;

        public InfectionsFragment(HvzClient client)
        {
            this.client = client;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.infections_fragment, container, false);
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

            adapter = new InfectionAdapter(this.Activity);
            recyclerView.SetAdapter(adapter);

            recyclerView.SetOnScrollListener(new OnScrollListener(this));

            LoadPage(0);
        }

        private void LoadPage(int page)
        {
            if (loading)
                return;

            loading = true;

            client.GetInfectionList(page, (response) =>
                {
                    if (Activity == null)
                        return;

                    this.Activity.RunOnUiThread(() =>
                    {
                        switch (response.Status)
                        {
                            case ApiResponse.ResponseStatus.Ok:
                                int start = adapter.Infections.Count;
                                adapter.Infections.AddRange(response.Infections);
                                adapter.NotifyItemRangeInserted(start, response.Infections.Count);

                                if (page > currentPage)
                                    currentPage = page;
                                break;

                            case ApiResponse.ResponseStatus.Error:
                                Toast.MakeText(this.Activity, Resource.String.api_err_infection_list, ToastLength.Short)
                                    .Show();
                                break;
                        }

                        loading = false;
                    });
                }, 10);
        }

        private void RefreshList()
        {
            if (loading)
                return;

            int count = adapter.Infections.Count;
            adapter.Infections.Clear();
            adapter.NotifyItemRangeRemoved(0, count);

            currentPage = 0;
            LoadPage(0);
        }

        private class OnScrollListener : RecyclerView.OnScrollListener
        {
            private InfectionsFragment fragment = null;

            public OnScrollListener(InfectionsFragment fragment)
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

