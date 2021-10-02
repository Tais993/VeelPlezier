using System.Windows;
using System.Windows.Controls;
using VeelPlezier.scr.utilities;

namespace VeelPlezier.scr.controls
{
    internal sealed partial class NavigationBar
    {
        public NavigationBar()
        {
            InitializeComponent();
        }

        private void EventSetter_OnHandler(object sender, RoutedEventArgs e)
        {
            MainWindow.GetInstance().SwitchScreen(Util.ScreenTypeValueOf(((Button) sender).Name));
        }
    }
}