using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOP_Football_Manager
{
    // ================= PERSON =================
    class Person
    {
        public string Name { get; set; }
        public string Nationality { get; set; }
        public int Age { get; set; }

        public Person(string name, string nationality, int age)
        {
            Name = name;
            Nationality = nationality;
            Age = age;
        }
    }

    // ================= STATS =================
    class Stats
    {
        public int Speed, Stamina, Technique, ShotPower;

        public double Rating =>
            (Speed + Stamina + Technique + ShotPower) / 4.0;

        public Stats(int speed, int stamina, int technique, int shotPower)
        {
            Speed = speed;
            Stamina = stamina;
            Technique = technique;
            ShotPower = shotPower;
        }
    }

    enum Position
    {
        Goalkeeper = 1,
        Defender,
        Midfielder,
        Striker
    }

    // ================= PLAYER =================
    class Player : Person
    {
        public Position Position { get; set; }
        public double MarketValue { get; set; }
        public string Club { get; set; }
        public Stats Stats { get; set; }

        public Player(string name, string nationality, int age,
            Position position, double value, string club, Stats stats)
            : base(name, nationality, age)
        {
            Position = position;
            MarketValue = value;
            Club = club;
            Stats = stats;
        }

        public override string ToString()
        {
            return $"{Name,-15} | {Position,-10} | Rating: {Stats.Rating:F1} | €{MarketValue}M";
        }
    }

    // ================= COMPARATORS =================

    // Sort by price (ascending / descending)
    class PriceComparer : IComparer<Player>
    {
        private bool ascending;

        public PriceComparer(bool ascending)
        {
            this.ascending = ascending;
        }

        public int Compare(Player a, Player b)
        {
            return ascending
                ? a.MarketValue.CompareTo(b.MarketValue)
                : b.MarketValue.CompareTo(a.MarketValue);
        }
    }

    // Sort by rating
    class RatingComparer : IComparer<Player>
    {
        public int Compare(Player a, Player b)
        {
            return b.Stats.Rating.CompareTo(a.Stats.Rating);
        }
    }

    // Sort by name (alphabetical)
    class NameComparer : IComparer<Player>
    {
        public int Compare(Player a, Player b)
        {
            return a.Name.CompareTo(b.Name);
        }
    }

    // ================= TEAM =================
    class Team
    {
        public List<Player> Players { get; private set; } = new List<Player>();

        public void AddPlayer(Player p) => Players.Add(p);

        public void RemovePlayer(int index)
        {
            if (index >= 0 && index < Players.Count)
                Players.RemoveAt(index);
        }

        public bool Contains(Player p) => Players.Contains(p);

        public void PrintWithIndex()
        {
            for (int i = 0; i < Players.Count; i++)
                Console.WriteLine($"{i}. {Players[i]}");
        }

        // === SORT METHODS USING COMPARATORS ===
        public void SortByPrice(bool ascending)
        {
            Players.Sort(new PriceComparer(ascending));
        }

        public void SortByRating()
        {
            Players.Sort(new RatingComparer());
        }

        public void SortByName()
        {
            Players.Sort(new NameComparer());
        }
    }

    // ================= PROGRAM =================
    class Program
    {
        static Team database = new Team();
        static Team dreamTeam = new Team();

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            SeedPlayers();

            while (true)
            {
                ShowMenu();
                Console.Write("Избор: ");
                string choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1": ShowDatabase(); break;
                    case "2": AddPlayerFromConsole(); break;
                    case "3": RemovePlayer(); break;
                    case "4": AddToDreamTeam(); break;
                    case "5": ShowDreamTeam(); break;

                    case "6":
                        database.SortByPrice(true);
                        Console.WriteLine("Сортирано по цена (възходящо):");
                        ShowDatabase();
                        break;

                    case "7":
                        database.SortByPrice(false);
                        Console.WriteLine("Сортирано по цена (низходящо):");
                        ShowDatabase();
                        break;

                    case "8":
                        database.SortByRating();
                        Console.WriteLine("Сортирано по рейтинг:");
                        ShowDatabase();
                        break;

                    case "9":
                        database.SortByName();
                        Console.WriteLine("Сортирано по име:");
                        ShowDatabase();
                        break;

                    case "0": return;
                    default: Console.WriteLine("Невалиден избор!"); break;
                }

                Console.WriteLine("\nНатисни Enter...");
                Console.ReadLine();
                Console.Clear();
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("=== FOOTBALL SCOUT MANAGER ===");
            Console.WriteLine("1. Покажи всички играчи");
            Console.WriteLine("2. Добави нов играч");
            Console.WriteLine("3. Изтрий играч");
            Console.WriteLine("4. Добави играч в отбора (макс 10)");
            Console.WriteLine("5. Покажи отбора на мечтите");
            Console.WriteLine("6. Сортирай по цена (възходящо)");
            Console.WriteLine("7. Сортирай по цена (низходящо)");
            Console.WriteLine("8. Сортирай по рейтинг");
            Console.WriteLine("9. Сортирай по име");
            Console.WriteLine("0. Изход");
            Console.WriteLine();
        }

        static void ShowDatabase()
        {
            database.PrintWithIndex();
        }

        static void AddPlayerFromConsole()
        {
            Console.Write("Име: ");
            string name = Console.ReadLine();

            Console.Write("Националност: ");
            string nat = Console.ReadLine();

            Console.Write("Възраст: ");
            int age = int.Parse(Console.ReadLine());

            Console.WriteLine("Позиция: 1-Вратар 2-Защитник 3-Халф 4-Нападател");
            Position pos = (Position)int.Parse(Console.ReadLine());

            Console.Write("Цена (в млн): ");
            double value = double.Parse(Console.ReadLine());

            Console.Write("Клуб: ");
            string club = Console.ReadLine();

            Console.Write("Скорост: ");
            int sp = int.Parse(Console.ReadLine());
            Console.Write("Издръжливост: ");
            int st = int.Parse(Console.ReadLine());
            Console.Write("Техника: ");
            int te = int.Parse(Console.ReadLine());
            Console.Write("Шут: ");
            int sh = int.Parse(Console.ReadLine());

            database.AddPlayer(new Player(
                name, nat, age, pos, value, club,
                new Stats(sp, st, te, sh)));

            Console.WriteLine("✔ Играчът е добавен!");
        }

        static void RemovePlayer()
        {
            ShowDatabase();
            Console.Write("Избери индекс за изтриване: ");
            int index = int.Parse(Console.ReadLine());
            database.RemovePlayer(index);
            Console.WriteLine("✔ Играчът е изтрит!");
        }

        static void AddToDreamTeam()
        {
            if (dreamTeam.Players.Count >= 10)
            {
                Console.WriteLine("❌ Отборът вече има 10 играчи!");
                return;
            }

            ShowDatabase();
            Console.Write("Избери индекс за добавяне: ");
            int index = int.Parse(Console.ReadLine());

            if (index < 0 || index >= database.Players.Count)
            {
                Console.WriteLine("❌ Невалиден индекс!");
                return;
            }

            Player selected = database.Players[index];

            if (dreamTeam.Contains(selected))
            {
                Console.WriteLine("❌ Този играч вече е в отбора!");
                return;
            }

            dreamTeam.AddPlayer(selected);
            Console.WriteLine("✔ Добавен в отбора!");
        }
         
        static void ShowDreamTeam()
        {
            Console.WriteLine($"=== ОТБОР НА МЕЧТИТЕ ({dreamTeam.Players.Count}/10) ===");
            dreamTeam.PrintWithIndex();
        }
            






        // AI-generated data, reviewed by team
        static void SeedPlayers()
        {
            database.AddPlayer(new Player("Mbappe", "Франция", 25, Position.Striker, 180, "PSG", new Stats(98, 85, 90, 92)));
            database.AddPlayer(new Player("Haaland", "Норвегия", 24, Position.Striker, 170, "Man City", new Stats(90, 88, 85, 96)));
            database.AddPlayer(new Player("Salah", "Египет", 31, Position.Striker, 90, "Liverpool", new Stats(93, 85, 88, 90)));
            database.AddPlayer(new Player("Messi", "Аржентина", 36, Position.Midfielder, 50, "Inter Miami", new Stats(78, 80, 98, 85)));
            database.AddPlayer(new Player("De Bruyne", "Белгия", 33, Position.Midfielder, 90, "Man City", new Stats(75, 90, 95, 88)));
            database.AddPlayer(new Player("Van Dijk", "Нидерландия", 32, Position.Defender, 80, "Liverpool", new Stats(70, 85, 80, 75)));
            database.AddPlayer(new Player("Courtois", "Белгия", 31, Position.Goalkeeper, 45, "Real Madrid", new Stats(60, 90, 70, 65)));
        }
    }
}