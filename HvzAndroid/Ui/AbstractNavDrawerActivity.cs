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
    public abstract class AbstractNavDrawerActivity : Activity
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
                this.ActionBar.Title = item.Label;
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

            this.DrawerListView.ItemClick += (sender, e) => {this.SelectItem(e.Position);};

            this.ActionBar.SetDisplayHomeAsUpEnabled(true);
            this.ActionBar.SetHomeButtonEnabled(true);

            this.DrawerToggle = new NavDrawerToggle(this);
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
            return this.DrawerToggle.OnOptionsItemSelected(item);
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

