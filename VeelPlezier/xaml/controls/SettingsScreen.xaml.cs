using System;
using System.Windows.Controls;

namespace VeelPlezier.xaml.controls
{
    public sealed partial class SettingsScreen : UserControl
    {
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
    }
}