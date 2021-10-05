using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Castle.Core.Internal;
using JetBrains.Annotations;
using VeelPlezier.scr.enums;
using VeelPlezier.scr.items.objects;
using VeelPlezier.scr.settings;
using VeelPlezier.scr.utilities;

namespace VeelPlezier.scr.controls
{
    internal sealed partial class ReceiptPrinter
    {
        private static readonly Random Random = new();
        private static readonly double TaxPercentage = 0.21;

        private static int _transactionNumber;

        private readonly List<PurchasedItem> _purchasedItems = new();

        private TranslationLanguage _currentLanguage;


        internal ReceiptPrinter([NotNull] TranslationLanguage language)
        {
            _currentLanguage = language;

            MainScreen mainScreen = MainWindow.GetInstance().MainScreen;

            InitializeComponent();

            TransactionIdLabel.Content = _transactionNumber++ + "";
            AccountIdLabel.Content = Random.Next(100);

            CashierIdLabel.Content = "0";

            DateLabel.Content = DateTime.Today.ToShortDateString();
            TimeLabel.Content = DateTime.Now.ToShortTimeString();

            SetLanguageDictionary(language);


            SettingsContainer settingsContainer = SettingsContainer.GetInstance();

            TranslationLanguage receiptLanguage = settingsContainer.ReceiptTranslationLanguage;
            if (receiptLanguage is not null)
            {
                ChangeReceiptLanguage(receiptLanguage);
            }

            if (!mainScreen.PurchasedItems.IsNullOrEmpty())
            {
                _purchasedItems = new List<PurchasedItem>(mainScreen.PurchasedItems);
                GenerateReceipt();
            }

            if (settingsContainer.MergeItemsOfSameTypeInReceipt)
            {
                MergeItems();
            }
        }

        private void MergeItems()
        {
            var temporaryPurchasedItems = new Dictionary<Item, PurchasedItem>(_purchasedItems.Count);

            foreach (PurchasedItem purchasedItem in _purchasedItems)
            {
                Item currentItem = purchasedItem.Item;

                if (temporaryPurchasedItems.ContainsKey(currentItem))
                {
                    PurchasedItem oldPurchasedItem = temporaryPurchasedItems[purchasedItem.Item];

                    int amount = oldPurchasedItem.Amount + purchasedItem.Amount;

                    temporaryPurchasedItems.Remove(currentItem);

                    temporaryPurchasedItems.Add(purchasedItem.Item, new PurchasedItem(purchasedItem.Item, amount));
                }
                else
                {
                    temporaryPurchasedItems.Add(purchasedItem.Item, purchasedItem);
                }
            }

            _purchasedItems.Clear();

            foreach (var pair in temporaryPurchasedItems)
            {
                _purchasedItems.Add(pair.Value);
            }

            GenerateReceipt();
        }

        private void GenerateReceipt()
        {
            double subtotal = 0;
            double tax = 0;
            double total;

            ItemNames.Children.Clear();
            ItemUnitPrices.Children.Clear();
            ItemTotalPrices.Children.Clear();

            foreach (PurchasedItem purchasedItem in _purchasedItems)
            {
                Item item = purchasedItem.Item;


                ItemNames.Children.Add(new Label
                {
                    Content = item.GetTranslationByTranslationLanguage(_currentLanguage)
                });

                ItemUnitPrices.Children.Add(new Label
                {
                    Content = purchasedItem.Amount + " x " + item.Price
                });

                double totalPrice = purchasedItem.Amount * Util.ParseToDouble(item.Price);

                subtotal += totalPrice;

                ItemTotalPrices.Children.Add(new Label
                {
                    Content = $"{totalPrice:C2}"
                });
            }

            tax = subtotal * TaxPercentage;
            total = tax + subtotal;

            SubTotalPriceLabel.Content = $"{subtotal:c2}";
            TaxLabel.Content = $"{tax:c2}";
            TotalPriceLabel.Content = $"{total:c2}";
        }


        internal void SetLanguageDictionary([NotNull] TranslationLanguage language)
        {
            _currentLanguage = language;

            ResourceDictionary normalResource = new ResourceDictionary
            {
                Source = language.UriToResource
            };

            ResourceDictionary receiptResource = new ResourceDictionary
            {
                Source = language.UriToReceiptResource
            };

            ResourceDictionary styleResource = Resources.MergedDictionaries[0];


            if (language.Equals(TranslationLanguage.Dutch))
            {
                Language_Nl.IsSelected = true;
            }
            else
            {
                Language_En.IsSelected = true;
            }

            Resources.MergedDictionaries.Add(styleResource);
            Resources.MergedDictionaries.Add(normalResource);
            Resources.MergedDictionaries.Add(receiptResource);


            GenerateReceipt();
        }

        private void PrintButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                switch (button.Name)
                {
                    case "DontPrintButton":
                        DontPrintExit();
                        break;

                    case "PrintButton":
                        PrintExit();
                        break;
                }
            }
        }

        private void DontPrintExit()
        {
            Close();
        }

        private void PrintExit()
        {
            Close();
        }

        private void Selector_OnSelectionChanged([NotNull] object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ComboBox comboBox)
            {
                throw new ApplicationException("?");
            }

            switch (comboBox.SelectedItem)
            {
                case ComboBoxItem comboBoxItem:
                {
                    string languageCode = comboBoxItem.Name.Split('_')[1];
                    TranslationLanguage language =
                        Util.LanguageValueOf(languageCode) ?? throw new ApplicationException();

                    ChangeReceiptLanguage(language);
                    return;
                }
                case null:
                    return;
            }
        }

        private void ChangeReceiptLanguage([NotNull] TranslationLanguage language)
        {
            ResourceDictionary oldReceiptResource = new ResourceDictionary
            {
                Source = _currentLanguage.UriToReceiptResource
            };
            Resources.MergedDictionaries.Remove(oldReceiptResource);


            _currentLanguage = language;


            ResourceDictionary receiptResource = new ResourceDictionary
            {
                Source = language.UriToReceiptResource
            };

            Resources.MergedDictionaries.Add(receiptResource);

            GenerateReceipt();
        }
    }
}