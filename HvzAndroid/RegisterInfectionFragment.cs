
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views.InputMethods;
using Hvz.Api;
using Hvz.Api.Game;
using Hvz.Api.Response;
using Hvz.Ui;
using Java.IO;

namespace Hvz
{
    public class RegisterInfectionFragment : Fragment, ILocationListener
    {
        private HvzClient client = null;

        private EditText humanIdInput = null;

        private EditText zombieIdInput = null;

        private TextView locationStatus = null;

        private LocationManager locationManager = null;

        private Location lastLocation = null;

        public RegisterInfectionFragment()
        {
            this.client = HvzClient.Instance;
        }

        public RegisterInfectionFragment(HvzClient client)
        {
            this.client = client;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            locationManager = this.Activity.GetSystemService(Context.LocationService) as LocationManager;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.register_infection_fragment, container, false);

            humanIdInput = view.FindViewById<EditText>(Resource.Id.human_id_input);
            zombieIdInput = view.FindViewById<EditText>(Resource.Id.zombie_id_input);
            locationStatus = view.FindViewById<TextView>(Resource.Id.location_status);

            var submitButton = view.FindViewById<Button>(Resource.Id.submit_button);
            submitButton.Click += Submit;

            var humanScanButton = view.FindViewById<Button>(Resource.Id.human_scan_button);
            humanScanButton.Click += async (sender, args) =>
            {
                string result = await client.ScanQRId(this.Activity, GameUtils.Team.Human);
                if (result != null)
                    humanIdInput.Text = result;
            };

            var zombieScanButton = view.FindViewById<Button>(Resource.Id.zombie_scan_button);
            zombieScanButton.Click += async (sender, args) =>
            {
                string result = await client.ScanQRId(this.Activity, GameUtils.Team.Zombie);
                if (result != null)
                    zombieIdInput.Text = result;
            };

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
        }

        public override void OnResume()
        {
            base.OnResume();

            Criteria locationCriteria = new Criteria();
            locationCriteria.Accuracy = Accuracy.Fine;
            locationCriteria.PowerRequirement = Power.High;

            string locationProvider = locationManager.GetBestProvider(locationCriteria, true);
            if (locationProvider != null)
            {
                locationManager.RequestLocationUpdates(locationProvider, 2000, 1, this);
            }
        }

        public override void OnPause()
        {
            base.OnPause();

            locationManager.RemoveUpdates(this);
        }

        public void Submit(object sender, EventArgs args)
        {
            var imm = (InputMethodManager)this.Activity.GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(humanIdInput.ApplicationWindowToken, 0);
            imm.HideSoftInputFromWindow(zombieIdInput.ApplicationWindowToken, 0);

            if (client.ApiKey.Length != 32)
            {
                Toast.MakeText(this.Activity, Resource.String.api_err_bad_key, ToastLength.Long)
                    .Show();
            }
            else
            {
                string humanId = humanIdInput.Text.Trim();
                string zombieId = zombieIdInput.Text.Trim();

                if (humanId.Length != 8)
                {
                    Toast.MakeText(this.Activity, Resource.String.infect_err_human_id_length, ToastLength.Long)
                        .Show();
                    return;
                }

                if (zombieId.Length != 8)
                {
                    Toast.MakeText(this.Activity, Resource.String.infect_err_zombie_id_length, ToastLength.Long)
                        .Show();
                    return;
                }

                Toast.MakeText(this.Activity, Resource.String.infect_submit, ToastLength.Short)
                    .Show();

                double? latitude = null;
                double? longitude = null;
                if (lastLocation != null)
                {
                    latitude = lastLocation.Latitude;
                    longitude = lastLocation.Longitude;
                }

                client.RegisterInfection(humanId, zombieId, response =>
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
                                humanIdInput.Text = string.Empty;
                                Toast.MakeText(this.Activity, string.Format("{0} has joined the horde, courtesy of {1}", response.HumanName, response.ZombieName), ToastLength.Long)
                                    .Show();
                                break;

                            case ApiResponse.ResponseStatus.Error:
                                Toast.MakeText(this.Activity, Resource.String.infect_err_generic, ToastLength.Short)
                                    .Show();
                                foreach (string error in response.Errors)
                                {
                                    Toast.MakeText(this.Activity, error, ToastLength.Short)
                                        .Show();
                                }
                                break;
                        }
                    });
                }, latitude, longitude);
            }
        }

        public void OnLocationChanged(Location location)
        {
            lastLocation = location;
            locationStatus.Text = "Location: SENDING";
        }

        public void OnProviderDisabled(string provider)
        {
            locationStatus.Text = "Location: NOT SENDING";
            lastLocation = null;
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
        }
    }
}

