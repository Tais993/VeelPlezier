using System;
using System.Windows;
using System.Windows.Controls;
using VeelPlezier.enums;
using VeelPlezier.scr.settings;

namespace VeelPlezier.xaml.controls
{
    // ReSharper disable once MemberCanBeInternal
    public sealed partial class SettingsScreen
    {
        // ReSharper disable once MemberCanBeInternal
        public SettingsScreen()
        {
            InitializeComponent();
        }
        
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                if (comboBox.SelectedItem is ComboBoxItem comboBoxItem)
                {
                    string languageCode = comboBoxItem.Name.Split('_')[1];
                    TranslationLanguage translationLanguage = Util.LanguageValueOf(languageCode);

                    MainWindow.MainWindowInstance.SetLanguageDictionary(translationLanguage ?? TranslationLanguage.English);
                    
                    return;
                }

                if (comboBox.SelectedItem == null)
                {
                    return;
                }
            }

            throw new ApplicationException("?");
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
        
        private void MergeItemsSameTypeReceipt_OnChange(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                SettingsContainer settingsContainer = SettingsContainer.GetInstance();

                bool isChecked = checkBox.IsChecked ?? false;
                
                settingsContainer.MergeItemsOfSameTypeInReceipt = isChecked;
            }
        }
    }
}