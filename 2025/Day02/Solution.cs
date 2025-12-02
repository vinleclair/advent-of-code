namespace AdventOfCode._2025.Day02;

public class Solution : ISolution
{
    
    public object PartOne(string input)
    {
        long sum = 0;
        
        var ranges = ParseInput(input);
        
        foreach (var range in ranges)
            for (var i = range.firstId; i <= range.lastId; i++)
                if (HasMatchingHalves(i))
                    sum += i;

        return sum;
    }

    private static bool HasMatchingHalves(long number)
    {
        var s = Math.Abs(number).ToString();

        if (s.Length % 2 != 0) return false;

        var mid = s.Length / 2;
        return s.AsSpan(0, mid).SequenceEqual(s.AsSpan(mid));
    }


    private static IEnumerable<(long firstId, long lastId)> ParseInput(string input) => input.Split(",")
        .Select(range =>
        {
            var ids = range.Split("-");
            return (long.Parse(ids[0]), long.Parse(ids[1]));
        });
}