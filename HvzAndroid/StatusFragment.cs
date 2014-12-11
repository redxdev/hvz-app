
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
    public class StatusFragment : Fragment
    {
        private HvzClient client = null;

        private TextView humanCount = null;
        private TextView zombieCount = null;

        private TextView dayCount = null;
        private TextView hourCount = null;
        private TextView minuteCount = null;
        private TextView secondCount = null;

        private bool loading = false;

        public StatusFragment()
        {
            this.client = HvzClient.Instance;
        }

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
            return inflater.Inflate(Resource.Layout.status_fragment, container, false);
        }

        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);

            RefreshStatus();
        }

        public override void OnStart()
        {
            base.OnStart();

            humanCount = this.View.FindViewById<TextView>(Resource.Id.human_count);
            zombieCount = this.View.FindViewById<TextView>(Resource.Id.zombie_count);

            dayCount = this.View.FindViewById<TextView>(Resource.Id.day_count);
            hourCount = this.View.FindViewById<TextView>(Resource.Id.hour_count);
            minuteCount = this.View.FindViewById<TextView>(Resource.Id.minute_count);
            secondCount = this.View.FindViewById<TextView>(Resource.Id.second_count);
        }

        public void RefreshStatus()
        {
            if (loading)
                return;

            loading = true;

            this.client.GetTeamStatus((response) =>
                {
                    if(Activity == null)
                        return;

                    this.Activity.RunOnUiThread(() =>
                    {
                        if (this.Activity == null)
                            return;

                        switch(response.Status)
                        {
                            case ApiResponse.ResponseStatus.Ok:
                                humanCount.Text = response.HumanCount.ToString();
                                zombieCount.Text = response.ZombieCount.ToString();
                                break;

                            case ApiResponse.ResponseStatus.Error:
                                humanCount.Text = "Err";
                                zombieCount.Text = "Err";
                                Toast.MakeText(this.Activity, Resource.String.api_err_team_status, ToastLength.Short)
                                    .Show();
                                break;
                        }
                    });
                });

            this.client.GetGameStatus((response) =>
                {
                    if(Activity == null)
                        return;

                    this.Activity.RunOnUiThread(() =>
                    {
                        if (this.Activity == null)
                            return;

                        loading = false;

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
                                Toast.MakeText(this.Activity, Resource.String.api_err_game_status, ToastLength.Short)
                                    .Show();
                                break;
                        }
                    });
                });
        }
    }
}

