using System.Collections.Immutable;
using Map = System.Collections.Immutable.ImmutableDictionary<int, System.Collections.Generic.List<int>>;

namespace AdventOfCode._2024.Day05;

public class Solution : ISolution
{
    public object PartOne(string input)
    {
        var (updates, map) = ParseInput(input);
        return GetCorrectUpdates(updates, map).Select(update => update.Split(","))
            .Select(numbers => int.Parse(numbers[numbers.Length / 2])).Sum();
    }

    public object PartTwo(string input)
    {
        var (updates, map) = ParseInput(input);
        var incorrectUpdates = updates.Except(GetCorrectUpdates(updates, map));

        var result = 0;
        foreach (var incorrectUpdate in incorrectUpdates)
        {
            var numbers = incorrectUpdate.Split(',').Select(int.Parse).ToList();

            numbers.Sort((a, b) => map[a].Contains(b) ? -1 : 1);

            result += numbers[numbers.Count / 2];
        }

        return result;
    }

    private static List<string> GetCorrectUpdates(List<string> updates, Map rules) => updates.Where(update =>
            update.Split(",").Select(int.Parse)
                .Select((pageNumber, index) => (pageNumber, index))
                .All(item => rules[item.pageNumber]
                    .All(rule => !string.Join(",", update.Split(",").Take(item.index))
                        .Contains(rule.ToString()))))
        .ToList();

    private static (List<string>, Map) ParseInput(string input)
    {
        var parts = input.Split("\n\n");
        var dict = new Dictionary<int, List<int>>();

        parts[0].Split("\n")
            .Where(line => line.Contains('|'))
            .Select(line => line.Split('|'))
            .ToList()
            .ForEach(nums =>
                (dict.GetValueOrDefault(int.Parse(nums[0])) ?? (dict[int.Parse(nums[0])] = []))
                .Add(int.Parse(nums[1])));

        var updates = parts[1].Split("\n").ToList();

        return (updates, dict.ToImmutableDictionary());
    }
}