using System;
using System.Windows;
using System.Windows.Controls;
using VeelPlezier.scr.enums;
using VeelPlezier.scr.settings;
using VeelPlezier.scr.utilities;

namespace VeelPlezier.scr.controls
{
    // ReSharper disable once MemberCanBeInternal
    internal sealed partial class SettingsScreen
    {
        // ReSharper disable once MemberCanBeInternal
        public SettingsScreen()
        {
            InitializeComponent();
        }

        private void MergeItemsSameTypeCheckout_OnChange(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                SettingsContainer settingsContainer = SettingsContainer.GetInstance();

                bool isChecked = checkBox.IsChecked ?? false;

                settingsContainer.MergeItemsOfSameTypeInCheckout = isChecked;
            }
        }

        private void Language_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
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
                    TranslationLanguage translationLanguage = Util.LanguageValueOf(languageCode);

                    MainWindow.GetInstance().SetLanguageDictionary(translationLanguage ?? TranslationLanguage.English);

                    return;
                }
                case null:
                    return;
            }

            throw new ApplicationException("?");
        }

        private void ReceiptLanguage_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ComboBox comboBox)
            {
                throw new ApplicationException("?");
            }

            switch (comboBox.SelectedItem)
            {
                case ComboBoxItem comboBoxItem:
                {
                    string languageCode = comboBoxItem.Name.Split('_')[2];
                    TranslationLanguage translationLanguage = Util.LanguageValueOf(languageCode);

                    SettingsContainer.GetInstance().ReceiptTranslationLanguage = translationLanguage;

                    return;
                }
                case null:
                    return;

                default:
                    throw new ApplicationException("?");
            }
        }
    }
}