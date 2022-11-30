using System;
using System.Collections.Generic;

using GameEngine.Interfaces;
using GameEngine.Heroes;
using GameEngine.Items;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace GameEngine.Merchants
{
    abstract class Merchant : IInteractable
    {
        private const int MinStartingCurrency = 25;
        private const int MaxStartingCurrency = 200;

        // Should use Random from the main engine eventually
        private static readonly Random _random = new Random();

        protected readonly Inventory _items = new();
        private readonly string _name;
        private int _currency;

        protected Merchant(string name)
        {
            _name = name;
            _currency = _random.Next(MinStartingCurrency, MaxStartingCurrency);
        }

        public void Interact(Player player)
        {
            ConsoleHelper.WriteLine($"Welcome to {_name}'s shop.");
            bool keepShopping = true;
            while (keepShopping)
            {
                // Member variable Dictionary<int, Func> -> a really nice approach for this
                int choice = BuySellMenu(player);
                switch (choice)
                {
                    case 1: // Merchant sells an item to the player or the player buys an item from the merchant
                        MerchantSellToPlayer(player);
                        break;
                    case 2: // Merchant buys an item from the player or the player sells an item to the merchant
                        MerchantBuyFromPlayer(player);
                        break;
                    default: // Player exits the interaction
                        ConsoleHelper.WriteLine($"{_name} bids you farewell.");
                        keepShopping = false;
                        break;
                }
            }
        }

        // Do we need this?
        protected void AddItems(Item item, int quantity)
        {
            _items.Add(item, quantity);
        }

        protected virtual bool BuyFromPlayer(Player player, Item item)
        {
            bool itemBought = false;
            int cost = item.GetValue();
            if (cost <= _currency)
            {
                player.Sell(item);
                _items.Add(item);
                _currency -= cost;
                itemBought = true;
            }
            return itemBought;
        }

        protected virtual bool SellToPlayer(Player player, Item item)
        {
            bool itemSold = false;
            if (player.Buy(item))
            {
                _items.Remove(item);
                itemSold = true;
            }
            return itemSold;
        }

        // A bit long and complicated
        private void DoTransaction(Player player, bool selling)
        {
            int numItems = (selling) ? _items.GetNumSlots() : player.NumItems();
            int totalChoices = numItems + 1;
            Action display = (selling) ? _items.Display : player.DisplayInventory;

            if (numItems == 0)
            {
                string msg = (selling) ? $"{_name} does not currently have any items in stock."
                    : "The merchant stares at your empty pack in dismay.";
                ConsoleHelper.WriteLine(msg);
            }
            else
            {
                string msg = (selling) ? $"{_name} currently has the following items for sale."
                    : $"You show {_name} what you are carrying.";
                ConsoleHelper.WriteLine(msg);
                display();
                ConsoleHelper.WriteLine($"{totalChoices}. Go Back", ConsoleColor.Red);
                ConsoleHelper.WriteLine($"You currently have currency worth {player.GetCurrency()}.");
                int slot = ChooseItemMenu(totalChoices);
                if (slot <= numItems)
                {
                    int index = slot - 1;
                    Func<Player, Item, bool> transaction = (selling) ? SellToPlayer : BuyFromPlayer;
                    msg = (selling) ? "You can't afford this item!" : "The merchant can't buy this item from you!";
                    Item item = (selling) ? _items.GetItemAtIndex(index) : player.GetInventoryItem(index);
                    if (transaction(player, item))
                    {
                        ConsoleHelper.WriteLine($"{_name} thanks you for your patronage.");
                    }
                    else
                    {
                        ConsoleHelper.WriteLine(msg);
                    }
                }
            }
        }

        private void MerchantSellToPlayer(Player player)
        {
            DoTransaction(player, true);
        }

        private void MerchantBuyFromPlayer(Player player)
        {
            DoTransaction(player, false);
        }

        private static int BuySellMenu(Player player)
        {
            ConsoleHelper.WriteLine($"You currently have currency worth {player.GetCurrency()}.");
            ConsoleHelper.WriteLine("What would you like to do?\n1. Buy\n2. Sell");
            ConsoleHelper.WriteLine("3. Leave", ConsoleColor.Red);
            return GetSelection(1, 3, "The merchant stares at you blankly clearly not understanding" +
                " what you want.");
        }

        private static int ChooseItemMenu(int numItems)
        {
            ConsoleHelper.WriteLine($"Choose an item or press {numItems} to go back.");
            return GetSelection(1, numItems, "The merchant does not seem to understand you.\n" +
                "Choose a number corresponding to the item you want to buy or sell.");
        }

        private static int GetSelection(int min, int max, string errorMsg)
        {
            int? selection = ConsoleHelper.SanitizeInput(Console.ReadLine(), min, max);
            while (selection == null)
            {
                Console.WriteLine(errorMsg);
                selection = ConsoleHelper.SanitizeInput(Console.ReadLine(), min, max);
            }
            return (int)selection; // 'selection' cannot be null here
        }
    }

    sealed class WeaponMerchant : Merchant
    {
        private const int StartingWeapons = 5;
        private const string InvalidItem = "This is a weapon shop. What use would I have for a ";

        public WeaponMerchant(string name) : base(name)
        {
            // Only 1 type of weapon for now
            // Can improve this by filling the initial inventory 
            // with different types of weapons (how to automate this? Factory)
            AddWeapons(new Sword(), StartingWeapons);
            AddWeapons(new Sword(ItemRarity.Magical, 15, 20, "An exceedingly well crafted greatsword."), 1);
        }

        protected override bool BuyFromPlayer(Player player, Item item)
        {
            if (item is Weapon weapon)
            {
                return (base.BuyFromPlayer(player, weapon));
            }
            ConsoleHelper.WriteLine($"{InvalidItem}{item.GetItemID()}", ConsoleColor.Red);
            return false;
        }

        private void AddWeapons(Weapon weapon, int quantity)
        {
            _items.Add(weapon, quantity);
        }
    }

    sealed class ArmorMerchant : Merchant
    {
        private const int StartingArmors = 5;
        private const string InvalidItem = "This is an armor shop. What use would I have for a ";

        public ArmorMerchant(string name) : base(name)
        {
            AddArmors(new Helmet(), StartingArmors); 
            AddArmors(new Shield(), StartingArmors);   
        }

        protected override bool BuyFromPlayer(Player player, Item item)
        {
            if (item is Armor armor)
            {
                return (base.BuyFromPlayer(player, armor));
            }
            ConsoleHelper.WriteLine($"{InvalidItem}{item.GetItemID()}", ConsoleColor.Red);
            return false;
        }

        private void AddArmors(Armor armor, int quantity)
        {
            _items.Add(armor, quantity);   
        }

    }

    sealed class PotionMerchant : Merchant
    {
        private const int StartingPotions = 5;
        private const string InvalidItem = "This is a Potion shop. What use would I have for a ";

        public PotionMerchant(string name) : base(name)
        {
            AddPotions(new HealingPotion(), StartingPotions);
        }

        protected override bool BuyFromPlayer(Player player, Item item)
        {
            if (item is Potion potion)
            {
                return (base.BuyFromPlayer(player, potion));
            }
            ConsoleHelper.WriteLine($"{InvalidItem}{item.GetItemID()}", ConsoleColor.Red);
            return false;
        }

        private void AddPotions(Potion potion, int quantity)
        {
            _items.Add(potion, quantity);
        }
    }
}
