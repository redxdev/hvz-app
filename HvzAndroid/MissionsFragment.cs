
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
    public class MissionsFragment : Fragment
    {
        private HvzClient client = null;

        private RecyclerView recyclerView = null;

        private LinearLayoutManager layoutManager = null;

        private MissionAdapter adapter = null;

        public MissionsFragment()
        {
            this.client = HvzClient.Instance;
        }

        public MissionsFragment(HvzClient client)
        {
            this.client = client;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.missions_fragment, container, false);
            
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recycler_view);

            layoutManager = new LinearLayoutManager(this.Activity);
            recyclerView.SetLayoutManager(layoutManager);

            recyclerView.SetItemAnimator(new DefaultItemAnimator());

            adapter = new MissionAdapter(this.Activity);
            recyclerView.SetAdapter(adapter);

            return view;
        }

        public override void OnStart()
        {
            base.OnStart();

            if (client.ApiKey.Length != 32)
            {
                Toast.MakeText(this.Activity, Resource.String.api_err_bad_key, ToastLength.Long)
                    .Show();
            }
            else
            {
                client.GetMissionList(response =>
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
                                int start = adapter.Missions.Count;
                                adapter.Missions.AddRange(response.Missions);
                                adapter.NotifyItemRangeInserted(start, response.Missions.Count);
                                break;

                            case ApiResponse.ResponseStatus.Error:
                                Toast.MakeText(this.Activity, Resource.String.api_err_mission_list, ToastLength.Short)
                                    .Show();
                                break;
                        }
                    });
                });
            }
        }
    }
}

