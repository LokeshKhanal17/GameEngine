using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;

namespace GameEngine.Items
{
    class Inventory
    {
        private const int DefaultStartCapacity = 10;
        protected readonly List<Item> _items;

        public Inventory() : this(DefaultStartCapacity) { }

        public Inventory(int startingCapacity)
        {
            _items = new List<Item>(startingCapacity);
        }

        public int GetPackValue()
        {
            int totalValue = 0;
            foreach (var item in _items)
            {
                totalValue += item.GetValue() * item.GetQuantity();
            }
            return totalValue;
        }

        public int GetNumSlots()
        {
            return _items.Count;
        }

        public int GetNumItems()
        {
            int totalItems = 0;
            foreach (var item in _items)
            {
                totalItems += item.GetQuantity();
            }
            return totalItems;
        }

        public Item GetItemAtIndex(int index)
        {
            if (index < 0 || index >= _items.Count)
            {
                return null;
            }
            return _items[index];
        }

        public void Add(params Item[] items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void Add(Item item, int quantity = 1)
        {
            int index = FindItemIndex(item);
            if (index == -1) // -1 means the no items of this type exist in the inventory
            {
                _items.Add(item.Clone());
                _items[^1].IncreaseQuantity(quantity - 1);
            }
            else
            {
                _items[index].IncreaseQuantity(quantity);
            }
        }

        public bool Remove(Item item, int quantity = 1)
        {
            int index = FindItemIndex(item);
            return RemoveAtIndex(index, quantity);
        }

        public bool RemoveAtIndex(int index, int quantity = 1)
        {
            bool removed = false;
            if (index >= 0 && index < _items.Count)
            {
                Item item = _items[index];
                // If the quantity of the slot is 0 after removing an item
                if (!item.DecreaseAmount(quantity))
                {
                    // Remove the item from the list
                    _items.RemoveAt(index);
                }
                removed = true;
            }
            return removed;
        }

        public bool Contains<T>()
        {
            foreach (var item in _items)
            {
                if (item is T)
                {
                    return true;
                }
            }
            return false;
        }

        // I'm writing the getters out in full here for academic purposes.
        // You should use the lambda operator (see Player.cs) to shrink down your code
        // The idiomatic C# approach would be to use properties but make sure you
        // know what you are doing!
        public bool ContainsMap()
        {
            return Contains(ItemID.Map);
        }

        public bool ContainsAncientSword()
        {
            return Contains(ItemID.AncientSword);
        }

        public bool RemoveMap()
        {
            return Remove(ItemID.Map);
        }

        public bool RemoveAncientSword()
        {
            return Remove(ItemID.AncientSword);
        }

        private bool Contains(ItemID target)
        {
            return FindItemIndex(target) != -1;
        }

        private bool Remove(ItemID id)
        {
            int index = FindItemIndex(id);
            return RemoveAtIndex(index);
        }

        public int FindItemIndex(ItemID target)
        {
            for (int i = 0; i < _items.Count; ++i)
            {
                if (_items[i].GetItemID() == target)
                {
                    return i;
                }
            }
            return -1;
        }

        public int FindItemIndex(Item target)
        {
            for (int i = 0; i < _items.Count; ++i)
            {
                if (_items[i].Equals(target))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Display()
        {
            int index = 1;
            foreach (var item in _items)
            {
                ConsoleHelper.WriteItemLine(index++, item);
            }
        }

        public void SortByWeapons()
        {
            for (int j = 0; j < _items.Count-1; j++)
            {
                int iMin = j;
                for (int i = j+1; i < _items.Count; i++)
                {
                    if (_items[i].GetItemID() == ItemID.Sword || _items[i].GetItemID() == ItemID.AncientSword || _items[i].GetItemID() == ItemID.BowAndArrow )
                    {
                        iMin = i;
                    }
                       
                }
                if (iMin != j)
                {
                    (_items[j], _items[iMin]) = (_items[iMin], _items[j]);
                }
            }
        }

        public void SortByArmors()
        {
            for (int j = 0; j < _items.Count - 1; j++)
            {
                int iMin = j;
                for (int i = j + 1; i < _items.Count; i++)
                {
                    if (_items[i].GetItemID() == ItemID.Helmet || _items[i].GetItemID() == ItemID.Shield )
                    {
                        iMin = i;
                    }

                }
                if (iMin != j)
                {
                    (_items[j], _items[iMin]) = (_items[iMin], _items[j]);
                }
            }
        }

        public int DisplayWeapons()
        {
            SortByWeapons();
            int index = 0;
            foreach (var item in _items)
            {
                if (item is Sword || item is AncientSword || item is BowAndArrow)
                {
                    ConsoleHelper.WriteItemLine(++index, item);
                }
                
            }
            return index;
        }

        public int DisplayArmors()
        {
            SortByArmors();
            int index = 0;
            foreach (var item in _items)
            {
                if (item is Helmet || item is Shield )
                {
                    ConsoleHelper.WriteItemLine(++index, item);
                }

            }
            return index;
        }

        public override string ToString()
        {
            StringBuilder sb = new("Your pack contains the following items.\n");
            int index = 1;
            int value = 0;
            foreach (var item in _items)
            {
                value += item.GetValue();
                sb.Append($"{index++}. {item}\n");
            }
            sb.Append($"\nTotal value: ${value}");
            return sb.ToString();
        }
    }
}
