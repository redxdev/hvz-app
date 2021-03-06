
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
using Android.Views.InputMethods;
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
            RetainInstance = true;
        }

        public AntivirusFragment(HvzClient client)
        {
            this.client = client;
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.antivirus_fragment, container, false);

            antivirusIdInput = view.FindViewById<EditText>(Resource.Id.antivirus_id_input);
            zombieIdInput = view.FindViewById<EditText>(Resource.Id.zombie_id_input);

            var submitButton = view.FindViewById<Button>(Resource.Id.submit_button);
            submitButton.Click += Submit;

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
                new AlertDialog.Builder(this.Activity)
                    .SetTitle("Error")
                    .SetMessage(Resource.String.api_err_bad_key)
                    .SetPositiveButton("OK", (s, a) => { })
                    .Show();
            }
        }

        public void Submit(object sender, EventArgs args)
        {
            var imm = (InputMethodManager)this.Activity.GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(antivirusIdInput.ApplicationWindowToken, 0);
            imm.HideSoftInputFromWindow(zombieIdInput.ApplicationWindowToken, 0);

            if (client.ApiKey.Length != 32)
            {
                new AlertDialog.Builder(this.Activity)
                    .SetTitle("Error")
                    .SetMessage(Resource.String.api_err_bad_key)
                    .SetPositiveButton("OK", (s, a) => { })
                    .Show();
            }
            else
            {
                string antivirusId = antivirusIdInput.Text.Trim();
                string zombieId = zombieIdInput.Text.Trim();

                if (antivirusId.Length != 8)
                {
                    new AlertDialog.Builder(this.Activity)
                        .SetTitle("Error")
                        .SetMessage(Resource.String.infect_err_antivirus_id_length)
                        .SetPositiveButton("OK", (s, a) => { })
                        .Show();
                    return;
                }

                if (zombieId.Length != 8)
                {
                    new AlertDialog.Builder(this.Activity)
                        .SetTitle("Error")
                        .SetMessage(Resource.String.infect_err_zombie_id_length)
                        .SetPositiveButton("OK", (s, a) => { })
                        .Show();
                    return;
                }

                Toast.MakeText(this.Activity, Resource.String.infect_antivirus_submit, ToastLength.Short)
                    .Show();

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
                                zombieIdInput.Text = string.Empty;
                                new AlertDialog.Builder(this.Activity)
                                    .SetTitle("Success")
                                    .SetMessage(string.Format("{0} has taken an antivirus and become human once more!",
                                        response.ZombieName))
                                    .SetPositiveButton("OK", (s, a) => { })
                                    .Show();
                                break;

                            case ApiResponse.ResponseStatus.Error:
                                new AlertDialog.Builder(this.Activity)
                                    .SetTitle("Error")
                                    .SetMessage(Resource.String.infect_err_antivirus_generic)
                                    .SetPositiveButton("OK", (s, a) => { })
                                    .Show();
                                break;
                        }
                    });
                });
            }
        }
    }
}

