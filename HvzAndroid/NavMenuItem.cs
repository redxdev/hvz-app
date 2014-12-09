using System;

using Android.Content;

namespace Hvz
{
    public class NavMenuItem : INavDrawerItem
    {
        public static NavMenuItem Create(int id, string label, Context context, bool shouldUpdateActionBarTitle = true)
        {
            var item = new NavMenuItem()
            {
                Id = id,
                Label = label,
                ShouldUpdateActionBarTitle = shouldUpdateActionBarTitle
            };

            return item;
        }

        public NavMenuItem()
        {
        }

        public int Id { get; set; }

        public string Label { get; set; }

        public NavDrawerItemType Type { get { return NavDrawerItemType.Item; } }

        public bool Enabled { get; set; }

        public bool ShouldUpdateActionBarTitle { get; set; }
    }
}

