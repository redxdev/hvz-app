using System;

namespace Hvz
{
    public enum NavDrawerItemType
    {
        Section,
        Item
    }

    public interface INavDrawerItem
    {
        int Id { get; }

        string Label { get; }

        NavDrawerItemType Type { get; }

        bool Enabled { get; }

        bool ShouldUpdateActionBarTitle { get; }
    }
}

