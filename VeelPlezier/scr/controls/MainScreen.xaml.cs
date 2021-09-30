﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            
            CalculateTotalMoneyReturning();
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
            
            foreach (WrapPanel panel in Grid.Children.OfType<WrapPanel>()
                .Where(panel => panel.Name.Contains("Euro")))
            {
                double toMultiplyBy;
                
                switch (panel.Name.ToLower())
                {
                    case "eurocent5":
                        toMultiplyBy = 0.05;
                        break;
                
                    case "eurocent10":
                        toMultiplyBy = 0.10;
                        break;
                
                    case "eurocent20":
                        toMultiplyBy = 0.20;
                        break;
                
                    case "eurocent50":
                        toMultiplyBy = 0.50;
                        break;
                
                    case "euro1":
                        toMultiplyBy = 1.00;
                        break;
                                
                    case "euro2":
                        toMultiplyBy = 2.00;
                        break;
                                
                    case "euro5":
                        toMultiplyBy = 5.00;
                        break;
                                
                    case "euro10":
                        toMultiplyBy = 10.00;
                        break;
                                
                    case "euro20":
                        toMultiplyBy = 20.00;
                        break;
                                
                    case "euro50":
                        toMultiplyBy = 50.00;
                        break;
                
                    case "euro100":
                        toMultiplyBy = 100.00;
                        break;
                                
                    case "euro200":
                        toMultiplyBy = 200.00;
                        break;
                
                    default:
                        throw new ApplicationException("*Confused noises*");
                }

                TextBox count = panel.Children.OfType<TextBox>().First();

                double amountOfTimesGiven = Util.ParseToDouble(count.Text, exception =>
                {
                    MessageBox.Show("Fake number?");
                });

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

        private static double SetAmountReturning(double totalMoneyReturning, double moneyToCheck, [NotNull] ContentControl moneyToCheckLabel)
        {
            double returnValue = totalMoneyReturning % moneyToCheck;

            if (moneyToCheckLabel.Parent is UIElement uiElement)
            {
                if (Math.Abs(returnValue - totalMoneyReturning) < 1)
                {
                    uiElement.Visibility = Visibility.Collapsed;
                    return returnValue;
                }   

                int amountOfMoney = (int) Math.Floor(totalMoneyReturning / moneyToCheck);

                moneyToCheckLabel.Content = amountOfMoney;
                uiElement.Visibility = Visibility.Visible;
            
                return returnValue; 
            }
            else
            {
                throw new ApplicationException("Wha?");
            }
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