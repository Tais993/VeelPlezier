using System.Collections.Generic;

namespace VeelPlezier.scr.items.objects
{
    public sealed record Items(IEnumerable<Item> ItemsArray)
    {
        public IEnumerable<Item> ItemsArray { get; } = ItemsArray;
    }
}