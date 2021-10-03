using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

namespace VeelPlezier.scr.controls
{
    internal sealed partial class MainScreen
    {
        private readonly ItemHandler _itemHandler;

        public readonly List<PurchasedItem> PurchasedItems = new();
        private readonly Dictionary<string, PurchasedItem> _purchasedItemsDictionary = new();
        private double _totalMoneyGiven;

        private double _totalPriceRequired;
        
        internal ReceiptPrinter ReceiptPrinter { get; private set; }
        internal CalculatorWindow CalculatorWindow = new(TranslationLanguage.English);

        public MainScreen()
        {
            InitializeComponent();

            _itemHandler = new ItemHandler(ItemsInStore);
            _itemHandler.LoadItemsAsync(Thread.CurrentThread.CurrentUICulture);
        }

        private void SubmitItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (ItemsInStore.SelectedItem is null || ParseAmountOfItem() == 0)
            {
                // TODO: error handling
                return;
            }

            string currentLang = Thread.CurrentThread.CurrentUICulture.Name.Split('-')[0];

            StackPanel selectedStackPanel = ItemsInStore.SelectedItem as StackPanel
                                            ?? throw new ApplicationException("Something went wrong, " +
                                                                              nameof(ItemsInStore) + " was null");

            Label selectedLabel = selectedStackPanel.Children.OfType<Label>().First();

            string itemName = selectedLabel.Content.ToString();

            Item item = _itemHandler.GetItemByName(itemName);

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
                _purchasedItemsDictionary.Add(item.GetTranslationByKey(currentLang),
                    new PurchasedItem(item, ParseAmountOfItem()));
            }


            ItemsPurchased.Items.Add(PanelFromItem(item, ParseAmountOfItem(), currentLang));
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

        [NotNull]
        private static StackPanel PanelFromItem([NotNull] Item item, int amount, string currentLang)
        {
            Label nameLabel = new Label
            {
                Name = "name",
                FontSize = 10,
                Content = item.GetTranslationByKey(currentLang)
            };

            Label amountLabel = new Label
            {
                FontSize = 10
            };
            amountLabel.SetResourceReference(ContentProperty, "Amount");

            Label amountNumberLabel = new Label
            {
                Name = "amountPurchased",
                FontSize = 10,
                Content = amount
            };

            Label unitPrice = new Label
            {
                FontSize = 10
            };
            unitPrice.SetResourceReference(ContentProperty, "UnitPrice");

            Label euroIcon = new Label
            {
                FontSize = 10,
                Content = "€"
            };

            Label price = new Label
            {
                Name = "price",
                FontSize = 10,
                Content = item.Price
            };

            StackPanel purchasedItem = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            UIElementCollection purchasedItemCollection = purchasedItem.Children;

            purchasedItemCollection.Add(nameLabel);
            purchasedItemCollection.Add(amountLabel);
            purchasedItemCollection.Add(amountNumberLabel);
            purchasedItemCollection.Add(unitPrice);
            purchasedItemCollection.Add(euroIcon);
            purchasedItemCollection.Add(price);

            return purchasedItem;
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
                _totalPriceRequired = totalPrice;
            }

            TotalPriceRequired.Content = $"{_totalPriceRequired:c2}";

            CalculateTotalMoneyReturning();
        }

        private int ParseAmountOfItem()
        {
            double amountOfItem = Util.ParseToDouble(AmountOfItem.Text, static _ =>
            {
                // TODO: add error handling        
            });

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

            textBox.Text = Regex.Replace(textBox.Text, "[^0-9,]", "");

            double amountOfMoney = Util.ParseToDouble(textBox.Text);

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
                TotalMoneyReturning.Content = $"{totalMoneyReturning:c2}";
                TotalMoneyReturning.Foreground = Brushes.Red;

                totalMoneyReturning = 0;
            }
            else
            {
                TotalMoneyReturning.Content = $"{totalMoneyReturning:c2}";
                TotalMoneyReturning.Foreground = Brushes.Green;
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
            totalMoneyReturning = SetAmountReturning(totalMoneyReturning, 0.05, Times5EuroCentLabel);
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
            ReceiptPrinter?.Close();

            ReceiptPrinter = new ReceiptPrinter(MainWindow.GetInstance().CurrentTranslationLanguage);

            ReceiptPrinter.Show();

            ItemsPurchased.Items.Clear();
            PurchasedItems.Clear();
            _purchasedItemsDictionary.Clear();
            ResetGivenMoneyCounters();
            ResetMoneyChangeLabels();

            _totalMoneyGiven = 0;
            _totalPriceRequired = 0;

            TotalMoneyGiving.Content = $"{_totalMoneyGiven:c2}";
            TotalPriceRequired.Content = $"{_totalPriceRequired:c2}";
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
            moneyToCheckLabel.Visibility = Visibility.Collapsed;
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

                    PurchasedItems.Add(purchasedItem);
                    _purchasedItemsDictionary.Add(
                        pair.Key.GetTranslationByTranslationLanguage(
                            MainWindow.GetInstance().CurrentTranslationLanguage)!, purchasedItem);

                    Panel panel = PanelFromItem(purchasedItem.Item, purchasedItem.Amount,
                        MainWindow.GetInstance().CurrentTranslationLanguage.LanguageShortCode);

                    itemsPurchasedItems.Add(panel);
                }
            }
        }

        private void Calculator_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CalculatorWindow.IsVisible)
            {
                CalculatorWindow = new CalculatorWindow(MainWindow.GetInstance().CurrentTranslationLanguage);
                CalculatorWindow.Show();
            }
            else
            {
                CalculatorWindow.Focus();
            }
        }
    }
}