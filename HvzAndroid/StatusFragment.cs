
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

        private CountdownTimer timer = null;

        private bool loading = false;

        public StatusFragment()
        {
            this.client = HvzClient.Instance;
            RetainInstance = true;
        }

        public StatusFragment(HvzClient client)
        {
            this.client = client;
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.status_fragment, container, false);

            humanCount = view.FindViewById<TextView>(Resource.Id.human_count);
            zombieCount = view.FindViewById<TextView>(Resource.Id.zombie_count);

            dayCount = view.FindViewById<TextView>(Resource.Id.day_count);
            hourCount = view.FindViewById<TextView>(Resource.Id.hour_count);
            minuteCount = view.FindViewById<TextView>(Resource.Id.minute_count);
            secondCount = view.FindViewById<TextView>(Resource.Id.second_count);

            return view;
        }

        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);

            RefreshStatus();
        }

        public override void OnDetach()
        {
            base.OnDetach();

            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
        }

        private void TimerCallback(int days, int hours, int minutes, int seconds)
        {
            this.Activity.RunOnUiThread(() =>
            {
                dayCount.Text = days.ToString() + " DAYS";
                hourCount.Text = hours.ToString() + " HOURS";
                minuteCount.Text = minutes.ToString() + " MINUTES";
                secondCount.Text = seconds.ToString() + " SECONDS";
            });
        }

        public void RefreshStatus()
        {
            if (loading)
                return;

            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }

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
                                new AlertDialog.Builder(this.Activity)
                                    .SetTitle("Error")
                                    .SetMessage(Resource.String.api_err_team_status)
                                    .SetPositiveButton("OK", (s, a) => { })
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

                                    timer = new CountdownTimer();
                                    timer.Callback = TimerCallback;
                                    timer.Start(response.Days, response.Hours, response.Minutes, response.Seconds);
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
                                new AlertDialog.Builder(this.Activity)
                                    .SetTitle("Error")
                                    .SetMessage(Resource.String.api_err_game_status)
                                    .SetPositiveButton("OK", (s, a) => { })
                                    .Show();
                                break;
                        }
                    });
                });
        }
    }
}

