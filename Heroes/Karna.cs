

using GameEngine.Heroes;
using GameEngine.Items;
using System.Runtime.CompilerServices;

namespace GameEngine.Heroes
{
    class Karna : Player
    {
        public Karna(int? strength = null, int? intelligence = null, int? aeroDamage = null, int? vitality = null, int? luck = null, int? magic = null) : base(strength, intelligence, aeroDamage, vitality, luck, magic) { }
   

        public override int Attack(/*Monster monster*/)
        {
            switch (GetWeaponEquipped())
            {
                case BowAndArrow:
                    return GetWeaponEquipped().GetDamageRating();
                case Sword:
                    return GetWeaponEquipped().GetDamageRating()*2;
                case null:
                    return 1;
                default:
                    return GetWeaponEquipped().GetDamageRating();
            }
        }

        public override void TakeDamage(int amount)
        {
            if (_rng.NextDouble() < Speed)
            {
                Console.WriteLine("Our hero has dodge the attack");
            }
            else
            {
                if (ShieldIsEquipped())
                {
                    amount = amount - GetArmorEquipped().GetDefenseRating()*2;
                }
                else if (HelmetIsEquipped())
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
    }
}
