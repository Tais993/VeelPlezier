using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using Newtonsoft.Json;
using VeelPlezier.objects;

namespace VeelPlezier
{
    internal sealed class ItemHandler
    {
        private readonly ComboBox _itemsInStore;
        private Items items = null;

        internal ItemHandler(ComboBox itemsInStore)
        {
            _itemsInStore = itemsInStore;
        }

        internal void LoadItemsAsync(CultureInfo cultureInfo)
        {
            string path =
                Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                    throw new InvalidOperationException(), @"resources/items/items.json");
            string json = File.ReadAllText(path);
            items = JsonConvert.DeserializeObject<Items>(json);

            ReloadItemsInDisplay(cultureInfo);
        }

        public void ReloadItemsInDisplay(CultureInfo cultureInfo)
        {
            string countryName = cultureInfo.Name.Split('-')[0];
            
            _itemsInStore.Items.Clear();

            foreach (Item item in items.ItemsArray)
            {
                StackPanel stackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };

                UIElementCollection stackPanelChildren = stackPanel.Children;

                Label titleLabel = new Label
                {
                    Content = item.GetByKey(countryName)
                };

                Label priceLabel = new Label
                {
                    Content = "€ " + item.Price
                };

                stackPanelChildren.Add(titleLabel);
                stackPanelChildren.Add(priceLabel);

                _itemsInStore.Items.Add(stackPanel);
            }
        }

        public Item GetItemByName(string name)
        {
            return (from item in items.ItemsArray 
                from itemName in item.Names 
                where itemName.Value.Equals(name) 
                select item).FirstOrDefault();
        }
    }
}