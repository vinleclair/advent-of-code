using System.Text.RegularExpressions;

namespace AdventOfCode._2024.Day03;

public partial class Solution : ISolution
{
    public object PartOne(string input) => ParseInput(input).SelectMany(line => MulRegex().Matches(line))
        .Sum(mulMatch =>
            int.Parse(mulMatch.Groups[1].Captures[0].Value) * int.Parse(mulMatch.Groups[2].Captures[0].Value));

    public object PartTwo(string input) => ParseInput(input)
        .SelectMany(line => MulDoDontRegex().Matches(line))
        .Aggregate((result: 0, enabled: true), (acc, match) => match.ToString() switch
        {
            "do()" => (acc.result, true),
            "don't()" => (acc.result, false),
            _ => (
                acc.enabled
                    ? acc.result + int.Parse(match.Groups[2].Captures[0].Value) *
                    int.Parse(match.Groups[3].Captures[0].Value)
                    : acc.result, acc.enabled)
        }, acc => acc.result);

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex MulRegex();

    [GeneratedRegex(@"(mul\((\d{1,3}),(\d{1,3})\))|(don't\(\))|(do\(\))")]
    private static partial Regex MulDoDontRegex();

    private static string[] ParseInput(string input) => input.Split("\n");
}