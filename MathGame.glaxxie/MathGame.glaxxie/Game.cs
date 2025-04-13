namespace MathGame.glaxxie;

internal enum GameMode
{
    Addition = 1,
    Subtraction,
    Multiplication,
    Division,
    Prime,
    Random
}

internal enum Difficulty
{
    Easy = 1,
    Medium,
    Hard
}

internal class Game(int gameMode, int difficulty)
{
    private GameMode Mode { get; } = (GameMode)gameMode;
    private Difficulty Difficulty { get; } = (Difficulty)difficulty;
    private int Score { get; set; }
    private double TotalTime { get; set; }
    private List<Round> Record { get; set; } = [];

    // play the rounds
    public Round Play() => new(Mode, Difficulty);
    public void UpdateScore() => Score++;
    public void UpdateTimer(double time) => TotalTime += time;
    public int GetScore() => Score;
    public double GetTime() => TotalTime;
    public void AddRecord(Round round) => Record.Add(round);
    public string History() => $"{Mode} game at {Difficulty} difficulty. Score: {Score} in {TotalTime:F2} secs";
    internal List<Round> ShowRecord() => Record;
}

internal class Round
{
    private GameMode Mode { get; }
    private Difficulty Dif { get; }
    private double Time { get; set; }
    private int Op1 { get; set; }
    private int Op2 { get; set; }
    private int Result { get; set; }
    private string Guess { get; set; }
    private char Operator { get; set; }
    private char[] Operators { get; } = [' ', '+', '-', '*', '/', 'P'];

    public Round(GameMode mode, Difficulty difficulty)
    {
        Guess = "--";

        Dif = difficulty;

        Random random = new();
        int max = (int)Math.Pow(10, (int)difficulty + 1);
        // random mode
        mode = mode == GameMode.Random ? (GameMode)random.Next(1, 6) : mode;
        Operator = Operators[(int)mode];
        Mode = mode;

        if (mode == GameMode.Division)
        {
            (Op1, Op2, Result) = Utilities.GetDivisionOperands(difficulty);
        }
        else
        {
            Op1 = random.Next(0, max);
            Op2 = random.Next(0, max);

            Result = mode switch
            {
                GameMode.Addition => Op2 + Op1,
                GameMode.Subtraction => Op1 - Op2,
                GameMode.Multiplication => Op1 * Op2,
                _ => Utilities.IsPrime(Math.Max(Op1, Op2)) ? 1 : 0,
            };
        }
    }

    public void Update(string? answer, double time)
    {
        Guess = answer?? "--";
        Time = time;
    }

    public bool CheckResult(string input)
    {
        if (!int.TryParse(input, out var result))
            return false;
        return result == Result;
    }

    public int GetResult() => Result;
    public string History() => $" - {Mode,-15} {Dif,-8} Guess: {Guess,-5} Result: {Result,-5} Time: {Time,6:F2}s";
    public override string ToString() => Mode switch
    {
        GameMode.Prime  => $"{Math.Max(Op1, Op2),5} Prime?",
        _               => $"{Op1,5} {Operator} {Op2} = ?"
    };
}
