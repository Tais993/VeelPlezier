using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using JetBrains.Annotations;
using VeelPlezier.scr.enums;
using VeelPlezier.scr.items;
using VeelPlezier.scr.items.objects;
using VeelPlezier.scr.settings;
using VeelPlezier.scr.utilities;
using VeelPlezier.scr.windows;

namespace VeelPlezier.scr.controls
{
    internal sealed partial class MainScreen
    {
        private readonly ItemHandler _itemHandler;

        internal readonly List<PurchasedItem> PurchasedItems = new();
        private readonly Dictionary<string, PurchasedItem> _purchasedItemsDictionary = new();
        private double _totalMoneyGiven;

        private double _totalPriceRequired;

        private ReceiptPrinter _receiptPrinter;
        private CalculatorWindow _calculatorWindow;
        private CurrencyConverterWindow _currencyConverterWindow;

        public MainScreen()
        {
            InitializeComponent();

            TranslationLanguage language =
                Util.LanguageValueOf(Thread.CurrentThread.CurrentCulture.Name.Split('-')[0]) ??
                TranslationLanguage.English;

            _calculatorWindow = new CalculatorWindow(language);
            _currencyConverterWindow = new CurrencyConverterWindow(language);

            _itemHandler = new ItemHandler(ItemsInStore);
            _itemHandler.LoadItemsAsync();
            _itemHandler.ReloadItemsInDisplay(language);
        }

        private void SubmitItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (ItemsInStore.SelectedItem is null || ParseAmountOfItem() == 0)
            {
                return;
            }

            TranslationLanguage language = MainWindow.GetInstance().CurrentTranslationLanguage;

            StackPanel selectedStackPanel = ItemsInStore.SelectedItem as StackPanel
                                            ?? throw new ApplicationException("Something went wrong, " +
                                                                              nameof(ItemsInStore) + " was null");

            Label selectedLabel = selectedStackPanel.Children.OfType<Label>().First();

            string itemName = selectedLabel.Content.ToString();

            Item item = _itemHandler.GetItemByName(itemName) ??
                        throw new ApplicationException("Invalid item, shouldn't happen");

            if (_purchasedItemsDictionary.ContainsKey(itemName))
            {
                if (SettingsContainer.GetInstance().MergeItemsOfSameTypeInCheckout)
                {
                    FindAndMergePurchasedItem(itemName);
                    return;
                }
            }
            else
            {
                _purchasedItemsDictionary.Add(
                    item.GetTranslationByTranslationLanguage(language) ??
                    throw new ApplicationException("Item hasn't implemented said language"),
                    new PurchasedItem(item, ParseAmountOfItem()));
            }


            ItemsPurchased.Items.Add(Util.PanelFromItem(item, ParseAmountOfItem(), language));
            PurchasedItems.Add(new PurchasedItem(item, ParseAmountOfItem()));
            ReloadTotalPrice();
        }


        private void FindAndMergePurchasedItem([NotNull] string itemName)
        {
            PurchasedItem purchasedItem = _purchasedItemsDictionary[itemName];

            foreach (StackPanel panel in from panel
                    in ItemsPurchased.Items.OfType<StackPanel>()
                let currentPanelName = Util.GetLabelByNameFromPanel(panel, "name").Content.ToString()
                where currentPanelName.Equals(itemName)
                select panel)
            {
                MergePurchasedItem(purchasedItem, panel);
            }
        }

        private void MergePurchasedItem([NotNull] PurchasedItem purchasedItem, [NotNull] Panel panel)
        {
            Label amountPurchasedLabel = Util.GetLabelByNameFromPanel(panel, "amountPurchased");

            int amountPurchased = Util.ParseToInt(amountPurchasedLabel.Content.ToString());
            int amountToAdd = ParseAmountOfItem();

            amountPurchasedLabel.Content = amountPurchased + amountToAdd;

            purchasedItem.Amount = amountPurchased + amountToAdd;

            ReloadTotalPrice();
        }

        private void ReloadTotalPrice()
        {
            double totalPrice = 0;

            foreach (StackPanel itemsPurchasedItem in ItemsPurchased.Items)
            {
                string priceString = (from Label label in itemsPurchasedItem.Children
                    where label.Name.Equals("price")
                    select label.Content).First().ToString();

                string amountString = (from Label label in itemsPurchasedItem.Children
                    where label.Name.Equals("amountPurchased")
                    select label.Content).First().ToString();


                double price = Util.ParseToDouble(priceString);
                int amount = (int) Util.ParseToDouble(amountString);

                totalPrice += price * amount;
                _totalPriceRequired = totalPrice * 1.21;
            }

            TotalPriceRequired.Content = $"{_totalPriceRequired:c2}";

            CalculateTotalMoneyReturning();
        }

        private int ParseAmountOfItem()
        {
            double amountOfItem = Util.ParseToDouble(AmountOfItem.Text);

            return (int) Math.Floor(amountOfItem);
        }

        private void moneyChanged_OnHandler(object sender, RoutedEventArgs e)
        {
            if (SubmitButton is null)
                return;

            if (sender is not TextBox textBox)
            {
                throw new ApplicationException("Something triggered that wasn't supposed to be triggered");
            }

            if (textBox.Parent is not WrapPanel parent)
            {
                throw new ApplicationException("Something triggered that wasn't supposed to be triggered");
            }

            double amountOfMoney = Util.ValidateTextToDouble(sender);

            Label timesMoneyLabel = parent.Name.ToLower() switch
            {
                "eurocent5" => Times5EuroCentLabel,
                "eurocent10" => Times10EuroCentLabel,
                "eurocent20" => Times20EuroCentLabel,
                "eurocent50" => Times50EuroCentLabel,
                "euro1" => Times1EuroLabel,
                "euro2" => Times2EuroLabel,
                "euro5" => Times5EuroLabel,
                "euro10" => Times10EuroLabel,
                "euro20" => Times20EuroLabel,
                "euro50" => Times50EuroLabel,
                "euro100" => Times100EuroLabel,
                "euro200" => Times200EuroLabel,
                _ => throw new ApplicationException(
                    "Button that shouldn't exist triggered something that shouldn't be triggered.")
            };

            timesMoneyLabel.Content = amountOfMoney;

            if (timesMoneyLabel.Content.ToString().Equals("0"))
            {
                ((StackPanel) timesMoneyLabel.Parent).Visibility = Visibility.Collapsed;
            }
            else
            {
                ((StackPanel) timesMoneyLabel.Parent).Visibility = Visibility.Visible;
            }


            CalculateTotalMoneyGiven();
        }

        private void CalculateTotalMoneyGiven()
        {
            double totalMoneyGiven = 0;

            foreach (WrapPanel panel in Grid.Children.OfType<WrapPanel>()
                .Where(static panel => panel.Name.Contains("Euro")))
            {
                double toMultiplyBy = panel.Name.ToLower() switch
                {
                    "eurocent5" => 0.05,
                    "eurocent10" => 0.10,
                    "eurocent20" => 0.20,
                    "eurocent50" => 0.50,
                    "euro1" => 1.00,
                    "euro2" => 2.00,
                    "euro5" => 5.00,
                    "euro10" => 10.00,
                    "euro20" => 20.00,
                    "euro50" => 50.00,
                    "euro100" => 100.00,
                    "euro200" => 200.00,
                    _ => throw new ApplicationException("*Confused noises*")
                };

                TextBox count = panel.Children.OfType<TextBox>().First();

                double amountOfTimesGiven = Util.ParseToDouble(count.Text, static _ => { });

                totalMoneyGiven += amountOfTimesGiven * toMultiplyBy;
            }

            _totalMoneyGiven = totalMoneyGiven;

            TotalMoneyGiving.Content = $"{_totalMoneyGiven:c2}";

            CalculateTotalMoneyReturning();
        }

        private void CalculateTotalMoneyReturning()
        {
            double totalMoneyReturning = _totalMoneyGiven - _totalPriceRequired;

            if (totalMoneyReturning < 0)
            {
                TotalChange.Content = $"{totalMoneyReturning:c2}";
                TotalChange.Foreground = Brushes.Red;

                totalMoneyReturning = 0;
            }
            else
            {
                // TotalChange.Content = "124i012421j4oi21j4";
                TotalChange.Content = $"{totalMoneyReturning:c2}";
                TotalChange.Foreground = Brushes.Green;
            }

            totalMoneyReturning = SetAmountReturning(totalMoneyReturning, 200.00, Times200EuroLabel);
            totalMoneyReturning = SetAmountReturning(totalMoneyReturning, 100.00, Times100EuroLabel);
            totalMoneyReturning = SetAmountReturning(totalMoneyReturning, 50.00, Times50EuroLabel);
            totalMoneyReturning = SetAmountReturning(totalMoneyReturning, 20.00, Times20EuroLabel);
            totalMoneyReturning = SetAmountReturning(totalMoneyReturning, 10.00, Times10EuroLabel);
            totalMoneyReturning = SetAmountReturning(totalMoneyReturning, 5.00, Times5EuroLabel);
            totalMoneyReturning = SetAmountReturning(totalMoneyReturning, 2.00, Times2EuroLabel);
            totalMoneyReturning = SetAmountReturning(totalMoneyReturning, 1.00, Times1EuroLabel);
            totalMoneyReturning = SetAmountReturning(totalMoneyReturning, 0.50, Times50EuroCentLabel);
            totalMoneyReturning = SetAmountReturning(totalMoneyReturning, 0.20, Times20EuroCentLabel);
            totalMoneyReturning = SetAmountReturning(totalMoneyReturning, 0.10, Times10EuroCentLabel);
            SetAmountReturning(totalMoneyReturning, 0.05, Times5EuroCentLabel);
        }

        private static double SetAmountReturning(double totalMoneyReturning, double moneyToCheck,
            [NotNull] ContentControl moneyToCheckLabel)
        {
            double returnValue = totalMoneyReturning % moneyToCheck;

            if (moneyToCheckLabel.Parent is not UIElement uiElement)
            {
                throw new ApplicationException("Wha?");
            }

            if (Math.Abs(returnValue - totalMoneyReturning) < 0.05)
            {
                uiElement.Visibility = Visibility.Collapsed;
                return returnValue;
            }

            double amountOfMoney = Math.Floor(totalMoneyReturning / moneyToCheck);

            moneyToCheckLabel.Content = amountOfMoney;
            uiElement.Visibility = Visibility.Visible;

            return returnValue;
        }

        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (TotalChange.Content.ToString().Contains("-"))
            {
                MessageBox.Show("Customer has to pay enough!");
                return;
            }

            _receiptPrinter?.Close();

            _receiptPrinter = new ReceiptPrinter(MainWindow.GetInstance().CurrentTranslationLanguage);

            _receiptPrinter.Show();

            ItemsPurchased.Items.Clear();
            PurchasedItems.Clear();
            _purchasedItemsDictionary.Clear();
            ResetGivenMoneyCounters();
            ResetMoneyChangeLabels();

            _totalMoneyGiven = 0;
            _totalPriceRequired = 0;

            TotalMoneyGiving.Content = $"{_totalMoneyGiven:c2}";
            TotalPriceRequired.Content = $"{_totalPriceRequired:c2}";
            TotalChange.Content = "€ 0,00";

            TotalChange.Foreground = Brushes.Green;
        }


        private void ResetGivenMoneyCounters()
        {
            ResetMoneyGivenWrapPanel(
                EuroCent5,
                EuroCent10,
                EuroCent20,
                EuroCent50,
                Euro1,
                Euro2,
                Euro5,
                Euro10,
                Euro20,
                Euro50,
                Euro100,
                Euro200
            );
        }

        private static void ResetMoneyGivenWrapPanel([NotNull] params Panel[] panels)
        {
            foreach (Panel panel in panels)
            {
                panel.Children.OfType<TextBox>().First().Text = "0";
            }
        }


        private void ResetMoneyChangeLabels()
        {
            ResetChangeLabel(Times5EuroCentLabel);
            ResetChangeLabel(Times10EuroCentLabel);
            ResetChangeLabel(Times20EuroCentLabel);
            ResetChangeLabel(Times50EuroCentLabel);
            ResetChangeLabel(Times1EuroLabel);
            ResetChangeLabel(Times2EuroLabel);
            ResetChangeLabel(Times5EuroLabel);
            ResetChangeLabel(Times10EuroLabel);
            ResetChangeLabel(Times20EuroLabel);
            ResetChangeLabel(Times50EuroLabel);
            ResetChangeLabel(Times100EuroLabel);
            ResetChangeLabel(Times200EuroLabel);
        }

        private static void ResetChangeLabel([NotNull] ContentControl moneyToCheckLabel)
        {
            moneyToCheckLabel.Content = "€ 0";
            ((StackPanel) moneyToCheckLabel.Parent).Visibility = Visibility.Collapsed;
        }


        internal void BecomesVisible()
        {
            SettingsContainer settingsContainer = SettingsContainer.GetInstance();

            if (settingsContainer.MergeItemsOfSameTypeInCheckout)
            {
                var temporaryPurchasedItems = new Dictionary<Item, PurchasedItem>(PurchasedItems.Count);

                foreach (PurchasedItem purchasedItem in PurchasedItems)
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

                ItemCollection itemsPurchasedItems = ItemsPurchased.Items;

                itemsPurchasedItems.Clear();
                PurchasedItems.Clear();
                _purchasedItemsDictionary.Clear();

                foreach (var pair in temporaryPurchasedItems)
                {
                    PurchasedItem purchasedItem = pair.Value;

                    string itemTranslation = pair.Key.GetTranslationByTranslationLanguage(
                                                 MainWindow.GetInstance().CurrentTranslationLanguage) ??
                                             throw new ApplicationException(
                                                 "Item does have an translation for the current item!");

                    PurchasedItems.Add(purchasedItem);
                    _purchasedItemsDictionary.Add(itemTranslation, purchasedItem);

                    Panel panel = Util.PanelFromItem(purchasedItem.Item, purchasedItem.Amount,
                        MainWindow.GetInstance().CurrentTranslationLanguage);

                    itemsPurchasedItems.Add(panel);
                }
            }
        }

        private void Calculator_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_calculatorWindow.IsVisible)
            {
                _calculatorWindow = new CalculatorWindow(MainWindow.GetInstance().CurrentTranslationLanguage);
                _calculatorWindow.Show();
            }
            else
            {
                _calculatorWindow.Focus();
            }
        }

        private void CurrencyConverter_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_currencyConverterWindow.IsVisible)
            {
                _currencyConverterWindow =
                    new CurrencyConverterWindow(MainWindow.GetInstance().CurrentTranslationLanguage);
                _currencyConverterWindow.Show();
            }
            else
            {
                _currencyConverterWindow.Focus();
            }
        }


        private void ValidateTextToNumber_Event([NotNull] object sender, TextChangedEventArgs e)
        {
            Util.ValidateTextToDouble(sender);
        }

        private void ManualMoneyGiving_OnTextChanged([NotNull] object sender, TextChangedEventArgs e)
        {
            _totalMoneyGiven = Util.ValidateTextToDouble(sender);

            TotalMoneyGiving.Content = $"{_totalMoneyGiven:c2}";

            CalculateTotalMoneyReturning();
        }

        internal void SetLanguageDictionary([NotNull] TranslationLanguage language)
        {
            _receiptPrinter?.SetLanguageDictionary(language);
            _calculatorWindow.SetLanguageDictionary(language);
            _currencyConverterWindow.SetLanguageDictionary(language);


            ItemCollection itemsPurchasedItems = ItemsPurchased.Items;

            itemsPurchasedItems.Clear();
            _purchasedItemsDictionary.Clear();

            foreach (PurchasedItem purchasedItem in PurchasedItems)
            {
                string currentItemName = purchasedItem.Item.GetTranslationByTranslationLanguage(language) ??
                                         throw new InvalidOperationException();

                if (!_purchasedItemsDictionary.ContainsKey(currentItemName))
                {
                    _purchasedItemsDictionary.Add(currentItemName, purchasedItem);
                }

                Panel panel = Util.PanelFromItem(purchasedItem.Item, purchasedItem.Amount,
                    MainWindow.GetInstance().CurrentTranslationLanguage);

                itemsPurchasedItems.Add(panel);
            }

            _itemHandler.ReloadItemsInDisplay(language);
        }
    }
}