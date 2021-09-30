using System.Diagnostics;
using System.Windows.Navigation;
using JetBrains.Annotations;

namespace VeelPlezier.scr.controls
{
    public sealed partial class AboutMeSection
    {
        public AboutMeSection()
        {
            InitializeComponent();
        }

        private void Hyperlink_OnRequestNavigate(object sender, [NotNull] RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
    }
}