using System.Text.RegularExpressions;

namespace AdventOfCode._2024.Day13;

using Machine = (Vector2Long A, Vector2Long B, Vector2Long Prize);

internal record struct Vector2Long(long X, long Y);

public partial class Solution : ISolution
{
    private const long CostA = 3;
    private const long CostB = 1;
    private const long UnitConversionError = 10000000000000;

    public object PartOne(string input) => ParseInput(input).Sum(GetPrize);
    public object PartTwo(string input) => ConversionError(ParseInput(input)).Sum(GetPrize);

    private static long GetPrize(Machine machine)
    {
        var (buttonA, buttonB, prize) = machine;

        var pushA = Determinant(prize, buttonB) / Determinant(buttonA, buttonB);
        var pushB = Determinant(buttonA, prize) / Determinant(buttonA, buttonB);

        if (pushA < 0 ||
            pushB < 0 ||
            buttonA.X * pushA + buttonB.X * pushB != prize.X ||
            buttonA.Y * pushA + buttonB.Y * pushB != prize.Y) return 0;

        return pushA * CostA + pushB * CostB;
    }

    private static long Determinant(Vector2Long v1, Vector2Long v2) => v1.X * v2.Y - v1.Y * v2.X;
    
    private static IEnumerable<Machine> ConversionError(IEnumerable<Machine> machines) => machines.Select(machine =>
        (machine.A, machine.B,
            new Vector2Long(machine.Prize.X + UnitConversionError, machine.Prize.Y + UnitConversionError)));

    private static IEnumerable<Machine> ParseInput(string input) => input.Split("\n\n").Select(block =>
        OneOrMoreDigits().Matches(block)
            .Select(m => long.Parse(m.Value))
            .Chunk(2).Select(p => new Vector2Long(p[0], p[1]))
            .ToArray()).Select(nums => (nums[0], nums[1], nums[2]));

    [GeneratedRegex(@"\d+", RegexOptions.Multiline)]
    private static partial Regex OneOrMoreDigits();
}