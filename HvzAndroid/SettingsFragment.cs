
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
using Xamarin;

namespace Hvz
{
    public class SettingsFragment : Fragment
    {
        private HvzClient client = null;

        private EditText apiKeyInput = null;

        public SettingsFragment()
        {
            this.client = HvzClient.Instance;
        }

        public SettingsFragment(HvzClient client)
        {
            this.client = client;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.settings_fragment, container, false);

            apiKeyInput = view.FindViewById<EditText>(Resource.Id.api_key_input);

            var settings = this.Activity.GetSharedPreferences(MainActivity.PrefsName, 0);
            apiKeyInput.Text = settings.GetString("apikey", string.Empty);

            var saveButton = view.FindViewById<Button>(Resource.Id.save_button);
            saveButton.Click += Submit;

            return view;
        }

        public void Submit(object sender, EventArgs args)
        {
            var imm = (InputMethodManager)this.Activity.GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(apiKeyInput.ApplicationWindowToken, 0);

            string apikey = apiKeyInput.Text.Trim();
            if (apikey.Length != 32)
            {
                Toast.MakeText(this.Activity, "Api key must be 32 characters long", ToastLength.Long)
                    .Show();
                return;
            }

            var settings = this.Activity.GetSharedPreferences(MainActivity.PrefsName, 0);
            var editor = settings.Edit();
            editor.PutString("apikey", apikey);
            editor.Commit();

            client.ApiKey = apikey;
            Log.Debug("HvzAndroid", "Changed api key to \"" + client.ApiKey + "\"");

            Toast.MakeText(this.Activity, Resource.String.settings_api_change, ToastLength.Short)
                .Show();

            client.TestApiKey(response =>
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
                            Toast.MakeText(this.Activity, Resource.String.settings_api_test_ok, ToastLength.Long)
                                .Show();
                            client.GetProfile(r =>
                            {
                                if (response.Status == ApiResponse.ResponseStatus.Ok)
                                {
                                    Insights.Identify(client.ApiKey, new Dictionary<string, string>
                                    {
                                        {Insights.Traits.Name, r.Profile.FullName},
                                        {Insights.Traits.Email, r.Profile.Email},
                                        {"Team", r.Profile.Team.ToString()}
                                    });
                                }
                            });
                            break;

                        case ApiResponse.ResponseStatus.Error:
                            Toast.MakeText(this.Activity, Resource.String.settings_api_test_error, ToastLength.Long)
                                .Show();
                            break;
                    }
                });
            });
        }
    }
}

