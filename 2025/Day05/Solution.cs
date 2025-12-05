namespace AdventOfCode._2025.Day05;

public class Solution : ISolution
{
    public object PartOne(string input)
    {
        var (freshIdRanges, availableIds) = ParseInput(input);

        var sum = 0;

        foreach (var id in availableIds)
        {
            foreach (var (start, end) in freshIdRanges)
            {
                if (id >= start && id <= end)
                {
                    sum++;
                    break;
                }
            }
        }

        return sum;
    }

    public object PartTwo(string input)
    {
        var (freshIngredients, _) = ParseInput(input);

        var sortedFreshIngredients = freshIngredients.OrderBy(r => r.start);

        long sum = 0;

        var freshRanges = new List<(long start, long end)>();

        foreach (var ingredient in sortedFreshIngredients)
        {
            if (freshRanges.Count == 0 || ingredient.start > freshRanges[^1].end)
            {
                freshRanges.Add((ingredient.start, ingredient.end));
            }
            else if (ingredient.end > freshRanges[^1].end)
            {
                freshRanges[^1] = (freshRanges[^1].start, ingredient.end);
            }
        }

        foreach (var (start, end) in freshRanges)
        {
            sum += end - start + 1;
        }

        return sum;
    }

    private static ((long start, long end)[] freshIdRanges, long[] availableIds) ParseInput(string input)
    {
        var ids = input.Split("\n\n");

        var freshIdRanges = ids[0].Split("\n").Select(range =>
        {
            var bounds = range.Split('-');
            return (long.Parse(bounds[0]), long.Parse(bounds[1]));
        }).ToArray();

        var availableIds = ids[1].Split("\n").Select(long.Parse).ToArray();

        return (freshIdRanges, availableIds);
    }
}