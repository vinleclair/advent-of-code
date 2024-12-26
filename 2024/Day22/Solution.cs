namespace AdventOfCode._2024.Day22;

public class Solution : ISolution
{
    public object PartOne(string input) => ParseInput(input).Select(n => GetSecretNumbers(n, 2000).Last()).Sum();

    public object PartTwo(string input) => ParseInput(input)
        .SelectMany(BuyingOptions)
        .GroupBy(option => option.Key, option => option.Value)
        .ToDictionary(group => group.Key, group => group.Sum())
        .Values.Max();

    private static Dictionary<string, long> BuyingOptions(long seed)
    {
        var bananas = Bananas(seed);
        var buyOptions = new Dictionary<string, long>();

        for (var i = 5; i < bananas.Length; i++)
        {
            var slice = bananas[(i - 5) .. i];
            var seq = string.Join(",", Difference(slice.ToList()));
            if (!buyOptions.ContainsKey(seq))
                buyOptions[seq] = slice.Last();
        }

        return buyOptions;
    }

    private static long[] Bananas(long initialNumber) =>
        GetSecretNumbers(initialNumber, 2000).Select(n => n % 10).ToArray();

    private static long[] Difference(List<long> nums) =>
        nums.Zip(nums.Skip(1)).Select(p => p.Second - p.First).ToArray();

    private static IEnumerable<long> GetSecretNumbers(long initialNumber, int numberCount)
    {
        yield return initialNumber;

        for (var i = 0; i < numberCount; i++)
        {
            initialNumber = Prune(Mix(initialNumber, initialNumber << 6));
            initialNumber = Prune(Mix(initialNumber, initialNumber >> 5));
            initialNumber = Prune(Mix(initialNumber, initialNumber << 11));
            yield return initialNumber;
        }

        yield break;

        long Mix(long value, long secretNumber) => value ^ secretNumber;
        long Prune(long secretNumber) => secretNumber % 16777216;
    }

    private static IEnumerable<long> ParseInput(string input) => input.Split("\n").Select(long.Parse);
}