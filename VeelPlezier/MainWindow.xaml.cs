using System;
using System.Windows;

// TODO: settings menu

namespace VeelPlezier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once MemberCanBeInternal
    internal sealed partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            SetLanguageDictionary("en");
        }
        
        private void SetLanguageDictionary(string currentLang)
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (currentLang)
            {
                case "nl":
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

        // private void SettingMenuButton_OnClick(object sender, RoutedEventArgs e)
        // {
        //     Thread.CurrentThread.CurrentUICulture = new CultureInfo("nl-NL");
        //
        //     string currentLang = Thread.CurrentThread.CurrentUICulture.Name.Split('-')[0];
        //     
        //     SetLanguageDictionary(currentLang);
        //     _itemHandler.ReloadItemsInDisplay(currentLang);
        //     
        //     foreach (StackPanel itemStackPanel in ItemsPurchased.Items)
        //     {
        //         foreach (Label child in itemStackPanel.Children)
        //         {
        //             if (!child.Name.Equals("name")) continue;
        //             
        //             string translatedName =
        //                 _itemHandler.TranslateNameToCurrentLang(child.Content.ToString(), currentLang);
        //             child.Content = translatedName;
        //         }
        //
        //         string currentName;
        //     }
        // }
    }
}