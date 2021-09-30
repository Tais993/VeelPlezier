using VeelPlezier.enums;

namespace VeelPlezier.scr.settings
{
    internal sealed class SettingsContainer
    {
        private static readonly SettingsContainer Instance = new SettingsContainer();

        /// <summary>
        /// Whenever items should be merged when they're the same type within the Checkout
        /// </summary>
        public bool MergeItemsOfSameTypeInCheckout = false;
        
        /// <summary>
        /// Whenever items should be merged when they're the same type within the receipt
        /// </summary>
        public bool MergeItemsOfSameTypeInReceipt = false;

        /// <summary>
        /// Returns the default language to be used for receipts
        /// </summary>
        public Language ReceiptLanguage = Language.English;
        
        static SettingsContainer()
        {
        }
        
        private SettingsContainer()
        {
        }

        /// <summary>
        /// Returns the current <see cref="SettingsContainer"/> instance
        /// </summary>
        /// <returns>The <see cref="SettingsContainer"/> instance</returns>
        internal static SettingsContainer GetInstance()
        {
            return Instance;
        }
    }
}