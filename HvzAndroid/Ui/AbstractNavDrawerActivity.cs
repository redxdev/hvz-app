using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;

namespace Hvz.Ui
{
    public abstract class AbstractNavDrawerActivity : ActionBarActivity
    {
        public abstract DrawerLayout DrawerLayout { get; }

        public abstract ListView DrawerListView { get; }

        public abstract INavDrawerItem[] NavDrawerItems { get; }

        protected ActionBarDrawerToggle DrawerToggle { get; private set; }

        protected abstract void OnNavItemSelected(INavDrawerItem item);

        public void SelectItem(int position)
        {
            var item = this.NavDrawerItems[position];

            this.OnNavItemSelected(item);
            this.DrawerListView.SetItemChecked(position, true);

            if (item.ShouldUpdateActionBarTitle)
            {
                this.SupportActionBar.Title = item.Label;
            }

            if (this.DrawerLayout.IsDrawerOpen(this.DrawerListView))
            {
                this.DrawerLayout.CloseDrawer(this.DrawerListView);
            }
        }

        protected void OnSetupNavDrawer()
        {
            this.DrawerListView.Adapter = new NavDrawerAdaptor(
                this,
                Resource.Layout.nav_menu_item,
                this.NavDrawerItems
            );

            this.DrawerListView.ItemClick += (sender, e) => { this.SelectItem(e.Position); };

            this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            this.SupportActionBar.SetHomeButtonEnabled(true);

            this.DrawerToggle = new NavDrawerToggle(this);
            this.DrawerLayout.SetDrawerListener(this.DrawerToggle);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            DrawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            DrawerToggle.OnConfigurationChanged(newConfig);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (this.DrawerToggle.OnOptionsItemSelected(item))
                return true;

            return base.OnOptionsItemSelected(item);
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Menu)
            {
                if (this.DrawerLayout.IsDrawerOpen(this.DrawerListView))
                {
                    this.DrawerLayout.CloseDrawer(this.DrawerListView);
                }
                else
                {
                    this.DrawerLayout.OpenDrawer(this.DrawerListView);
                }
            }

            return base.OnKeyDown(keyCode, e);
        }

        public override void OnBackPressed()
        {
            if (DrawerLayout.IsDrawerOpen(this.DrawerListView))
            {
                DrawerLayout.CloseDrawer(this.DrawerListView);
            }

            base.OnBackPressed();
        }

        private class NavDrawerToggle : ActionBarDrawerToggle
        {
            private AbstractNavDrawerActivity activity = null;

            public NavDrawerToggle(AbstractNavDrawerActivity activity)
                : base(activity, activity.DrawerLayout, Resource.String.nav_drawer_open, Resource.String.nav_drawer_close)
            {
                this.activity = activity;
            }

            public override void OnDrawerClosed(View drawerView)
            {
                base.OnDrawerClosed(drawerView);
                activity.InvalidateOptionsMenu();
            }

            public override void OnDrawerOpened(View drawerView)
            {
                base.OnDrawerOpened(drawerView);
                activity.InvalidateOptionsMenu();
            }
        }
    }
}

