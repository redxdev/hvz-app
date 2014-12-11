
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
    public class RegisterInfectionFragment : Fragment
    {
        private HvzClient client = null;

        private EditText humanIdInput = null;

        private EditText zombieIdInput = null;

        public RegisterInfectionFragment(HvzClient client)
        {
            this.client = client;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.register_infection_fragment, container, false);
        }

        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
        }

        public override void OnStart()
        {
            base.OnStart();

            humanIdInput = this.View.FindViewById<EditText>(Resource.Id.human_id_input);
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

                    client.RegisterInfection(humanId, zombieId, response =>
                    {
                        if (this.Activity == null)
                            return;

                        this.Activity.RunOnUiThread(() =>
                        {
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
                    }); // TODO: location support
                }
            };

            if (client.ApiKey.Length != 32)
            {
                Toast.MakeText(this.Activity, Resource.String.api_err_bad_key, ToastLength.Long)
                    .Show();
            }
        }
    }
}

