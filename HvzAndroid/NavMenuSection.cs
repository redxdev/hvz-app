using System;

namespace Hvz
{
    public class NavMenuSection : INavDrawerItem
    {
        public static NavMenuSection Create(int id, string label)
        {
            var section = new NavMenuSection() { Id = id, Label = label };
            return section;
        }

        public NavMenuSection()
        {
        }

        public int Id { get; set; }

        public string Label { get; set; }

        public NavDrawerItemType Type { get { return NavDrawerItemType.Section; } }

        public bool Enabled { get { return false; } }

        public bool ShouldUpdateActionBarTitle { get { return false; } }
    }
}

