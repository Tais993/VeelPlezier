using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Navigation;
using JetBrains.Annotations;
using static VeelPlezier.scr.ProjectInfo;

namespace VeelPlezier.scr.controls
{
    internal sealed partial class AboutMeSection
    {
        public AboutMeSection()
        {
            InitializeComponent();

            CreatorLabel.Content = Creator;

            EmailLink.NavigateUri = EmailUri;
            EmailLink.Inlines.Add(Email);

            PhoneLink.NavigateUri = PhoneUri;
            PhoneLink.Inlines.Add(Phone);


            Run githubRun = new Run();
            githubRun.SetResourceReference(Run.TextProperty, "TheGithub");

            GithubLink.NavigateUri = GithubUri;
            GithubLink.Inlines.Add(githubRun);


            Run githubProjectRun = new Run();
            githubProjectRun.SetResourceReference(Run.TextProperty, "TheGithubProject");

            GithubProjectLink.NavigateUri = GithubProjectUri;
            GithubProjectLink.Inlines.Add(githubProjectRun);
        }

        private void Hyperlink_OnRequestNavigate(object sender, [NotNull] RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
    }
}