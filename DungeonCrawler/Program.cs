using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player();
            Console.ForegroundColor = ConsoleColor.White;

            // Title Screen
            Console.WriteLine("===================================\nDUNGEON CRAWLER (by Chris Simmons)\n===================================\n");

            // Set Player Name
            Console.WriteLine("Please type your name and hit enter:");
            player.name = Console.ReadLine();

            // Set Class
            Console.WriteLine("What class are you? (1 = Warrior, 2 = Archer, 3 = Mage)");
            player.role = Convert.ToInt32(Console.ReadLine());

            // Display Player Details
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Now playing as " + player.details());

            // Init Dungeons
            Dungeon dungeonOne = new Dungeon(player, 1);
            // Dungeon dungeonTwo = new Dungeon(player, 2);

            // Display
            Console.ReadLine();

        }
    }

    class Player
    {
        public string name { get; set; }
        public int role { get; set; }
        public int health { get; set; }

        // Constructor
        public Player()
        {
            this.health = 10;
        }

        // Player Details
        public string details()
        {
            return String.Format("{0} the {1} ({2})", this.name, this.displayClass(), this.health);
        }

        // Diplay Class Name
        public string displayClass()
        {
            switch (this.role)
            {
                case 1: return "Warrior";
                case 2: return "Archer";
                case 3: return "Mage";
                default: return  "Noob";
            }
        }

        // Special Ability
        private void specialAbility()
        {
            // ...
        }
    }

    class Enemy
    {
        public string name { get; set; }
        public int health { get; set; }
        public string greeting { get; set; }

        // Constructor
        public Enemy(string name, int health, string greeting)
        {
            this.name = name;
            this.health = health;
            this.greeting = greeting;
            Console.WriteLine(this.Greeting()); // display details when spawned
        }

        // Greeting
        public string Greeting()
        {
            return String.Format("A {0} ({1}) appears, it yells '{2}'", this.name, this.health, this.greeting);
        }

        public string Details()
        {
            return String.Format("Enemy {0} ({1})", this.name, this.health);
        }

    }

    class Dungeon
    {
        public Player player { get; set; }
        public string name { get; set; }
        public int depth { get; set; }

        // Constructor
        public Dungeon(Player player, int depth)
        {
            this.player = player;
            this.depth = depth;

            // If 1, start Rocky Cave
            if (this.depth == 1)
            {
                this.name = "Rocky Cave";
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(this.entryMessage());

                Enemy enemyRat = new Enemy("Giant Rat", 5, "Sweeeeeek!");
                Battle battleRat = new Battle(player, enemyRat);

                Enemy enemySkeletonGrunt = new Enemy("Skeleton Grunt", 10, "Prepare to Die!");
                Battle battleSkeletonGrunt = new Battle(player, enemySkeletonGrunt);

                Enemy enemySkeletonBoss = new Enemy("Skeleton Boss", 15, "Time to Fight!");
                Battle battleSkeletonBoss = new Battle(player, enemySkeletonBoss);
            }
            else if (this.depth == 2)
            {
                this.name = "Crystal Cavern";
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(this.entryMessage());

                Enemy enemyGolem = new Enemy("Crystal Golem", 5, "Roar!");
                Battle battleGolem = new Battle(player, enemyGolem);

            }

        }

        // Entry Message
        public string entryMessage()
        {
            return "\n~~~ " + this.player.name.ToUpper() + " ENTERS THE " + this.name.ToUpper() + " DUNGEON! ~~~\n";
        }

    }

    class Battle
    {
        public Player player { get; set; }
        public Enemy enemy { get; set; }
        public bool turn { get; set; } // T=player, F=enemy
        public int playerAttack { get; set; }

        // Constructor
        public Battle(Player player, Enemy enemy)
        {
            this.player = player;
            this.enemy = enemy;
            this.turn = true; // player start

            Console.WriteLine("{0} prepares to battle the {1}!", this.player.name, this.enemy.name);

            // Battle Loop
            while (this.player.health > 0 && this.enemy.health > 0)
            {
                // Player Turn
                if (this.turn == true)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n" + this.player.details());
                    Console.WriteLine("Choose your attack (1 = weapon, 2 = special)");
                    this.playerAttack = Convert.ToInt32(Console.ReadLine());
                    if (this.playerAttack == 1)
                    {
                        Console.WriteLine("{0} swings weapon at enemy {1}!", this.player.name, this.enemy.name);
                        this.Damage(1);
                    }
                    else if (this.playerAttack == 2)
                    {
                        Console.WriteLine("{0} fires a special attack at enemy {1}!", this.player.name, this.enemy.name);
                        this.Damage(5);
                    }
                    else
                    {
                        Console.WriteLine("{0} isn't paying attention, misses enemy {1}!", this.player.name, this.enemy.name);
                    }
                    this.turn = false;
                }
                // Enmey Turn
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n{0} attacks {1}", this.enemy.Details(), this.player.name);
                    this.Damage(this.randomNumberGenerator(0,5));
                    this.turn = true;
                }
                Console.ForegroundColor = ConsoleColor.White;
            }

            
        }

        // Random Number Generator
        private static Random rand = new Random();
        public int randomNumberGenerator(int start, int end)
        {
            int num = rand.Next(start, end);
            return num;
        }

        // Player Attack Damage
        public void Damage(int amount)
        {
            int rngHit = this.randomNumberGenerator(0, 2); // 0/1
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (rngHit > 0)
            {
                Console.WriteLine("Attack his for {0} damage!", amount);
                if (this.turn == true)
                {
                    this.enemy.health -= amount;
                    if (this.enemy.health <= 0)
                        this.endBattleMessage(true);
                }
                else
                {

                    this.player.health -= amount;
                    if (this.player.health <= 0)
                        this.endBattleMessage(false);
                }
            }
            // Miss
            else
            {
                Console.WriteLine("Attack missed!");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        // End Battle Message
        public void endBattleMessage(bool state)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            if (state)
                Console.WriteLine("\n=== ({0} has defeated {1}! ===\n", this.player.name, this.enemy.name);
            else
                Console.WriteLine("\n=== {0} has defeated {1}! ===\nGAME OVER", this.enemy.name, this.player.name);
        }

    }

}
