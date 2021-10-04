using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace VeelPlezier.scr.items.objects
{
    [SuppressMessage("ReSharper", "MemberCanBeInternal")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public sealed record Items(IEnumerable<Item> ItemsArray)
    {
        public IEnumerable<Item> ItemsArray { get; } = ItemsArray;
    }
}