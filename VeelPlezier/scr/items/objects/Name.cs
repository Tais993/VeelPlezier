namespace VeelPlezier.scr.items.objects
{
    public sealed record Name(string Key, string Value)
    {
        public string Value { get; } = Value;
        public string Key { get; } = Key;
    }
}