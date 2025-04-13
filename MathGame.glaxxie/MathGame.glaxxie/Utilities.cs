namespace MathGame.glaxxie;

internal class Utilities
{
    internal static List<Game> history = [];

    internal static bool IsPrime(int number)
        => Enumerable.Range(2, (int)Math.Sqrt(number) - 1).All(x => number % x != 0);

    internal static void UpdateHistory(Game game) => history.Add(game);

    // Display history
    internal static void ShowHistory()
    {
        Console.Clear();
        if (history.Count == 0)
        {
            Console.WriteLine("You haven't played any game yet");
            Menu.ReturnToMenu();
            return;
        }

        Console.WriteLine("Do you want the full details? (y for yes / anything else for no)");
        Console.Write(">> ");
        string? ans = Console.ReadLine();
        bool full = ans != null && ans.Equals("y", StringComparison.CurrentCultureIgnoreCase);

        Console.WriteLine("Game history");
        Console.WriteLine("-".PadRight(70, '-'));
        foreach (Game game in history)
        {
            Console.WriteLine(game.History());
            if (!full) continue;
            foreach (Round round in game.ShowRecord())
                Console.WriteLine(round.History());
        }
        
        Console.WriteLine("-".PadRight(70, '-'));
        Menu.ReturnToMenu();
    }

    // get operands depend on operations and difficulty
    internal static (int, int, int) GetDivisionOperands(Difficulty dif)
    {
        Random random = new();
        int divisor = dif switch
        {
            Difficulty.Easy     => random.Next(1, 21),
            Difficulty.Medium   => random.Next(20, 51),
            _                   => random.Next(50, 100),
        };

        int quotient = dif switch
        {
            Difficulty.Easy     => random.Next(1, 10),
            Difficulty.Medium   => random.Next(1, 50),
            _                   => random.Next(1, 100),
        };
        return (divisor * quotient, divisor, quotient);
    }
}
