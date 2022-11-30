using GameEngine;
using GameEngine.Heroes;
using GameEngine.Items;
using Phase2.Heroes;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;

namespace GameEngine.Heroes
{
    class HeroGenerator

    {
        private static readonly Random _rng = new();  // Variables used to randomly select the player if the uses choice that option.
        private static int _randomSelection;
        
        
        /// <summary>
        /// Generates a Hero from the 3 options available or randomly.
        /// </summary>
        /// <returns>An instance of the Hero Selected</returns>
        public static Player GeneratePlayer()
        {
            ConsoleHelper.WriteLine("Welcome to Our Magic World\n Please choose a hero to represent you during the game:");
            ConsoleHelper.WriteLine(" 1. Arjuna, Greatest Archer in the Kingdom");
            ConsoleHelper.WriteLine(" 2. Aswat, Our most agile savior!");
            ConsoleHelper.WriteLine(" 3. Karna, Nobody can beat him with a Sword");
            ConsoleHelper.WriteLine(" 4. Let the fate choose for you", ConsoleColor.DarkYellow);

            int myHero = ConsoleHelper.GetSelection(1, 4, "Sorry but such a Hero doesn't exit. Try Again");
            switch (myHero)
            {
                case 1:
                    return SelectHero(new Arjuna());
                case 2:
                    return SelectHero(new Aswat());
                case 3:
                    return SelectHero(new Karna());
                default:
                   Console.WriteLine("You have made eror to select your hero so we are selecting Arjuna for you");
                    return SelectHero(new Arjuna());
                    break;
                    }
            }
        }

        /// <summary>
        /// Generate a Hero from a Json File 
        /// </summary>
        /// <param name="filename">The path of th Json File</param>
        /// <returns>An instance of the Player stored on the Json File</returns>
        public static Player GeneratePlayer(string filename)
        {
            Player player = ParseFile(filename);
            return player;
        }

        /// <summary>
        /// Exports a Hero to a Json File
        /// </summary>
        /// <param name="player"> The hero to be exported</param>
        /// <param name="filename">The name of the file where it will be exported. If the file does exist it will be updated. If it doesn't exist it will create the file</param>
        public static void ExportPlayer(Player player, string filename)
        {
            var playerSerialized = JsonConvert.SerializeObject(player);
            File.WriteAllText($"{filename}", playerSerialized);
        }

        /// <summary>
        /// Method used check the file exist and parse the file to be use to generate a Hero from a File.
        /// </summary>
        /// <param name="filename"> File path to be checked.</param>
        /// <returns> An instance of the Hero if file exist and it has a valid Hero stored.</returns>
        /// <exception cref="FileParseException"> Thrown if the file doesn't exist or the json file is not a Valid Hero</exception>
        private static Player ParseFile(string filename)
        {
            if (File.Exists(filename))
            {
                
                using StreamReader sr = new StreamReader(filename);
                string jsonString = sr.ReadToEnd();
                JSchema schema = JSchema.Parse(File.ReadAllText("schema.json"));
                JObject jsonObject = JObject.Parse(jsonString);
                if (!jsonObject.IsValid(schema))
                {
                    throw new FileParseException($"This json file {filename} is not valid or doesn't contain a Player");
                }
                Player player = JsonConvert.DeserializeObject<Player>(jsonString);
                return player;
            }
            else
            {
                throw new FileParseException($"Unable to find file at location {filename}.");
                
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private static Player SelectHero(Player player)
        {
            Console.WriteLine("What do you want to do?\n1. Customise Powers\n2. Import Hero Powers\n3. Continue with default");
            int? selection = ConsoleHelper.SanitizeInput(Console.ReadLine(), 1, 3);
            switch (selection)
            {
                case 1:
                    return PowerCustomisation(player);
                    break;
                case 2:
                    return ImportHeroPowers(player);
                    break;
                case 3:
                    //returning default player deafult 
                    return player;
                    break;
                default:
                    Console.WriteLine("You have made eror to select your hero so we are selecting Arjuna for you");

                    break;
            }
            return player;
        }
        private static Player PowerCustomisation(Player player)
        {
            int? selection = 1;
            while (selection == 1)
            {
                player = SinglePowerCustomisation(player);
                Console.WriteLine("Would you like to customise another power?\n1. Yes \n2 No.");
                selection = ConsoleHelper.SanitizeInput(Console.ReadLine(), 1, 2);
            }
            return player; //check it again..
        }
       private static Player ImportHeroPowers(Player player)
        {
            string file = File.ReadAllText(@"C:\Users\dell\Desktop\C#projects\Juan\Phase2 Classes\Heroes\Heros.json");//json file location
            ExtractJSON inportHero = JsonSerializer.Deserialize<ExtractJSON>(file);
            player.Strength = inportHero.Strength;
            player.Intelligence = inportHero.Intelligence;
            player.AeroDamage = inportHero.AeroDamage;
            player.Vitality = inportHero.Vitality;
            player.Luck = inportHero.Luck;
            player.Magic = inportHero.Magic;
            return player;
        }
        


     public struct ExtractJSON
        {
            public int Strength { get; set; }
            public int Intelligence { get; set; }
            public int AeroDamage { get; set; }
            public int Vitality { get; set; }
            public int Luck { get; set; }
            public int Magic { get; set; }
        }
        private static Player SinglePowerCustomisation(Player player)
        {
            Console.WriteLine("Which of the following power you want to customise?\n1. Strength,\n2. Intelligence,\n3. AeroDamage,\n4. Vitality,\n5. Luck,\n6. Magic,\n7. Speed,\n8. ArcherPower,\n9.Dodge");
            int? selection = ConsoleHelper.SanitizeInput(Console.ReadLine(), 1, 9);
            int? newSelection;
            switch (selection)
            {
                case 1:
                    Console.WriteLine("Please choose strength for player upto 20");
                    newSelection = ConsoleHelper.SanitizeInput(Console.ReadLine(), 1, 20);
                    player.Strength = (int)newSelection;
                    return player;
                case 2:
                    Console.WriteLine("Please choose intelligence for player upto 20");
                    newSelection = ConsoleHelper.SanitizeInput(Console.ReadLine(), 1, 20);
                    player.Intelligence = (int)newSelection;
                    return player;
                case 3:
                    Console.WriteLine("Please choose aerodamage for player upto 20");
                    newSelection = ConsoleHelper.SanitizeInput(Console.ReadLine(), 1, 20);
                    player.AeroDamage = (int)newSelection;
                    return player;
                case 4:
                    Console.WriteLine("Please choose vitality for player upto 20");
                    newSelection = ConsoleHelper.SanitizeInput(Console.ReadLine(), 1, 20);
                    player.Vitality = (int)newSelection;
                    return player;
                case 5:
                    Console.WriteLine("Please choose luck for player upto 20");
                    newSelection = ConsoleHelper.SanitizeInput(Console.ReadLine(), 1, 20);
                    player.Luck = (int)newSelection;
                    return player;
                case 6:
                    Console.WriteLine("Please choose magic for player upto 20");
                    newSelection = ConsoleHelper.SanitizeInput(Console.ReadLine(), 1, 20);
                    player.Magic = (int)newSelection;
                    return player;
                case 7:
                    Console.WriteLine("Please choose speed for player from below:\n1. SuperSlow\n2. Slow \n3.Normal \n4. Fast \n5. SuperFast");
                    newSelection = ConsoleHelper.SanitizeInput(Console.ReadLine(), 1, 5);
                    switch (newSelection)
                    {
                        case 1:
                            player.Speed = Power.SuperSlow; break;
                        case 2:
                            player.Speed = Power.Slow; break;
                        case 3:
                            player.Speed = Power.Normal; break;
                        case 4:
                            player.Speed = Power.Fast; break;
                        case 5:
                            player.Speed = Power.SuperFast; break;
                    }
                    return player;
                case 8:
                    Console.WriteLine("Please choose archerpower for player from below:\n1. SuperSlow\n2. Slow \n3.Normal \n4. Fast \n5. SuperFast");
                    newSelection = ConsoleHelper.SanitizeInput(Console.ReadLine(), 1, 5);
                    switch (newSelection)
                    {
                        case 1:
                            player.ArcherPower = Power.SuperSlow; break;
                        case 2:
                            player.ArcherPower = Power.Slow; break;
                        case 3:
                            player.ArcherPower = Power.Normal; break;
                        case 4:
                            player.ArcherPower = Power.Fast; break;
                        case 5:
                            player.ArcherPower = Power.SuperFast; break;
                    }
                    return player;
                case 9:
                    Console.WriteLine("Please choose dodge for player from below:\n1. SuperSlow\n2. Slow \n3.Normal \n4. Fast \n5. SuperFast");
                    newSelection = ConsoleHelper.SanitizeInput(Console.ReadLine(), 1, 5);
                    switch (newSelection)
                    {
                        case 1:
                            player.Dodge = Power.SuperSlow; break;
                        case 2:
                            player.Dodge = Power.Slow; break;
                        case 3:
                            player.Dodge = Power.Normal; break;
                        case 4:
                            player.Dodge = Power.Fast; break;
                        case 5:
                            player.Dodge = Power.SuperFast; break;
                    }
                    return player;

            }
            return player;
        }
        
        }
         
        class FileParseException : Exception
        {
            public FileParseException(string message) : base(string.Format($"{message}")) { }
            
        }



