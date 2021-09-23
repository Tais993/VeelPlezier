using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using VeelPlezier.objects;

// TODO: settings menu

namespace VeelPlezier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once MemberCanBeInternal
    internal sealed partial class MainWindow : Window
    {
        private readonly ItemHandler _itemHandler;
        
        public MainWindow()
        {
            InitializeComponent();

            _itemHandler = new ItemHandler(ItemsInStore);
            _itemHandler.LoadItemsAsync(Thread.CurrentThread.CurrentCulture);
            
            SetLanguageDictionary();
        }
        
        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "nl-NL":
                    dict.Source = new Uri("..\\Resources\\StringResources.nl-NL.xaml",  
                        UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("..\\resources\\StringResources.xaml", 
                        UriKind.Relative);
                    break;
            }
            
            Resources.MergedDictionaries.Add(dict);
        }

        private void Test_OnClick(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
            SetLanguageDictionary();
            _itemHandler.ReloadItemsInDisplay(Thread.CurrentThread.CurrentCulture);
        }
        
        private void SubmitItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (ItemsInStore.SelectedItem == null || ParseAmountOfItem() == 0)
            {
                // TODO: error handling
                return;
            }
            
            // TODO: possibility merge duplicates of items
            // TODO: reload on language change?
            
            StackPanel selectedStackPanel = ItemsInStore.SelectedItem as StackPanel 
                                            ?? throw new ApplicationException("Something went wrong, " + nameof(ItemsInStore) + " was null");
            
            Label selectedLabel = selectedStackPanel.Children.OfType<Label>().First();
            string selectedItemName = selectedLabel.Content.ToString();

            String name = selectedItemName.Split(' ')[0];
            Item item = _itemHandler.GetItemByName(name);

            Label nameLabel = new Label()
            {
                Content = name + "  "
            };
                
            Label amountLabel = new Label();
            amountLabel.SetResourceReference(ContentProperty, "Amount");
                
            Label amountNumberLabel = new Label
            {
                Content = " " + ParseAmountOfItem() + " "
            };

            Label unitPrice = new Label();
            unitPrice.SetResourceReference(ContentProperty, "UnitPrice");

            Label price = new Label()
            {
                Content = " €" + item.Price
            };

            StackPanel purchasedItem = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            UIElementCollection purchasedItemCollection = purchasedItem.Children;

            purchasedItemCollection.Add(nameLabel);
            purchasedItemCollection.Add(amountLabel);
            purchasedItemCollection.Add(amountNumberLabel);
            purchasedItemCollection.Add(unitPrice);
            purchasedItemCollection.Add(price);
                
            ItemsPurchased.Items.Add(purchasedItem);
        }

        private int ParseAmountOfItem()
        {
            try
            {
                return int.Parse(
                    Regex.Replace(AmountOfItem.Text, "[^0-9.]", "")
                );
            }
            catch (Exception exception)
            {
                return 0;
                // TODO: error handling
            }
        }
    }
}