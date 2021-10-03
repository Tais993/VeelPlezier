using System;
using System.Windows;
using JetBrains.Annotations;
using VeelPlezier.scr.enums;

namespace VeelPlezier
{
    /// <inheritdoc cref="System.Windows.Window" />
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once MemberCanBeInternal
    internal sealed partial class MainWindow
    {
        private static MainWindow _instance;
        private ScreenType _currentScreenType = ScreenType.StartScreen;

        public MainWindow()
        {
            _instance = this;
            InitializeComponent();

            SetLanguageDictionary(TranslationLanguage.English);
        }

        public TranslationLanguage CurrentTranslationLanguage { get; private set; }

        public static MainWindow GetInstance()
        {
            return _instance;
        }

        internal void SetLanguageDictionary([NotNull] TranslationLanguage language)
        {
            CurrentTranslationLanguage = language;


            MainScreen.ReceiptPrinter?.SetLanguageDictionary(language);
            MainScreen.CalculatorWindow.SetLanguageDictionary(language);


            ResourceDictionary dict = new ResourceDictionary
            {
                Source = CurrentTranslationLanguage.UriToResource
            };

            Resources.MergedDictionaries.Clear();
            Resources.MergedDictionaries.Add(dict);
        }

        public void SwitchScreen(ScreenType screenType)
        {
            ChangeScreenVisibility(_currentScreenType, Visibility.Collapsed);
            ChangeScreenVisibility(screenType, Visibility.Visible);

            if (screenType == ScreenType.MainScreen)
            {
                MainScreen.BecomesVisible();
            }

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