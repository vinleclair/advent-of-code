namespace AdventOfCode._2025.Day03;

public class Solution : ISolution
{
    public object PartOne(string input) => ParseInput(input).Sum(bank => GetBatteries(bank, 2));

    public object PartTwo(string input) => ParseInput(input).Sum(bank => GetBatteries(bank, 12));

    private static long GetBatteries(int[] bank, int batteryCount)
    {
        if (bank.Length < batteryCount)
            throw new InvalidOperationException($"Bank contains less than {batteryCount} batteries");

        var joltage = string.Empty;

        while (batteryCount > 0)
        {
            var largestBatteryIndex = 0;

            for (var i = 0; i <= bank.Length - batteryCount; i++)
                if (bank[i] > bank[largestBatteryIndex])
                    largestBatteryIndex = i;

            joltage += bank[largestBatteryIndex];
            bank = bank[(largestBatteryIndex + 1)..];
            batteryCount--;
        }

        return long.Parse(joltage);
    }

    private static IEnumerable<int[]> ParseInput(string input) =>
        input.Split("\n").Select(bank => bank.Select(c => c - '0').ToArray());
}