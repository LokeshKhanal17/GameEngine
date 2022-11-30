using System;

namespace GameEngine.Items
{
	abstract class Armor : Item
	{
		protected int _defenseRating;

		protected Armor(ItemID id, ItemRarity rarity, int defenseRating, int value) : base(id, rarity, value)
		{
			_defenseRating = defenseRating;
		}

		protected Armor(ItemID id, int defenseRating, int value)
			: this(id, ItemRarity.Common, value, defenseRating) { }

		protected Armor(Armor armor) 
			: this(armor._id, armor._rarity, armor._defenseRating, armor._value) {}

		public int GetDefenseRating()
		{
			return _defenseRating;
		}

		public override bool Equals(object obj)
		{
			return obj is Armor armor &&
				   base.Equals(obj) &&
				   _defenseRating == armor._defenseRating;
		}
		public override string ToString()
		{
			return $"{base.ToString()})\n" +
				$"Defense: {_defenseRating}";
		}
	}

	class Helmet : Armor
	{
		private const int GenericDefenseRating = 1;
		private const int GenericVisionModifier = 0;
		private const int GenericValue = 5;
		private const string GenericDescription = "A stylish leather cap.";

		private int _visionModifier;

		public Helmet()	: this(ItemRarity.Common, GenericDefenseRating, 
			GenericVisionModifier, GenericValue, GenericDescription) { }

		public Helmet(ItemRarity rarity, int defense, int visionModifier, int value, string description) 
			: base(ItemID.Helmet, rarity, defense, value)
		{
			_visionModifier = visionModifier;
			_description = description;
		}

		public float GetVisionModifier()
		{
			return _visionModifier;
		}

		public override Item Clone()
		{
			return new Helmet(_rarity, _defenseRating, _visionModifier, _value, _description);
		}

		public override bool Equals(object obj)
		{
			return obj is Helmet helmet &&
				   base.Equals(obj) &&
				   _visionModifier == helmet._visionModifier;
		}

		public override string ToString()
		{
			return $"{base.ToString()}\n" +
				$"Vision: {_visionModifier}\n" +
				$"{_description}";
		}
	}

	class Shield : Armor
	{
		private const int GenericDefenseRating = 5;
		private const float GenericBlockChance = .2f;
		private const int GenericValue = 10;
		private const string GenericDescription = "A strong wooden shield";

		private float _blockChance;

		public Shield()	: this(ItemRarity.Common, GenericDefenseRating, 
			GenericBlockChance, GenericValue, GenericDescription) { }

		public Shield(ItemRarity rarity, int defense, float blockChance, int value, string description) 
			: base(ItemID.Shield, rarity, defense, value)
		{
			if (blockChance <= 0f || blockChance > 1f)
			{
				throw new ArgumentOutOfRangeException($"{blockChance} is not a valid percentage. " +
					$"The chance of blocking must be > 0 and <= 1.");
			}
			_blockChance = blockChance;
			_description = description;
		}

		public float GetBlockChance()
		{
			return _blockChance;
		}

		public override Item Clone()
		{
			return new Shield(_rarity, _defenseRating, _blockChance, _value, _description);
		}

		public override bool Equals(object obj)
		{
			return obj is Shield shield &&
				   base.Equals(obj) &&
				   _blockChance == shield._blockChance;
		}

		public override string ToString()
		{
			return $"{base.ToString()}\n" +
				$"Block Chance: {_blockChance}\n" +
				$"{_description}";
		}
	}
}
