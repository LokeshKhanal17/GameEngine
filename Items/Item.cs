using System;

namespace GameEngine.Items
{
	abstract class Item
	{
		protected readonly ItemID _id;
		protected readonly ItemRarity _rarity;
		protected int _quantity = 1;
		protected int _value;
		protected bool _isUnique = false;
		protected bool _isSellable = true;
		protected string _description;

		protected Item(ItemID id, ItemRarity rarity, int value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException($"{value} is not a valid value for an item. " +
					$"The value of an item must be >= 0.");
			}
			_id = id;
			_rarity = rarity;
			_value = value; // What is the problem with this?
		}

		protected Item(ItemID id, int value) : this(id, ItemRarity.Common, value) { } 

		protected Item(Item item)
		{
			_id = item._id;
			_rarity = item._rarity;
			_value = item._value; // This is fine if we handle line 24 properly
		}

		// A factory (ItemFactory) class would be better for this type of thing
		public abstract Item Clone();

		// Properties help eliminate code like this. Lines 40-67 disappear with proper use of Properties
		public ItemID GetItemID()
		{
			return _id;
		}
		public ItemRarity GetRarity()
		{
			return _rarity; 
		}

		public int GetValue()
		{
			return _value;
		}

		public int GetQuantity()
		{
			return _quantity;
		}

		public bool IsUnique()
		{
			return _isUnique;
		}

		public bool IsSellable()
		{
			return _isSellable;
		}

		public void IncreaseQuantity(int amount=1)
		{
			if (!_isUnique)
			{
				_quantity += amount;
			}
		}

		public bool DecreaseAmount(int amount=1)
		{
			bool itemsLeft = false;
			if (_quantity - amount > 0)
			{
				_quantity -= amount;
				itemsLeft = true;
			}
			// Return true if _quantity > 0 after the decrease
			return itemsLeft;
		}

		public override string ToString()
		{
			return $"{_id} ({_quantity})\n" +
				$"Value: {_value}";
		}

		public override bool Equals(object obj)
		{
			return obj is Item item &&
				   _id == item._id &&
				   _rarity == item._rarity &&
				   _value == item._value &&
				   _isUnique == item._isUnique &&
				   _isSellable == item._isSellable;
		}
	}

	enum ItemID { Miscellaneous, HealingPotion, Map, Sword, AncientSword, Shield, Helmet, BowAndArrow }
	enum ItemRarity { Common, Magical, Epic, Legendary }
}
