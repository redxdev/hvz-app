
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

using Hvz.Api;
using Hvz.Api.Response;

namespace Hvz
{
    public class StatusFragment : Fragment, SwipeRefreshLayout.IOnRefreshListener
    {
        private HvzClient client = null;

        private SwipeRefreshLayout refreshLayout = null;

        private TextView humanCount = null;
        private TextView zombieCount = null;

        private TextView dayCount = null;
        private TextView hourCount = null;
        private TextView minuteCount = null;
        private TextView secondCount = null;

        public StatusFragment(HvzClient client)
        {
            this.client = client;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.Status, container, false);
        }

        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);

            RefreshStatus();
        }

        public override void OnStart()
        {
            base.OnStart();

            refreshLayout = this.View.FindViewById<SwipeRefreshLayout>(Resource.Id.refresh_layout);
            refreshLayout.SetOnRefreshListener(this);

            humanCount = this.View.FindViewById<TextView>(Resource.Id.human_count);
            zombieCount = this.View.FindViewById<TextView>(Resource.Id.zombie_count);

            dayCount = this.View.FindViewById<TextView>(Resource.Id.day_count);
            hourCount = this.View.FindViewById<TextView>(Resource.Id.hour_count);
            minuteCount = this.View.FindViewById<TextView>(Resource.Id.minute_count);
            secondCount = this.View.FindViewById<TextView>(Resource.Id.second_count);
            refreshLayout = this.View.FindViewById<SwipeRefreshLayout>(Resource.Id.refresh_layout);
        }

        public void RefreshStatus()
        {
            this.client.GetTeamStatus((response) =>
                {
                    this.Activity.RunOnUiThread(() => {
                        switch(response.Status)
                        {
                            case ApiResponse.ResponseStatus.Ok:
                                humanCount.Text = response.HumanCount.ToString();
                                zombieCount.Text = response.ZombieCount.ToString();
                                break;

                            case ApiResponse.ResponseStatus.Error:
                                humanCount.Text = "Err";
                                zombieCount.Text = "Err";
                                break;
                        }
                    });
                });

            this.client.GetGameStatus((response) =>
                {
                    this.Activity.RunOnUiThread(() => {
                        refreshLayout.Refreshing = false;

                        switch(response.Status)
                        {
                            case ApiResponse.ResponseStatus.Ok:
                                if(response.Game != GameStatusResponse.GameStatus.None)
                                {
                                    dayCount.Text = response.Days.ToString() + " DAYS";
                                    hourCount.Text = response.Hours.ToString() + " HOURS";
                                    minuteCount.Text = response.Minutes.ToString() + " MINUTES";
                                    secondCount.Text = response.Seconds.ToString() + " SECONDS";
                                }
                                else
                                {
                                    dayCount.Text = "- DAYS";
                                    hourCount.Text = "- HOURS";
                                    minuteCount.Text = "- MINUTES";
                                    secondCount.Text = "- SECONDS";
                                }
                                break;

                            case ApiResponse.ResponseStatus.Error:
                                dayCount.Text = "? DAYS";
                                hourCount.Text = "? HOURS";
                                minuteCount.Text = "? MINUTES";
                                secondCount.Text = "? SECONDS";
                                break;
                        }
                    });
                });
        }

        public void OnRefresh()
        {
            RefreshStatus();
        }
    }
}

