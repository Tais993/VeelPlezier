namespace VeelPlezier.scr.items.objects
{
    internal class PurchasedItem
    {
        internal readonly Item Item;
        internal int Amount;

        internal PurchasedItem(Item item, int amount)
        {
            Item = item;
            Amount = amount;
        }
    }
}