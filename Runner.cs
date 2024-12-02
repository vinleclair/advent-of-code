using System.Diagnostics;

namespace AdventOfCode;

public static class Runner
{
    private static string GetNormalizedInput(string file)
    {
        var input = File.ReadAllText(file).Replace("\r", "");

        if (input.EndsWith(Environment.NewLine))
            input = input[..^1];

        return input;
    }

    private static void RunSolution(ISolution solution)
    {
        var workingDir = solution.WorkingDir();
        WriteLine(ConsoleColor.White, $"{solution.DayName()}");
        var file = Path.Combine(workingDir, "input.txt");
        var input = GetNormalizedInput(file);
        var stopwatch = Stopwatch.StartNew();
        foreach (var line in solution.Solve(input))
        {
            var ticks = stopwatch.ElapsedTicks;

            Console.Write($" {line} ");
            var diff = ticks * 1000.0 / Stopwatch.Frequency;

            WriteLine(
                diff > 1000 ? ConsoleColor.Red :
                diff > 500 ? ConsoleColor.Yellow :
                ConsoleColor.DarkGreen,
                $"({diff:F3} ms)"
            );
            stopwatch.Restart();
        }
    }

    public static void RunAll(params ISolution[] solutions)
    {
        foreach (var solution in solutions)
            RunSolution(solution);
    }

    private static void WriteLine(ConsoleColor color = ConsoleColor.Gray, string text = "") => Write(color, text + "\n");

    private static void Write(ConsoleColor color = ConsoleColor.Gray, string text = "")
    {
        var c = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = c;
    }
}