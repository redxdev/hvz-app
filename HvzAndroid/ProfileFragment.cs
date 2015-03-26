
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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.profile_fragment, container, false);

            playerCard = view.FindViewById<CardView>(Resource.Id.player_card);
            nameText = view.FindViewById<TextView>(Resource.Id.player_name);
            emailText = view.FindViewById<TextView>(Resource.Id.player_email);
            teamText = view.FindViewById<TextView>(Resource.Id.player_team);
            clanText = view.FindViewById<TextView>(Resource.Id.player_clan);

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
                                new AlertDialog.Builder(this.Activity)
                                    .SetTitle("Error")
                                    .SetMessage(Resource.String.api_err_profile)
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

