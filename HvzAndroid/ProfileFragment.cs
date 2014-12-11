
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
    public class ProfileFragment : Fragment
    {
        private HvzClient client = null;

        private CardView playerCard = null;

        private TextView nameText = null;

        private TextView emailText = null;

        private TextView teamText = null;

        private TextView clanText = null;

        public ProfileFragment()
        {
            this.client = HvzClient.Instance;
        }

        public ProfileFragment(HvzClient client)
        {
            this.client = client;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.profile_fragment, container, false);
        }

        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
        }

        public override void OnStart()
        {
            base.OnStart();

            playerCard = this.View.FindViewById<CardView>(Resource.Id.player_card);
            nameText = this.View.FindViewById<TextView>(Resource.Id.player_name);
            emailText = this.View.FindViewById<TextView>(Resource.Id.player_email);
            teamText = this.View.FindViewById<TextView>(Resource.Id.player_team);
            clanText = this.View.FindViewById<TextView>(Resource.Id.player_clan);

            if (client.ApiKey.Length != 32)
            {
                Toast.MakeText(this.Activity, Resource.String.api_err_bad_key, ToastLength.Long)
                    .Show();
            }
            else
            {
                client.GetProfile(response =>
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
                                nameText.Text = response.Profile.FullName;
                                emailText.Text = "Email: " + response.Profile.Email;
                                clanText.Text = "Clan: " +
                                                (string.IsNullOrEmpty(response.Profile.Clan)
                                                    ? "none"
                                                    : response.Profile.Clan);

                                if (response.Profile.Team == GameUtils.Team.Human)
                                {
                                    playerCard.SetBackgroundResource(Resource.Color.human);
                                    teamText.Text = "Team: Human";
                                }
                                else
                                {
                                    playerCard.SetBackgroundResource(Resource.Color.zombie);
                                    teamText.Text = "Team: Zombie";
                                }
                                break;

                            case ApiResponse.ResponseStatus.Error:
                                Toast.MakeText(this.Activity, Resource.String.api_err_profile, ToastLength.Short)
                                    .Show();
                                break;
                        }
                    });
                });
            }
        }
    }
}

