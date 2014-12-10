using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Hvz.Api;
using Hvz.Ui;

namespace Hvz
{
    [Activity(Label = "Status", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AbstractNavDrawerActivity
    {
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
                    NavMenuItem.Create(2, "Players", this)
            };

            client = new HvzClient() { ApiKey = ApiConfiguration.DevApiKey };

            this.OnSetupNavDrawer();

            replaceFragment(new StatusFragment(client));
        }

        protected override void OnNavItemSelected(INavDrawerItem item)
        {
            switch (item.Id)
            {
                case 1:
                    replaceFragment(new StatusFragment(client));
                    break;

                case 2:
                    replaceFragment(new PlayersFragment(client));
                    break;
            }
        }

        protected void replaceFragment(Fragment fragment)
        {
            var transaction = FragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.content_frame, fragment);
            transaction.Commit();
        }
    }
}


