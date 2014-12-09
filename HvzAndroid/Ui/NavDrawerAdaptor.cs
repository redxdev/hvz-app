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

namespace Hvz.Ui
{		
    public class NavDrawerAdaptor : ArrayAdapter<INavDrawerItem>
    {
        private LayoutInflater inflater = null;

        public NavDrawerAdaptor(Context context, int textViewResourceId, INavDrawerItem[] objects)
            : base(context, textViewResourceId, objects)
        {
            this.inflater = LayoutInflater.From(context);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            INavDrawerItem item = this.GetItem(position);

            switch (item.Type)
            {
                case NavDrawerItemType.Item:
                    return GetItemView(convertView, parent, (NavMenuItem)item);

                case NavDrawerItemType.Section:
                    return GetSectionView(convertView, parent, (NavMenuSection)item);
            }

            return null;
        }

        public View GetItemView(View convertView, ViewGroup parentView, NavMenuItem item)
        {
            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.NavMenuItem, parentView, false);
            }

            TextView label = convertView.FindViewById<TextView>(Resource.Id.nav_menu_item_label);
            label.Text = item.Label;

            return convertView;
        }

        public View GetSectionView(View convertView, ViewGroup parentView, NavMenuSection item)
        {
            if(convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.NavMenuSection, parentView, false);
            }

            TextView label = convertView.FindViewById<TextView>(Resource.Id.nav_menu_section_label);
            label.Text = item.Label;

            return convertView;
        }

        public override int GetItemViewType(int position)
        {
            return (int)this.GetItem(position).Type;
        }

        public override bool IsEnabled(int position)
        {
            return this.GetItem(position).Enabled;
        }
    }
}

