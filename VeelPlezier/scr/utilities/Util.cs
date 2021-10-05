using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using JetBrains.Annotations;
using VeelPlezier.scr.enums;
using VeelPlezier.scr.items.objects;

namespace VeelPlezier.scr.utilities
{
    public static class Util
    {
        /// <summary>
        /// This regex checks whenever it's a valid number or not
        /// </summary>
        public static readonly Regex NumberValidator = new("[^0-9,.]", RegexOptions.Compiled);

        /// <summary>
        /// Returns the first <see cref="Label"/> with the given name
        /// </summary>
        /// <param name="panel">The <see cref="Panel"/> to grab the <see cref="Label"/> from</param>
        /// <param name="name">The <see cref="Label">Label's</see> name as a <see cref="string"/></param>
        /// <returns></returns>
        [NotNull]
        public static Label GetLabelByNameFromPanel([NotNull] Panel panel, string name)
        {
            return panel.Children.OfType<Label>().First(label => label.Name.Equals(name));
        }

        /// <summary>
        /// Returns the first <see cref="ComboBoxItem"/> with the given name
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox"/> to grab the <see cref="ComboBoxItem"/> from</param>
        /// <param name="name">The <see cref="ComboBoxItem">ComboBoxItem's</see> name as a <see cref="string"/></param>
        /// <returns></returns>
        [NotNull]
        public static ComboBoxItem GetComboBoxItemByContentFromComboBox([NotNull] ComboBox comboBox, string name)
        {
            return comboBox.Items.OfType<ComboBoxItem>().First(item => item.Content.ToString().Equals(name));
        }

        /// <summary>
        /// Returns the <see cref="TranslationLanguage"/> based on it's name
        /// </summary>
        /// <param name="name">The <see cref="TranslationLanguage">Language's</see> name as a <see cref="string">String</see></param>
        /// <returns>The (nullable) relating <see cref="TranslationLanguage"/></returns>
        [CanBeNull]
        public static TranslationLanguage LanguageValueOf([NotNull] string name)
        {
            switch (name.ToLower())
            {
                case "nl":
                case "dutch":
                    return TranslationLanguage.Dutch;
                case "en":
                case "english":
                    return TranslationLanguage.English;

                default:
                    throw new ArgumentOutOfRangeException(nameof(name));
            }
        }


        /// <summary>
        /// Returns the ScreenType based on it's name
        /// </summary>
        /// <param name="name">The ScreenType's name as a <see cref="string">String</see></param>
        /// <returns>The relating <see cref="ScreenType">ScreenType</see></returns>
        public static ScreenType ScreenTypeValueOf([NotNull] string name)
        {
            return (ScreenType) Enum.Parse(typeof(ScreenType), name);
        }

        /// <summary>
        /// Parses the given String to a int
        /// Supports usage of both . and ,
        /// </summary>
        /// <param name="s">The <see cref="string"/> you want to be parsed</param>
        /// <returns>The string parsed to a <see cref="int"/></returns>
        public static int ParseToInt(string s)
        {
            return ParseToInt(s, static _ => { });
        }

        /// <summary>
        /// Parses the given String to a int
        /// Supports usage of both . and ,
        /// </summary>
        /// <param name="s">The <see cref="string"/> you want to be parsed</param>
        /// <param name="onError">The <see cref="Action"/> that will run when something goes wrong</param>
        /// <returns>The string parsed to a <see cref="int"/></returns>
        public static int ParseToInt(string s, [NotNull] Action<Exception> onError)
        {
            try
            {
                return int.Parse(
                    s.Replace('.', ',')
                );
            }
            catch (Exception e)
            {
                onError.Invoke(e);
            }

            return -1;
        }

        /// <summary>
        /// Parses the given String to a double
        /// Supports usage of both . and ,
        /// </summary>
        /// <param name="s">The <see cref="string"/> you want to be parsed</param>
        /// <returns>The string parsed to a <see cref="double"/></returns>
        public static decimal ParseToDecimal(string s)
        {
            return ParseToDecimal(s, static _ => { });
        }

        /// <summary>
        /// Parses the given String to a double
        /// Supports usage of both . and ,
        /// </summary>
        /// <param name="s">The <see cref="string"/> you want to be parsed</param>
        /// <param name="onError">The <see cref="Action"/> that will run when something goes wrong</param>
        /// <returns>The string parsed to a <see cref="double"/></returns>
        public static decimal ParseToDecimal(string s, [NotNull] Action<Exception> onError)
        {
            try
            {
                return decimal.Parse(
                    s.Replace('.', ',')
                );
            }
            catch (Exception e)
            {
                onError.Invoke(e);
            }

            return -1;
        }

        /// <summary>
        /// Parses the given String to a double
        /// Supports usage of both . and ,
        /// </summary>
        /// <param name="s">The <see cref="string"/> you want to be parsed</param>
        /// <returns>The string parsed to a <see cref="double"/></returns>
        public static double ParseToDouble(string s)
        {
            return ParseToDouble(s, static _ => { });
        }

        /// <summary>
        /// Parses the given String to a double
        /// Supports usage of both . and ,
        /// </summary>
        /// <param name="s">The <see cref="string"/> you want to be parsed</param>
        /// <param name="onError">The <see cref="Action"/> that will run when something goes wrong</param>
        /// <returns>The string parsed to a <see cref="double"/></returns>
        public static double ParseToDouble(string s, [NotNull] Action<Exception> onError)
        {
            try
            {
                return double.Parse(
                    s.Replace('.', ',')
                );
            }
            catch (Exception e)
            {
                onError.Invoke(e);
            }

            return -1.00;
        }

        /// <summary>
        /// Generates a StackPanel for a purchased item
        /// </summary>
        /// <param name="item">The <see cref="Item"/> to generate a panel from</param>
        /// <param name="amount">A <see cref="int"/> containing the amount of times the item has been purchased</param>
        /// <param name="language">A <see cref="TranslationLanguage"/> with the current languages shortcode</param>
        /// <returns>The panel generated from the arguments</returns>
        [NotNull]
        public static StackPanel PanelFromItem([NotNull] Item item, int amount, [NotNull] TranslationLanguage language)
        {
            Label nameLabel = new Label
            {
                Name = "name",
                FontSize = 10,
                Content = item.GetTranslationByTranslationLanguage(language)
            };

            Label amountLabel = new Label
            {
                FontSize = 10
            };
            amountLabel.SetResourceReference(ContentControl.ContentProperty, "Amount");

            Label amountNumberLabel = new Label
            {
                Name = "amountPurchased",
                FontSize = 10,
                Content = amount
            };

            Label unitPrice = new Label
            {
                FontSize = 10
            };
            unitPrice.SetResourceReference(ContentControl.ContentProperty, "UnitPrice");

            Label euroIcon = new Label
            {
                FontSize = 10,
                Content = "€"
            };

            Label price = new Label
            {
                Name = "price",
                FontSize = 10,
                Content = item.Price
            };

            StackPanel purchasedItem = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            UIElementCollection purchasedItemCollection = purchasedItem.Children;

            purchasedItemCollection.Add(nameLabel);
            purchasedItemCollection.Add(amountLabel);
            purchasedItemCollection.Add(amountNumberLabel);
            purchasedItemCollection.Add(unitPrice);
            purchasedItemCollection.Add(euroIcon);
            purchasedItemCollection.Add(price);

            return purchasedItem;
        }


        public static double ValidateTextToDouble([NotNull] object sender)
        {
            TextBox textBox = sender as TextBox ??
                              throw new ArgumentException(nameof(sender) + " has to be a " + nameof(TextBox));
            textBox.Text = NumberValidator.Replace(textBox.Text, "");
            return Util.ParseToDouble(textBox.Text);
        }
    }
}