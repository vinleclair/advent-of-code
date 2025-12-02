using System.Text.RegularExpressions;

namespace AdventOfCode._2025.Day02;

public partial class Solution : ISolution
{
    public object PartOne(string input) =>
        CountMatchingIds(ParseInput(input), HasRepeatingPattern);

    public object PartTwo(string input) =>
        CountMatchingIds(ParseInput(input), HasMultipleRepeatingPattern);

    private static long CountMatchingIds(
        IEnumerable<(long firstId, long lastId)> ranges,
        Func<Regex> predicate)
    {
        long count = 0;
        foreach (var (firstId, lastId) in ranges)
        {
            for (var id = firstId; id <= lastId; id++)
            {
                if (predicate.Invoke().IsMatch(id.ToString()))
                    count += id;
            }
        }

        return count;
    }

    private static IEnumerable<(long firstId, long lastId)> ParseInput(string input) =>
        input.Split(",")
            .Select(range =>
            {
                var ids = range.Split("-");
                return (long.Parse(ids[0]), long.Parse(ids[1]));
            });

    [GeneratedRegex(@"^(\d+)\1$")]
    private static partial Regex HasRepeatingPattern();

    [GeneratedRegex(@"^(.+)\1+$")]
    private static partial Regex HasMultipleRepeatingPattern();
}