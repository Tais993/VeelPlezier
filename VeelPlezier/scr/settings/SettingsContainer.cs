using VeelPlezier.scr.enums;

namespace VeelPlezier.scr.settings
{
    internal sealed class SettingsContainer
    {
        private static readonly SettingsContainer Instance = new();

        /// <summary>
        /// Whenever items should be merged when they're the same type within the Checkout
        /// </summary>
        public bool MergeItemsOfSameTypeInCheckout = false;

        /// <summary>
        /// Whenever items should be merged when they're the same type within the Receipt
        /// </summary>
        public bool MergeItemsOfSameTypeInReceipt = false;

        public TranslationLanguage ReceiptLanguage = TranslationLanguage.English;

        /// <summary>
        /// Returns the default language to be used for receipts
        /// </summary>
        public TranslationLanguage ReceiptTranslationLanguage = null;

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