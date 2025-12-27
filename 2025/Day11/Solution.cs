namespace AdventOfCode._2025.Day11;

public class Solution : ISolution
{
    public object PartOne(string input) => GetPaths(ParseInput(input), "you", "out");

    private static int GetPaths(Dictionary<string, string[]> devices, string position, string end) =>
        devices[position].Contains(end) ? 1 : devices[position].Sum(device => GetPaths(devices, device, end));

    private static Dictionary<string, string[]> ParseInput(string input) => input.Split("\n")
        .Select(line => line.Split(':'))
        .ToDictionary(parts => parts[0], parts => parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries));
}