using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Hvz.Api;
using Hvz.Api.Response;
using Hvz.Ui;
using Xamarin;

namespace Hvz
{
    [Activity(Label = "HvZ @ RIT", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AbstractNavDrawerActivity
    {
        public const string PrefsName = "HvzAndroidPrefs";

        private HvzClient client = null;

        private DrawerLayout drawerLayout = null;

        private ListView drawerListView = null;

        private INavDrawerItem[] navDrawerItems = null;

        private int currentNavId = 1;

        public override DrawerLayout DrawerLayout { get { return drawerLayout; } }

        public override ListView DrawerListView { get { return drawerListView; } }

        public override INavDrawerItem[] NavDrawerItems { get { return navDrawerItems; } }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            if (HvzClient.Instance == null)
            {
                client = new HvzClient();
            }
            else
            {
                client = HvzClient.Instance;
            }

            this.RequestedOrientation = ScreenOrientation.Portrait;

            if (!Insights.IsInitialized)
            {
                Insights.Initialize(AppConfig.InsightsApiKey, ApplicationContext);
            }

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.main);

            this.drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            this.drawerListView = FindViewById<ListView>(Resource.Id.left_drawer);
            this.navDrawerItems = new INavDrawerItem[]
            {
                    NavMenuSection.Create(0, "Humans vs Zombies"),
                    NavMenuItem.Create(1, "Status", this),
                    NavMenuItem.Create(2, "Players", this),
                    NavMenuItem.Create(3, "Infections", this),
                    NavMenuItem.Create(4, "Missions", this),
                    NavMenuItem.Create(5, "Register Infection", this),
                    NavMenuItem.Create(6, "Antivirus", this),
                    NavMenuItem.Create(7, "Profile", this),
                    NavMenuItem.Create(8, "Rules", this),
                    NavMenuItem.Create(9, "Settings", this),
                    NavMenuItem.Create(10, "Contact the Admins", this, true, false)
            };

            var settings = GetSharedPreferences(PrefsName, 0);
            var apiKey = settings.GetString("apikey", string.Empty);

            Log.Debug("HvzAndroid", "Using api key \"" + apiKey + "\"");

            client.ApiKey = apiKey;

            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                client.GetProfile(response =>
                {
                    if (response.Status == ApiResponse.ResponseStatus.Ok)
                    {
                        Insights.Identify(apiKey, new Dictionary<string, string>
                        {
                            {Insights.Traits.Name, response.Profile.FullName},
                            {Insights.Traits.Email, response.Profile.Email},
                            {"Team", response.Profile.Team.ToString()}
                        });
                    }
                });
            }

            this.OnSetupNavDrawer();

            if (bundle == null)
            {
                ReplaceFragment(new StatusFragment(client));

                this.SupportActionBar.Title = "Status";
            }
            else
            {
                INavDrawerItem item = this.navDrawerItems[bundle.GetInt("current_nav_id", 1)];
                if (item.Type == NavDrawerItemType.Item)
                {
                    currentNavId = item.Id;
                    this.SupportActionBar.Title = item.Label;
                }
                else
                {
                    ReplaceFragment(new StatusFragment(client));

                    this.SupportActionBar.Title = "Status";
                }
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutInt("current_nav_id", currentNavId);
        }

        protected override void OnNavItemSelected(INavDrawerItem item)
        {
            currentNavId = item.Id;
            switch (item.Id)
            {
                case 1:
                    ReplaceFragment(new StatusFragment(client));
                    break;

                case 2:
                    ReplaceFragment(new PlayersFragment(client));
                    break;

                case 3:
                    ReplaceFragment(new InfectionsFragment(client));
                    break;

                case 4:
                    ReplaceFragment(new MissionsFragment(client));
                    break;

                case 5:
                    ReplaceFragment(new RegisterInfectionFragment(client));
                    break;

                case 6:
                    ReplaceFragment(new AntivirusFragment(client));
                    break;

                case 7:
                    ReplaceFragment(new ProfileFragment(client));
                    break;

                case 8:
                    ReplaceFragment(new RulesetsFragment(client));
                    break;

                case 9:
                    ReplaceFragment(new SettingsFragment(client));
                    break;

                case 10:
                    var email = new Intent(Android.Content.Intent.ActionSendto);
                    email.PutExtra(Android.Content.Intent.ExtraEmail, new string[] {"hvz@rit.edu"});
                    email.SetData(Android.Net.Uri.Parse("mailto:"));
                    if (email.ResolveActivity(PackageManager) != null)
                    {
                        StartActivity(email);
                    }
                    else
                    {
                        new AlertDialog.Builder(this)
                            .SetTitle("Error")
                            .SetMessage("It doesn't look like you have an email client :(")
                            .SetPositiveButton("OK", (s, a) => { })
                            .Show();
                    }
                    break;
            }
        }

        private void ReplaceFragment(Fragment fragment)
        {
            var transaction = FragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.content_frame, fragment);
            transaction.Commit();
        }
    }
}


