using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using JetBrains.Annotations;
using VeelPlezier.scr.enums;
using VeelPlezier.scr.utilities;

namespace VeelPlezier.scr.windows
{
    internal sealed partial class CurrencyConverterWindow
    {
        private const decimal EuroDollarCourse = (decimal) 1.1630;
        private const decimal DollarEuroCourse = (decimal) 0.8599;

        private bool _isChangeMadeByCalculator;

        internal CurrencyConverterWindow([NotNull] TranslationLanguage language)
        {
            InitializeComponent();

            ResourceDictionary dict = new ResourceDictionary
            {
                Source = language.UriToCurrencyConverterResource
            };

            Resources.MergedDictionaries.Add(dict);
        }

        public void SetLanguageDictionary([NotNull] TranslationLanguage language)
        {
            ResourceDictionary dict = new ResourceDictionary
            {
                Source = language.UriToCurrencyConverterResource
            };

            Resources.MergedDictionaries.RemoveAt(1);
            Resources.MergedDictionaries.Add(dict);
        }

        private void Selector_OnSelectionChanged([NotNull] object sender, RoutedEventArgs e)
        {
            if (!IsLoaded) return;

            ComboBox changedComboBox =
                sender as ComboBox ?? throw new ApplicationException("Only supported by combo boxes");
            ComboBox oppositeComboBox = GetOppositeComboBox(changedComboBox);

            TextBox relatingTextBox = GetRelatingTextBox(changedComboBox);
            TextBox oppositeTextBox = GetRelatingTextBox(oppositeComboBox);

            Recalculate(changedComboBox, relatingTextBox, oppositeComboBox, oppositeTextBox);
        }

        private void TextBox_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded) return;

            if (_isChangeMadeByCalculator)
            {
                _isChangeMadeByCalculator = false;
                return;
            }

            Util.ValidateTextToDouble(sender);
            TextBox changedTextBox = sender as TextBox ??
                                     throw new ApplicationException("Only valid comboboxes can use this handler!");
            ComboBox relatingComboBox = GetRelatingComboBox(changedTextBox);

            ComboBox oppositeComboBox = GetOppositeComboBox(relatingComboBox);
            TextBox oppositeTextBox = GetRelatingTextBox(oppositeComboBox);

            Recalculate(relatingComboBox, changedTextBox, oppositeComboBox, oppositeTextBox);
        }

        private void Recalculate([NotNull] Selector comboBox, [NotNull] TextBox textBox,
            [NotNull] Selector comboBoxForValueChange, [NotNull] TextBox textBoxForValueChange)
        {
            decimal converter = GetConverterByComboBoxes(comboBox, comboBoxForValueChange);

            decimal toConvert = Util.ParseToDecimal(textBox.Text);

            _isChangeMadeByCalculator = true;

            if (toConvert == 0)
            {
                textBoxForValueChange.Text = "0";
                return;
            }

            decimal converted = toConvert * converter;

            textBoxForValueChange.Text = converted + "";
        }

        private static decimal GetConverterByComboBoxes([NotNull] Selector firstComboBox,
            [NotNull] Selector secondComboBox)
        {
            ComboBoxItem firstComboBoxItem = firstComboBox.SelectedItem as ComboBoxItem ??
                                             throw new ApplicationException("It's impossible to be null");
            ComboBoxItem secondComboBoxItem = secondComboBox.SelectedItem as ComboBoxItem ??
                                              throw new ApplicationException("It's impossible to be null");

            string firstComboBoxContent = firstComboBoxItem.Content.ToString();
            string secondComboBoxContent = secondComboBoxItem.Content.ToString();

            if (firstComboBoxContent.Equals(secondComboBoxContent))
            {
                return 1;
            }

            return firstComboBoxContent switch
            {
                "Euro" => EuroDollarCourse,
                "Dollar" => DollarEuroCourse,
                _ => throw new ApplicationException("Forgot to change the name in code?")
            };
        }

        private TextBox GetRelatingTextBox([NotNull] ComboBox comboBox)
        {
            if (comboBox.Name.Contains("First"))
            {
                return FirstAmount;
            }

            if (comboBox.Name.Contains("Second"))
            {
                return SecondAmount;
            }

            throw new ApplicationException("Invalid " + nameof(comboBox));
        }

        private ComboBox GetRelatingComboBox([NotNull] TextBox textBox)
        {
            if (textBox.Name.Contains("First"))
            {
                return FirstComboBox;
            }

            if (textBox.Name.Contains("Second"))
            {
                return SecondComboBox;
            }

            throw new ApplicationException("Invalid " + nameof(textBox));
        }

        private ComboBox GetOppositeComboBox([NotNull] ComboBox comboBox)
        {
            if (comboBox.Name.Contains("First"))
            {
                return SecondComboBox;
            }

            if (comboBox.Name.Contains("Second"))
            {
                return FirstComboBox;
            }

            throw new ApplicationException("Invalid " + nameof(comboBox));
        }
    }
}