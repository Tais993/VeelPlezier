using System;
using System.Windows;
using System.Windows.Controls;
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
                    string language = comboBoxItem.Name.Split('_')[1];

                    MainWindow.MainWindowInstance.SetLanguageDictionary(language);
                    
                    return;
                }

                if (comboBox.SelectedItem == null)
                {
                    return;
                }
            }

            throw new ApplicationException("?");
        }

        private void MergeItemsSameType_OnChange(object sender, RoutedEventArgs e)
        {
            SettingsContainer settingsContainer = SettingsContainer.GetInstance();
        }
    }
}