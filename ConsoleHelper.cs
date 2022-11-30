using System;
using System.Collections.Generic;

using GameEngine.Items;

namespace GameEngine
{
	static class ConsoleHelper
	{
		// Should this be stored here or in the Item class? Why should it be stored in the Item class?
		static readonly Dictionary<ItemRarity, ConsoleColor> _rarityColorDict = new Dictionary<ItemRarity, ConsoleColor>()
		{
			{ ItemRarity.Common, ConsoleColor.Gray },
			{ ItemRarity.Magical, ConsoleColor.Green },
			{ ItemRarity.Epic, ConsoleColor.Magenta },
			{ ItemRarity.Legendary, ConsoleColor.Yellow }
		};

		// Changes to the specified color and then displays the text on its own line.
		public static void WriteLine(string text, ConsoleColor color=ConsoleColor.Gray)
		{
			Console.ForegroundColor = color;
			Console.WriteLine(text);
			Console.ForegroundColor = ConsoleColor.Gray;
		}

		public static void WriteItemLine(int index, Item item)
		{
			ConsoleColor color;
			if (_rarityColorDict.TryGetValue(item.GetRarity(), out color))
			{
				Console.ForegroundColor = color;
			}
			Console.WriteLine($"{index}. {item}\n");
			Console.ForegroundColor = ConsoleColor.Gray;
		}


		public static void Write(DisplayDetails details)
		{
			Console.ForegroundColor = details.Color;
			Console.Write(details.Text);
			Console.ForegroundColor = ConsoleColor.Gray;
		}

		// Changes to the specified color and then displays the text without moving to the next line.
		public static void Write(string text, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Console.Write(text);
			Console.ForegroundColor = ConsoleColor.Gray;
		}

		public static int? SanitizeInput(string input, int min, int max)
		{
			int result;
			if (int.TryParse(input, out result) && result >= min && result <= max)
			{
				return result;
			}
			return null;
		}

        public static int GetSelection(int min, int max, string errorMsg)
        {
            int? selection = SanitizeInput(Console.ReadLine(), min, max);
            while (selection == null)
            {
                Console.WriteLine(errorMsg);
                selection = SanitizeInput(Console.ReadLine(), min, max);
            }
            return (int)selection; // 'selection' cannot be null here
        }

    }
	record DisplayDetails(string Text, ConsoleColor Color);
}
