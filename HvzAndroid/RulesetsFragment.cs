
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
    public class RulesetsFragment : Fragment
    {
        private HvzClient client = null;

        private RecyclerView recyclerView = null;

        private LinearLayoutManager layoutManager = null;

        private RulesetAdapter adapter = null;

        private RulesetsFragment()
        {
            this.client = HvzClient.Instance;
        }

        public RulesetsFragment(HvzClient client)
        {
            this.client = client;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.rulesets_fragment, container, false);

            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recycler_view);

            layoutManager = new LinearLayoutManager(this.Activity);
            recyclerView.SetLayoutManager(layoutManager);

            recyclerView.SetItemAnimator(new DefaultItemAnimator());

            adapter = new RulesetAdapter(this.Activity);
            recyclerView.SetAdapter(adapter);

            return view;
        }

        public override void OnStart()
        {
            base.OnStart();

            client.GetRulesets(response =>
            {
                if (this.Activity == null)
                    return;

                this.Activity.RunOnUiThread(() =>
                {
                    if (this.Activity == null)
                        return;

                    switch (response.Status)
                    {
                        case ApiResponse.ResponseStatus.Ok:
                            int start = adapter.Rulesets.Count;
                            adapter.Rulesets.Clear();
                            adapter.NotifyItemRangeRemoved(0, start);
                            adapter.Rulesets.AddRange(response.Rulesets);
                            adapter.NotifyItemRangeInserted(0, response.Rulesets.Count);
                            break;

                        case ApiResponse.ResponseStatus.Error:
                            Toast.MakeText(this.Activity, Resource.String.api_err_ruleset_list, ToastLength.Short)
                                .Show();
                            break;
                    }
                });
            });
        }
    }
}

