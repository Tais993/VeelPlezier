using System.Windows.Controls;
using VeelPlezier.scr.utilities;
using Xunit;

namespace VeelPlezierTest.Tests.tests.scr.utilities.util
{
    public sealed class ValidateTextToNumber
    {
        [WpfTheory]
        [InlineData("away0425,259827421", 0425.259827421)]
        [InlineData("241far4", 2414)]
        [InlineData("259827darkness421.1", 259827421.1)]
        public void ValidateTextToNumberSuccessTest(string numberString, double number)
        {
            TextBox textBox = new TextBox
            {
                Text = numberString
            };

            Assert.Equal(Util.ValidateTextToDouble(textBox), number);
        }

        [WpfTheory]
        [InlineData("yeet413", 214)]
        public void ValidateTextToNumberFailureTest(string numberString, double number)
        {
            TextBox textBox = new TextBox
            {
                Text = numberString
            };

            Assert.NotEqual(Util.ValidateTextToDouble(textBox), number);
        }
    }
}