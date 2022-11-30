using System;

namespace GameEngine.Items
{
	// The warning here is something you may want to look into but not necessary for this project
	// However, it is a very bad habit to ignore compiler warnings and you should typically treat
	// them as errors.
	abstract class Weapon : Item
	{
		protected int _damageRating;

		protected Weapon(ItemID id, ItemRarity rarity, int value, int damageRating) : base(id, rarity, value) 
		{
			if (damageRating < 0)
			{
				throw new ArgumentOutOfRangeException($"{damageRating} is not a valid damage rating. " +
					$"The damage rating must be >= 0.");
			}
			_damageRating = damageRating;
		}

		protected Weapon(Weapon weapon) 
			: this(weapon._id, weapon._rarity, weapon._value, weapon._damageRating) {}

		public int GetDamageRating()
		{
			return _damageRating;
		}

		public override bool Equals(object obj)
		{
			return obj is Weapon weapon &&
				   base.Equals(obj) &&
				   _damageRating == weapon._damageRating;
		}

		public override string ToString()
		{
			return $"{base.ToString()}\n" +
				$"Damage: {_damageRating}\n" +
				$"{_description}";
		}
	}

	// Do we need a Sword class or should we make instances of Weapons that are swords?
	// At this point in development it is questionable whether we need a Sword class
	// It makes sense if eventually Swords will fundamentally act different than other
	// types of weapons 
	class Sword : Weapon
	{
		private const int GenericDamage = 7;
		private const int GenericValue = 5;
		private const string GenericDescription = "A gleaming sword with a razor sharp edge.";

		public Sword(ItemRarity rarity, int damage, int value, string description) 
			: base(ItemID.Sword, rarity, damage, value)
		{
			_description = description;
		}

		public Sword() : this(ItemRarity.Common, GenericDamage, GenericValue, GenericDescription) { }

		public override Item Clone()
		{
			return new Sword(_rarity, _value, _damageRating, _description);
		}
	}

	sealed class AncientSword : Weapon
	{
		private const int StartDamage = 0;
		private const int StartValue = 0;
		private const string StartDescription = "The ancient sword is too dull to be used as a weapon.\n" +
				"A talented weaponsmith may be able to restore it to its former glory.";
		private const int RenewedDamage = 100;
		private const int RenewedValue = 1000;

		public AncientSword() : this(StartDamage, StartValue, StartDescription) { }

		private AncientSword(int damageRating, int value, string description)
			: base(ItemID.AncientSword, ItemRarity.Epic, damageRating, value) 
		{
			_isUnique = true;
			_isSellable = false;
			_description = description;
		}

		public override Item Clone()
		{
			return new AncientSword(_damageRating, _value, _description);
		}

		public void Reforge()
		{
			_isSellable = true;
			_damageRating = RenewedDamage;
			_value = RenewedValue;
			_description = "This sword has been restored to its former glory " +
				"and is now a formidable weapon.";
		}
	}

    class BowAndArrow : Weapon
    {
        private const int GenericDamage = 7;
        private const int GenericValue = 5;
        private const string GenericDescription = "A well crafted Bow with some arrows.";

        public BowAndArrow(ItemRarity rarity, int damage, int value, string description)
            : base(ItemID.BowAndArrow, rarity, damage, value)
        {
            _description = description;
        }

        public BowAndArrow() : this(ItemRarity.Common, GenericDamage, GenericValue, GenericDescription) { }

        public override Item Clone()
        {
            return new BowAndArrow(_rarity, _value, _damageRating, _description);
        }
    }

   
}
