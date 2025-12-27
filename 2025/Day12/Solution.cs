namespace AdventOfCode._2025.Day12;

public class Solution : ISolution
{
    public object PartOne(string input)
    {
        var (presents, regions) = ParseInput(input);

        return regions.Count(region => region.size > region.quantities.Zip(presents).Sum(p => p.First * p.Second));
    }

    private static (int[] presents, (int size, int[] quantities)[] regions) ParseInput(string input)
    {
        var sections = input.Split("\n\n");

        var presents = sections[..^1].Select(p => p.Count(c => c == '#')).ToArray();

        var regions = sections[^1].Split('\n').Select(ParseRegion).ToArray();

        return (presents, regions);
    }

    private static (int size, int[] quantities) ParseRegion(string line)
    {
        var parts = line.Split(':');
        var dimensions = parts[0].Split('x').Select(int.Parse);
        var size = dimensions.Aggregate((a, b) => a * b);
        var quantities = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        return (size, quantities);
    }
}