using System.Diagnostics.CodeAnalysis;

namespace VeelPlezier.scr.items.objects
{
    [SuppressMessage("ReSharper", "MemberCanBeInternal")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    internal sealed record Items(Item[] ItemsArray)
    {
        internal Item[] ItemsArray { get; } = ItemsArray;
    }
}