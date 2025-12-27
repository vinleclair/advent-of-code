namespace AdventOfCode._2025.Day11;

public class Solution : ISolution
{
    [Flags]
    private enum Checkpoints
    {
        None = 0,
        Dac = 1,
        Fft = 2,
        All = Dac | Fft
    }

    public object PartOne(string input) => GetPaths(ParseInput(input), "you", "out", Checkpoints.None);

    public object PartTwo(string input) => GetPaths(ParseInput(input), "svr", "out", Checkpoints.All);

    private static long GetPaths(Dictionary<string, string[]> devices, string start, string end,
        Checkpoints? checkpoints)
    {
        var cache = new Dictionary<(string, Checkpoints), long>();

        return GetPathsAux(start, Checkpoints.None);

        long GetPathsAux(string position, Checkpoints passed)
        {
            switch (position)
            {
                case "dac":
                    passed |= Checkpoints.Dac;
                    break;
                case "fft":
                    passed |= Checkpoints.Fft;
                    break;
            }

            if (cache.TryGetValue((position, passed), out var value))
                return value;

            cache[(position, passed)] = position == end
                ? (passed & checkpoints) == checkpoints ? 1 : 0
                : (devices.GetValueOrDefault(position) ?? []).Sum(device => GetPathsAux(device, passed));

            return cache[(position, passed)];
        }
    }

    private static Dictionary<string, string[]> ParseInput(string input) => input.Split("\n")
        .Select(line => line.Split(':'))
        .ToDictionary(parts => parts[0], parts => parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries));
}