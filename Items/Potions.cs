namespace GameEngine.Items
{
	abstract class Potion : Item
	{
		protected Potion(ItemID id, int value) : base(id, value) { }

		protected Potion(Potion potion) : this(potion._id, potion._value) { }

		// This needs some work, need a struct/class that returns here
		// Try to avoid the following method signature
		// public abstract void Imbibe(Player player) -> It is tempting but why is that not a good approach?
		public abstract int Imbibe();
	}

	class HealingPotion : Potion
	{
		private const int HealAmount = 10;
		private const int Value = 2;

		public HealingPotion() : base(ItemID.HealingPotion, Value) 
		{
			_description = "A bottle with a thick red fluid inside.";
		}

		public override Item Clone()
		{
			return new HealingPotion();
		}

		public override int Imbibe()
		{
			return HealAmount;
		}
	}
}
