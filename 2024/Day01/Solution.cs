namespace AdventOfCode._2024.Day01;

public class Solution : ISolution 
{
    public object PartOne(string input) => ParseInput(input, 0).Zip(ParseInput(input, 1)).Select(n => Math.Abs(n.First - n.Second)).Sum();

    public object PartTwo(string input) => (from number in ParseInput(input, 0) let count = ParseInput(input, 1).Count(n => n == number) select number * count).Sum();

    private static IEnumerable<int> ParseInput(string input, int column) => input.Split("\n").Select(line => line.Split(" ", 2, StringSplitOptions.RemoveEmptyEntries)).Select(split => int.Parse(split[column])).OrderBy(n => n);
}