using System;
using VeelPlezier.objects;

namespace VeelPlezierTest.Tests.objects.itemTests
{
    internal static class ItemExt
    {
        public static Item GetItemByInt(int itemNumber)
        {
            Item item;
            switch (itemNumber)
            {
                case 1:
                    item = ItemOne;
                    break;
                
                case 2:
                    item = ItemTwo;
                    break;
                
                default: throw new ArgumentException();
            }

            return item;
        }

        
        public static string GetExpectedTranslation(int itemNumber, string lang)
        {
            string translation;
            switch (itemNumber)
            {
                case 1:
                    switch (lang)
                    {
                        case "en":
                            translation = "Fries";
                            break;
                        
                        case "nl":
                            translation = "Friet";
                            break;
                        
                        case "fake":
                            translation = "fake";
                            break;
                        
                        default: throw new ArgumentException();
                    }

                    break;
                
                case 2:
                    switch (lang)
                    {
                        case "en":
                            translation = "Sandwich ham-cheese";
                            break;
                        
                        case "nl":
                            translation = "Broodje ham-kaas";
                            break;
                        
                        case "fake":
                            translation = "fake";
                            break;
                        
                        default: throw new ArgumentException();
                    }

                    break;
                
                default: throw new ArgumentException();
            }

            return translation;
        }
        
        private static Item ItemOne =>
            new Item(new[]
            {
                new Name("en", "Fries"),
                new Name("nl", "Friet"),
                new Name("fake", "fake")
            }, "1,50");

        private static Item ItemTwo =>
            new Item(new[]
            {
                new Name("en", "Sandwich ham-cheese"),
                new Name("nl", "Broodje ham-kaas"),
                new Name("fake", "fake")
            }, "2,00");
    }
}