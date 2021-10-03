using System.Diagnostics.CodeAnalysis;

namespace VeelPlezier.scr.items.objects
{
    [SuppressMessage("ReSharper", "MemberCanBeInternal")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public sealed record Name(string Key, string Value)
    {
        public string Value { get; } = Value;
        public string Key { get; } = Key;
    }
}