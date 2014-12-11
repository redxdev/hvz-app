using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Hvz.Api;
using Hvz.Ui;

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

        public override DrawerLayout DrawerLayout { get { return drawerLayout; } }

        public override ListView DrawerListView { get { return drawerListView; } }

        public override INavDrawerItem[] NavDrawerItems { get { return navDrawerItems; } }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

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
                    NavMenuItem.Create(6, "Rules", this),
                    NavMenuItem.Create(7, "Settings", this)
            };

            var settings = GetSharedPreferences(PrefsName, 0);
            var apiKey = settings.GetString("apikey", string.Empty);

            Log.Debug("HvzAndroid", "Using api key \"" + apiKey + "\"");

            client = new HvzClient() { ApiKey = apiKey };

            this.OnSetupNavDrawer();

            ReplaceFragment(new StatusFragment(client));

            this.ActionBar.Title = "Status";
        }

        protected override void OnNavItemSelected(INavDrawerItem item)
        {
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
                    ReplaceFragment(new RulesetsFragment(client));
                    break;

                case 7:
                    ReplaceFragment(new SettingsFragment(client));
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


