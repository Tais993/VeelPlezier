using System.Windows;
using System.Windows.Controls;

namespace VeelPlezier.xaml.controls
{
    internal sealed partial class NavigationBar : UserControl
    {
        public NavigationBar()
        {
            InitializeComponent();
        }

        private void EventSetter_OnHandler(object sender, RoutedEventArgs e)
        {
            MainWindow.MainWindowInstance.SwitchScreen(Util.ScreenTypeValueOf(((Button) sender).Name));
        }
    }
}