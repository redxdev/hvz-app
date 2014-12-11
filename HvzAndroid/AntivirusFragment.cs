
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
using Java.IO;

namespace Hvz
{
    public class AntivirusFragment : Fragment
    {
        private HvzClient client = null;

        private EditText antivirusIdInput = null;

        private EditText zombieIdInput = null;

        public AntivirusFragment()
        {
            this.client = HvzClient.Instance;
        }

        public AntivirusFragment(HvzClient client)
        {
            this.client = client;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.antivirus_fragment, container, false);
        }

        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
        }

        public override void OnStart()
        {
            base.OnStart();

            antivirusIdInput = this.View.FindViewById<EditText>(Resource.Id.antivirus_id_input);
            zombieIdInput = this.View.FindViewById<EditText>(Resource.Id.zombie_id_input);

            var submitButton = this.View.FindViewById<Button>(Resource.Id.submit_button);
            submitButton.Click += (sender, args) =>
            {
                if (client.ApiKey.Length != 32)
                {
                    Toast.MakeText(this.Activity, Resource.String.api_err_bad_key, ToastLength.Long)
                        .Show();
                }
                else
                {
                    string antivirusId = antivirusIdInput.Text.Trim();
                    string zombieId = zombieIdInput.Text.Trim();

                    if (antivirusId.Length != 8)
                    {
                        Toast.MakeText(this.Activity, Resource.String.infect_err_antivirus_id_length, ToastLength.Long)
                            .Show();
                        return;
                    }

                    if (zombieId.Length != 8)
                    {
                        Toast.MakeText(this.Activity, Resource.String.infect_err_zombie_id_length, ToastLength.Long)
                            .Show();
                        return;
                    }

                    client.RegisterAntivirus(antivirusId, zombieId, response =>
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
                                    antivirusIdInput.Text = string.Empty;
                                    Toast.MakeText(this.Activity, string.Format("{0} has taken an antivirus and become human once more!", response.ZombieName), ToastLength.Long)
                                        .Show();
                                    break;

                                case ApiResponse.ResponseStatus.Error:
                                    Toast.MakeText(this.Activity, Resource.String.infect_err_antivirus_generic, ToastLength.Short)
                                        .Show();
                                    foreach (string error in response.Errors)
                                    {
                                        Toast.MakeText(this.Activity, error, ToastLength.Short)
                                            .Show();
                                    }
                                    break;
                            }
                        });
                    });
                }
            };

            var zombieScanButton = this.View.FindViewById<Button>(Resource.Id.zombie_scan_button);
            zombieScanButton.Click += async (sender, args) =>
            {
                string result = await client.ScanQRId(this.Activity, GameUtils.Team.Zombie);
                if (result != null)
                    zombieIdInput.Text = result;
            };

            if (client.ApiKey.Length != 32)
            {
                Toast.MakeText(this.Activity, Resource.String.api_err_bad_key, ToastLength.Long)
                    .Show();
            }
        }
    }
}
