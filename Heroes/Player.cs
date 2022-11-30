using GameEngine.Items;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace GameEngine.Heroes
{
	class Player
	{
		public HeroID Id { get; protected set; }  // enum used to identify each hero
		public float Speed { get; protected set;} // used to calculate the chances of dodge an attack.
        protected readonly Random _rng = new();
        protected Weapon? _weaponEquipped = null;
		protected Armor? _armorEquipped = null;
        private const int MaxHealth = 100;
		private const int StartingCurrency = 200;

		protected int _health = MaxHealth;
		protected Inventory _inventory = new();
		public int _currency = StartingCurrency;
		//public int _attackingCapacity = 1;

		// Indicates whether the player currently has the catacomb map.
		public bool HasMap
		{
			get => _inventory.ContainsMap();
		}

		// Indicates whether the player currently has the ancient sword.
		public bool HasAncientSword 
		{
			get => _inventory.ContainsAncientSword();
		}

		// These are traditional getters using the lambda operator
		public bool RemoveMap() => _inventory.RemoveMap();

		public bool RemoveAncientSword() => _inventory.RemoveAncientSword();

		public void AddItem(Item item) => _inventory.Add(item);

		public bool Buy(Item item)
		{
			bool itemBought = false;
			int cost = item.GetValue();
			if (_currency >= cost)
			{
				_inventory.Add(item);
				_currency -= cost;
				itemBought = true;
			}
			return itemBought;
		}

		public bool Sell(Item item)
		{
			bool itemSold = false;
			if (_inventory.Remove(item))
			{
				_currency += item.GetValue();
				itemSold = true;
			}
			return itemSold;
		}

		public Item GetInventoryItem(int index) => _inventory.GetItemAtIndex(index);

		public void DisplayInventory() => _inventory.Display();

		public int NumItems() => _inventory.GetNumSlots();

		public int GetCurrency() => _currency;

		public void Heal(int amount)
		{
			_health += amount;
			if (_health > MaxHealth)
			{
				_health = MaxHealth;
			}
		}

		public virtual void TakeDamage(int amount)	
		{
			if (_rng.NextDouble() < Speed)
			{
				Console.WriteLine("Our hero has dodge the attack");
			}
			else
			{
                if (ShieldIsEquipped() || HelmetIsEquipped())
                {
                    amount = amount - GetArmorEquipped().GetDefenseRating();
                }
                _health -= amount;
                if (_health < 0)
                {
                    _health = 0;
                }
            }
		}

		public virtual int Attack(/*Monster monster,*/) // do this method abstract
		{
			//This will be used later to attack the monster
			//monster._healt -= _attackingCapacity;
			//if (monster._healt < 0)
			//{
			//	monster._health = 0;
			//}
			if (GetWeaponEquipped() != null)
			{
                return GetWeaponEquipped().GetDamageRating();
            }
			else
			{
				return 0;
			}
			
        }

		public void EquipWeapon()
		{
			Console.WriteLine("These are the weapons that you currently have:");
			int maxSelection = _inventory.DisplayWeapons();
			int selection = ChooseItemMenu(maxSelection);
			_weaponEquipped = (Weapon?)GetInventoryItem(selection - 1);
		}

		public void EquipArmor()
		{
            Console.WriteLine("These are the weapons that you currently have:");
            int maxSelection = _inventory.DisplayArmors();
            int selection = ChooseItemMenu(maxSelection);
            _armorEquipped = (Armor?)GetInventoryItem(selection - 1);
        }

        private static int ChooseItemMenu(int numItems)
        {
            ConsoleHelper.WriteLine($"Choose an item to equip");
            return GetSelection(1, numItems, "That's not a valid option\n" +
                "Choose a number corresponding to the weapon you want to equip before you die.");
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
    


		public Weapon GetWeaponEquipped()
		{
			return _weaponEquipped;
		}
		
        public bool BowAndArrowIsEquipped()
		{
			if (_weaponEquipped is BowAndArrow)
			{
				return true;
			}
			return false;
		}

        public bool SwordIsEquipped()
        {
            if (_weaponEquipped is Sword)
            {
                return true;
            }
            return false;
        }

		public Armor GetArmorEquipped()
		{
			return _armorEquipped;
		}

        public bool ShieldIsEquipped()
        {
            if (_armorEquipped is Shield)
            {
                return true;
            }
            return false;
        }

        public bool HelmetIsEquipped()
        {
            if (_armorEquipped is Helmet)
            {
                return true;
            }
            return false;
        }

		



		/**************************************************************************************************
             * You do not need to alter anything below here but you are free to do
             * For example - while under the effects of a potion of invulnerability, the player cannot die
         *************************************************************************************************/

		// Indicates whether the player is alive or not.
		public bool IsAlive
		{
			get => _health > 0;
		}

		// Represents the distance the player can sense danger.
		// Diagonal adjacent squares have a range of 2 from the player.
		public int SenseRange { get; } = 1;

		// Creates a new player that starts at the given location.
		public Player(Location start) => Location = start;

		// The player's current location.
		public Location Location { get; set; }

		// Explains why a player died.
		public string CauseOfDeath { get; private set; }

		public void Kill(string cause)
		{
			_health = 0;
			CauseOfDeath = cause;
		}
	}

	// Represents a location in the 2D game world, based on its row and column.
	public record Location(int Row, int Column);

    enum HeroID { Arjuna, Karna, Aswat }
}

