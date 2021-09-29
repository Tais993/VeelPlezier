using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using JetBrains.Annotations;
using VeelPlezier.objects;

namespace VeelPlezier.xaml.controls
{
    internal sealed partial class MainScreen
    {
        private readonly ItemHandler _itemHandler;

        private double _totalPriceRequired;
        private double _totalMoneyGiven;
        
        public MainScreen()
        {
            InitializeComponent();
            
            _itemHandler = new ItemHandler(ItemsInStore);
            _itemHandler.LoadItemsAsync(Thread.CurrentThread.CurrentUICulture);
        }

        private void SubmitItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (ItemsInStore.SelectedItem == null || ParseAmountOfItem() == 0)
            {
                // TODO: error handling
                return;
            }
            
            // TODO: possibility merge duplicates of items

            string currentLang = Thread.CurrentThread.CurrentUICulture.Name.Split('-')[0];
            
            StackPanel selectedStackPanel = ItemsInStore.SelectedItem as StackPanel 
                                            ?? throw new ApplicationException("Something went wrong, " + nameof(ItemsInStore) + " was null");
            
            Label selectedLabel = selectedStackPanel.Children.OfType<Label>().First();
            Item item = _itemHandler.GetItemByName(selectedLabel.Content.ToString());

            Label nameLabel = new Label
            {
                Name = "name",
                FontSize = 10,
                Content = item.GetTranslationByKey(currentLang) + "  "
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
                Content = " " + ParseAmountOfItem() + " "
            };

            Label unitPrice = new Label
            {
                FontSize = 10
            };
            unitPrice.SetResourceReference(ContentProperty, "UnitPrice");

            Label euroIcon = new Label
            {
                FontSize = 10,
                Content = " €"
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
                
            ItemsPurchased.Items.Add(purchasedItem);

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
                _totalPriceRequired = totalPrice;
            }

            TotalPriceRequired.Content = $"{_totalPriceRequired:c2}";
        }
      
        private int ParseAmountOfItem()
        {
            double amountOfItem = Util.ParseToDouble(AmountOfItem.Text);
            if (amountOfItem == 0)
            {
                // TODO: add error handling
            }

            return (int) amountOfItem;
        }
        
        private void moneyChanged_OnHandler(object sender, RoutedEventArgs e)
        {
            if (SubmitButton == null) 
                return;

            TextBox textBox = sender as TextBox;
            if (textBox == null)
                throw new ApplicationException("Something triggered that wasn't supposed to be triggered");
            
            WrapPanel parent = textBox.Parent as WrapPanel;
            if (parent == null)
                throw new ApplicationException("Something triggered that wasn't supposed to be triggered");


            textBox.Text = Regex.Replace(textBox.Text, "[^0-9,]", "");

            double amountOfMoney = Util.ParseToDouble(textBox.Text);

            Label timesMoneyLabel;

            switch (parent.Name.ToLower())
            {
                case "eurocent5":
                    timesMoneyLabel = Times5EuroCentLabel;
                    break;
                
                case "eurocent10":
                    timesMoneyLabel = Times10EuroCentLabel;
                    break;
                
                case "eurocent20":
                    timesMoneyLabel = Times20EuroCentLabel;
                    break;
                
                case "eurocent50":
                    timesMoneyLabel = Times50EuroCentLabel;
                    break;
                
                case "euro1":
                    timesMoneyLabel = Times1EuroLabel;
                    break;
                                
                case "euro2":
                    timesMoneyLabel = Times2EuroLabel;
                    break;
                                
                case "euro5":
                    timesMoneyLabel = Times5EuroLabel;
                    break;
                                
                case "euro10":
                    timesMoneyLabel = Times10EuroLabel;
                    break;
                                
                case "euro20":
                    timesMoneyLabel = Times20EuroLabel;
                    break;
                                
                case "euro50":
                    timesMoneyLabel = Times50EuroLabel;
                    break;
                
                case "euro100":
                    timesMoneyLabel = Times100EuroLabel;
                    break;
                                
                case "euro200":
                    timesMoneyLabel = Times200EuroLabel;
                    break;
                
                default:
                    throw new ApplicationException("Button that shouldn't exist triggered something that shouldn't be triggered.");
            }
            
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

            totalMoneyGiven += Util.ParseToDouble(Times5EuroCentLabel.Content.ToString()) * 0.05;
            totalMoneyGiven += Util.ParseToDouble(Times10EuroCentLabel.Content.ToString()) * 0.10;
            totalMoneyGiven += Util.ParseToDouble(Times20EuroCentLabel.Content.ToString()) * 0.20;
            totalMoneyGiven += Util.ParseToDouble(Times50EuroCentLabel.Content.ToString()) * 0.50;
            totalMoneyGiven += Util.ParseToDouble(Times1EuroLabel.Content.ToString()) * 1.00;
            totalMoneyGiven += Util.ParseToDouble(Times2EuroLabel.Content.ToString()) * 2.00;
            totalMoneyGiven += Util.ParseToDouble(Times5EuroLabel.Content.ToString()) * 5.00;
            totalMoneyGiven += Util.ParseToDouble(Times10EuroLabel.Content.ToString()) * 10.00;
            totalMoneyGiven += Util.ParseToDouble(Times20EuroLabel.Content.ToString()) * 20.00;
            totalMoneyGiven += Util.ParseToDouble(Times50EuroLabel.Content.ToString()) * 50.00;
            totalMoneyGiven += Util.ParseToDouble(Times100EuroLabel.Content.ToString()) * 100.00;
            totalMoneyGiven += Util.ParseToDouble(Times200EuroLabel.Content.ToString()) * 200.00;

            _totalMoneyGiven = totalMoneyGiven;
            
            TotalMoneyGiving.Content = $"{_totalMoneyGiven:c2}";
        }

        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            ItemsPurchased.Items.Clear();
            ResetGivenMoneyCounters();

            _totalMoneyGiven = 0;
            _totalPriceRequired = 0;
            
            TotalMoneyGiving.Content = $"{_totalMoneyGiven:c2}";
            TotalPriceRequired.Content = $"{_totalPriceRequired:c2}";
            
            // TODO: create "print receipt" screen
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
    }
}