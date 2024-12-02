namespace AdventOfCode._2024.Day01;

public class Solution : ISolution 
{
    public object PartOne(string input) => GetNumbers(input, 0).Zip(GetNumbers(input, 1)).Select(n => Math.Abs(n.First - n.Second)).Sum();

    public object PartTwo(string input) => (from number in GetNumbers(input, 0) let count = GetNumbers(input, 1).Count(n => n == number) select number * count).Sum();

    private IEnumerable<int> GetNumbers(string input, int column)
    {
        return input.Split("\n").Select(line => line.Split(" ", 2, StringSplitOptions.RemoveEmptyEntries)).Select(split => int.Parse(split[column])).OrderBy(n => n).ToList();
    }
}