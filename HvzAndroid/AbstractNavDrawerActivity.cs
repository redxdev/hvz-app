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

namespace Hvz
{
    public abstract class AbstractNavDrawerActivity : Activity, ListView.IOnItemClickListener
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
                Resource.Layout.NavMenuItem,
                this.NavDrawerItems
            );

            this.DrawerListView.OnItemClickListener = this;

            this.ActionBar.SetDisplayHomeAsUpEnabled(true);
            this.ActionBar.SetHomeButtonEnabled(true);

            this.DrawerToggle = new ActionBarDrawerToggle(
                this,
                this.DrawerLayout,
                Resource.String.nav_drawer_open,
                Resource.String.nav_drawer_close
            );
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

        public virtual void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            this.SelectItem(position);
        }
    }
}

