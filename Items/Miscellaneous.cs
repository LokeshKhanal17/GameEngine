namespace GameEngine.Items
{
	class Map : Item
	{
		private const int Value = 0;
		public Map() : base(ItemID.Map, Value) 
		{
			_isUnique = true;
			_isSellable = false;
			_description = "An old map. It exudes an aura of magic.";
		}

		public override Item Clone()
		{
			return new Map();
		}
	}
}
