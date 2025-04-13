using System.Diagnostics;
namespace MathGame.glaxxie;

internal class Menu
{
    internal static string _categories = string.Join(Environment.NewLine, new[]
    {
        "Which category of math game you like to play today?",
        "Please choose from the options below (or 'q' to quit):",
        $"{"-".PadRight(55,'-')}",
        "\t1. Addition: get sum of two numbers",
        "\t2. Subtraction: get the diffence of two numbers",
        "\t3. Multiplication: get the product of two numbers",
        "\t4. Division: get the product of two numbers",
        "\t5. Prime: check for prime number (1 -> true, 0 -> false)",
        "\t6. Random: play one of the game mode randomly",
        "\t7. History: show history of the game"
    });

    internal static string _difficulties = string.Join(Environment.NewLine, new[]
    {
        "Which difficulty do you want to play (or press 'q' to quit):",
        $"{"-".PadRight(60,'-')}",
        "\t1 - Easy",
        "\t2 - Medium",
        "\t3 - Hard"
    });

    public static void MainMenu()
    {
        Console.Clear();
        int gameMode = GetMenuChoice(_categories, "1234567");
        if (gameMode == 7)
        {
            Utilities.ShowHistory();
            return;
        }
        Thread.Sleep(200);
        int difficulty = GetMenuChoice(_difficulties, "123");

        DelayClear();
        Console.WriteLine($"Preparing {(GameMode)gameMode} at {(Difficulty)difficulty} difficulty...");
        DelayClear(450);
        //
        int rounds = 10;
        Play(gameMode, difficulty, rounds);
    }
    
    static int GetMenuChoice(string menuText, string options)
    {
        string? userInput;
        Console.Clear();
        Console.WriteLine(menuText);
        do
        {
            Console.Write("\n>> ");
            userInput = Console.ReadLine();
            if (userInput?.Trim().ToLower() == "q")
                Environment.Exit(0);
            if (InvalidInput(userInput, options))
                Console.WriteLine("Invalid input. Please try again or press 'q' to quit.");
        } while (InvalidInput(userInput, options));
        return int.Parse(userInput!);
    }

    static bool InvalidInput(string? gameMode, string options)
        => string.IsNullOrWhiteSpace(gameMode) || !options.Contains(gameMode.Trim());


    public static void Play(int gameMode, int difficulty, int rounds)
    {
        Game session = new(gameMode, difficulty);
        
        for (int i = 0; i < rounds; i++)
        {
            Round round = session.Play();
            Console.WriteLine(round);
            
            Stopwatch timer = Stopwatch.StartNew();
            DrawProgress(i);
            Console.Write(">> ");
            string? answer = Console.ReadLine();
            timer.Stop();

            session.UpdateTimer(timer.Elapsed.TotalSeconds);
            
            if (answer == "q")
            {
                Console.WriteLine("Abandoned game.");
                round.Update("Abd", timer.Elapsed.TotalSeconds);
                break;
            }

            if (answer != null && round.CheckResult(answer))
            {
                session.UpdateScore();
                Console.WriteLine("\nCorrect!");
            } else
            {
                Console.WriteLine($"\nIncorrect! The answer is: {round.GetResult()}");
            }
            round.Update(answer, timer.Elapsed.TotalSeconds);
            session.AddRecord(round);
            DelayClear(450);
        }

        Utilities.UpdateHistory(session);

        Console.WriteLine("Game has finished, thank you for playing!");
        Console.WriteLine(session.History());

        Console.WriteLine($"{"-".PadRight(60, '-')}");
        ReturnToMenu();
    }

    internal static void ReturnToMenu()
    {
        Console.WriteLine("Please any key to go back to main menu");
        Console.ReadLine();
    }

    internal static void DelayClear(int time = 300)
    {
        Thread.Sleep(time);
        Console.Clear();
    }

    public static void DrawProgress(int round)
    {
        string msg = $"Round {round + 1}/10";
        int right = Console.WindowWidth - msg.Length;
        int bottom = Console.WindowHeight - 1;
        Console.SetCursorPosition(right, bottom);
        Console.Write(msg);
        Console.SetCursorPosition(0, 1);
    }
}
