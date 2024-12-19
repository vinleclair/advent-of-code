using Cache = System.Collections.Concurrent.ConcurrentDictionary<string, long>;

namespace AdventOfCode._2024.Day19;

public class Solution : ISolution
{
    public object PartOne(string input)
    {
        var (patterns, designs) = ParseInput(input);

        return designs.Count(design => Combinations(patterns, design, new Cache()) > 0);
    }

    public object PartTwo(string input)
    {
        var (patterns, designs) = ParseInput(input);

        return designs.Sum(design => Combinations(patterns, design, new Cache()));
    }

    private static long Combinations(HashSet<string> patterns, string design, Cache cache) => cache.GetOrAdd(design,
        towel => towel == string.Empty
            ? 1
            : patterns.Where(towel.StartsWith).Sum(pattern => Combinations(patterns, towel[pattern.Length..], cache)));

    private static (HashSet<string> patterns, string[] designs) ParseInput(string input) =>
        input.Split("\n\n") is var blocks
            ? (blocks[0].Split(',').Select(s => s.Trim()).ToHashSet(), blocks[1].Split('\n'))
            : default;
}