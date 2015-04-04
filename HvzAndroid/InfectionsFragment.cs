
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

        public InfectionsFragment()
        {
            this.client = HvzClient.Instance;
            RetainInstance = true;
        }

        public InfectionsFragment(HvzClient client)
        {
            this.client = client;
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.infections_fragment, container, false);

            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recycler_view);

            layoutManager = new LinearLayoutManager(this.Activity);
            recyclerView.SetLayoutManager(layoutManager);

            recyclerView.SetItemAnimator(new DefaultItemAnimator());

            adapter = new InfectionAdapter(this.Activity);
            recyclerView.SetAdapter(adapter);

            recyclerView.SetOnScrollListener(new OnScrollListener(this));

            return view;
        }

        public override void OnStart()
        {
            base.OnStart();

            RefreshList();
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
                        if (this.Activity == null)
                            return;

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
                                new AlertDialog.Builder(this.Activity)
                                    .SetTitle("Error")
                                    .SetMessage(Resource.String.api_err_infection_list)
                                    .SetPositiveButton("OK", (s, a) => { })
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

