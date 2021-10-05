using System;
using System.Windows.Controls;
using VeelPlezier.scr.utilities;
using Xunit;

namespace VeelPlezierTest.Tests.tests.scr.utilities.util
{
    public sealed class GetLabelByNameFromPanel
    {
        [WpfTheory]
        [InlineData(new[]
        {
            "name", "test", "_124521a"
        }, "_124521a", "a")]
        [InlineData(new[]
        {
            "f_26from", "_15i12away", "name"
        }, "f_26from", "&&%!#$@!&*")]
        public void GetLabelByNameFromPanelTest(string[] labelNames, string realName, string fakeName)
        {
            Panel panel = GeneratePanelWithLabels(labelNames);

            Label realLabel = Util.GetLabelByNameFromPanel(panel, realName);
            Assert.NotNull(realLabel);

            Assert.Throws<InvalidOperationException>(() => { Util.GetLabelByNameFromPanel(panel, fakeName); });
        }

        private static Panel GeneratePanelWithLabels(params string[] names)
        {
            Panel panel = new StackPanel();

            foreach (string name in names)
            {
                panel.Children.Add(new Label
                {
                    Name = name
                });
            }

            return panel;
        }
    }
}