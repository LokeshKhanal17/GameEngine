
using GameEngine.Heroes;
using GameEngine.Items;
using System.Security.Cryptography;

namespace GameEngine.Heroes
{
    class Aswat : Player
    {
       
        public Aswathama(int? strength = null, int? intelligence = null, int? aeroDamage = null, int? vitality = null, int? luck = null, int? magic = null) : base(strength, intelligence, aeroDamage, vitality, luck, magic) { }
    
        public override int Attack(/*Monster monster*/)
        {
            switch (GetWeaponEquipped())
            {
                case BowAndArrow:
                    return GetWeaponEquipped().GetDamageRating()/2;
                case Sword:
                    return GetWeaponEquipped().GetDamageRating();
                case null:
                    return 1;
                default:
                    return GetWeaponEquipped().GetDamageRating();
            }
        }
    }
}
