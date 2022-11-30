using GameEngine.Merchants;
using GameEngine.Items;
using GameEngine.Heroes;
using System.Numerics;
using Phase2.Heroes;
using Phase2;

namespace GameEngine
{
    static class Program
    {
        static void Main(string[] args)
        {

            Player player = HeroGenerator.GeneratePlayer();
            
            
            HeroGenerator.ExportPlayer(player, "prueba");
            
            Aswat p1 = new Aswat(new Location(0, 0));
            p1.AddItem(new Map());
            p11.AddItem(new BowAndArrow());
            p1.AddItem(new Shield());
            p1.AddItem(new Helmet());
            p1.AddItem(new Helmet(ItemRarity.Legendary, 100, -1, 50, "Legendary helmet"));
            p1.AddItem(new Sword());
            
            p1.DisplayInventory();
            p1.EquipArmor();
            p1.Attack();
           Merchant testMerchant = new WeaponMerchant("Test Merchant");
            testMerchant.Interact(p1);
        }
    }
}


