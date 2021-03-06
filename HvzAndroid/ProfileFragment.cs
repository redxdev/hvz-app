
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
using ZXing;
using ZXing.Common;

namespace Hvz
{
    public class ProfileFragment : Fragment
    {
        private HvzClient client = null;

        private CardView playerCard = null;

        private LinearLayout playerCardLayout = null;

        private TextView nameText = null;

        private TextView emailText = null;

        private TextView teamText = null;

        private TextView clanText = null;

        private TextView humanId1Text = null;

        private TextView humanId2Text = null;

        private TextView zombieIdText = null;

        private ImageView qrImageView = null;

        public ProfileFragment()
        {
            this.client = HvzClient.Instance;
            RetainInstance = true;
        }

        public ProfileFragment(HvzClient client)
        {
            this.client = client;
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.profile_fragment, container, false);

            playerCard = view.FindViewById<CardView>(Resource.Id.player_card);
            playerCardLayout = view.FindViewById<LinearLayout>(Resource.Id.player_card_layout);
            nameText = view.FindViewById<TextView>(Resource.Id.player_name);
            emailText = view.FindViewById<TextView>(Resource.Id.player_email);
            teamText = view.FindViewById<TextView>(Resource.Id.player_team);
            clanText = view.FindViewById<TextView>(Resource.Id.player_clan);

            humanId1Text = view.FindViewById<TextView>(Resource.Id.human_id_1);
            humanId2Text = view.FindViewById<TextView>(Resource.Id.human_id_2);
            zombieIdText = view.FindViewById<TextView>(Resource.Id.zombie_id);
            qrImageView = view.FindViewById<ImageView>(Resource.Id.qr_image);

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
                                                (string.IsNullOrWhiteSpace(response.Profile.Clan)
                                                    ? "none"
                                                    : response.Profile.Clan);

                                if (response.Profile.Team == GameUtils.Team.Human)
                                {
                                    // removed: breaks stuff on older devices
                                    //playerCard.SetBackgroundResource(Resource.Color.human);
                                    playerCardLayout.SetBackgroundResource(Resource.Color.human);
                                    teamText.Text = "Team: Human";
                                }
                                else
                                {
                                    // removed: breaks stuff on older devices
                                    //playerCard.SetBackgroundResource(Resource.Color.zombie);
                                    playerCardLayout.SetBackgroundResource(Resource.Color.zombie);
                                    teamText.Text = "Team: Zombie";
                                }

                                zombieIdText.Text = "Zombie id: " + response.Profile.ZombieId;
                                if (response.Profile.HumanIds.Count >= 1)
                                    humanId1Text.Text = "Human id: " + response.Profile.HumanIds[0].Id;

                                if (response.Profile.HumanIds.Count >= 2)
                                    humanId2Text.Text = "Human id: " + response.Profile.HumanIds[1].Id;

                                var writer = new ZXing.BarcodeWriter
                                {
                                    Format = BarcodeFormat.QR_CODE,
                                    Options = new EncodingOptions
                                    {
                                        Height = 400,
                                        Width = 400
                                    }
                                };

                                var bitmap = writer.Write(response.Profile.QRData);
                                qrImageView.SetImageBitmap(bitmap);
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

