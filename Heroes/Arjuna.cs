

using GameEngine.Heroes;
using GameEngine.Items;

namespace Phase2.Heroes
{
    class Arjuna : Player
    {
       
		public Arjuna(int? strength = null, int? intelligence = null, int? aeroDamage = null, int? vitality = null, int? luck = null, int? magic = null) : base(strength, intelligence, aeroDamage, vitality, luck, magic) { }
	
        public override int Attack(/*Monster monster*/)
        {
             
            switch (GetWeaponEquipped()) // check if this working and probably use enum
            {
                case BowAndArrow:
                    return GetWeaponEquipped().GetDamageRating() * 2;
                case Sword:
                    return GetWeaponEquipped().GetDamageRating()/2;
                case null:
                    return 1;
                default:
                    return GetWeaponEquipped().GetDamageRating();
            }
        }

    }
}
