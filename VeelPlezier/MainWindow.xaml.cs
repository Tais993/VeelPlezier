using System;
using System.Windows;
using JetBrains.Annotations;
using VeelPlezier.enums;

namespace VeelPlezier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once MemberCanBeInternal
    internal sealed partial class MainWindow
    {
        private ScreenType _currentScreenType = ScreenType.StartScreen;

        public static MainWindow MainWindowInstance { get; private set; }

        public MainWindow()
        {
            MainWindowInstance = this;
            InitializeComponent();

            SetLanguageDictionary("en");
        }

        internal void SetLanguageDictionary([NotNull] string currentLang)
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (currentLang.ToLower())
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

        public void SwitchScreen(ScreenType screenType)
        {
            ChangeScreenVisibility(_currentScreenType, Visibility.Collapsed);
            ChangeScreenVisibility(screenType, Visibility.Visible);

            _currentScreenType = screenType;
        }

        private void ChangeScreenVisibility(ScreenType screenType, Visibility visibility)
        {
            switch (screenType)
            {
                case ScreenType.StartScreen:
                    StartScreen.Visibility = visibility;
                    break;
                
                case ScreenType.MainScreen:
                    MainScreen.Visibility = visibility;
                    break;
                
                case ScreenType.SettingsScreen:
                    SettingsScreen.Visibility = visibility;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(screenType), screenType, null);
            }
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