using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using JetBrains.Annotations;
using Newtonsoft.Json;
using VeelPlezier.scr.items.objects;

namespace VeelPlezier.scr.items
{
    public class ItemHandler
    {
        private readonly ComboBox _itemsInStore;
        public Items Items;

        protected internal ItemHandler(ComboBox itemsInStore)
        {
            _itemsInStore = itemsInStore;
        }

        internal void LoadItemsAsync([NotNull] CultureInfo cultureInfo)
        {
            string path =
                Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                    throw new InvalidOperationException(), @"resources/items/items.json");
            string json = File.ReadAllText(path);
            Items = JsonConvert.DeserializeObject<Items>(json);

            ReloadItemsInDisplay(cultureInfo.Name.Split('-')[0]);
        }

        public bool ReloadItemsInDisplay(string currentLang)
        {
            int size = _itemsInStore.Items.Count;
            _itemsInStore.Items.Clear();

            foreach (Item item in Items.ItemsArray)
            {
                StackPanel stackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };

                UIElementCollection stackPanelChildren = stackPanel.Children;

                Label titleLabel = new Label
                {
                    Content = item.GetTranslationByKey(currentLang)
                };

                Label priceLabel = new Label
                {
                    Content = "€ " + item.Price
                };

                stackPanelChildren.Add(titleLabel);
                stackPanelChildren.Add(priceLabel);

                _itemsInStore.Items.Add(stackPanel);
            }

            return size == _itemsInStore.Items.Count;
        }

        [CanBeNull]
        public Item GetItemByName([NotNull] string name)
        {
            name = name.ToLower().Trim();

            return (from item in Items.ItemsArray
                from itemName in item.Names
                where itemName.Value.ToLower().Equals(name.ToLower())
                select item).FirstOrDefault();
        }

        [CanBeNull]
        public string TranslateNameToCurrentLang([NotNull] string givenName, [NotNull] string currentLang)
        {
            return GetItemByName(givenName)?.GetTranslationByKey(currentLang);
        }
    }
}