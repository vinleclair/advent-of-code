using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode._2024.Day03;

public partial class Solution : ISolution
{
    public object PartOne(string input) => ParseInput(input).SelectMany(line => MulRegex().Matches(line)).Sum(mulMatch => int.Parse(mulMatch.Groups[1].Captures[0].Value) * int.Parse(mulMatch.Groups[2].Captures[0].Value));

    public object PartTwo(string input)
    {
        var result = 0;
        var isEnabled = true;
        foreach (var line in ParseInput(input))
        {
            var matches = MulDoDontRegex().Matches(line);
            foreach (Match mulDoDontMatch in matches)
            {
                switch (mulDoDontMatch.ToString())
                {
                    case "do()":
                        isEnabled = true;
                        break;
                    case "don't()":
                        isEnabled = false;
                        break;
                    default:
                    {
                        if (isEnabled)
                            result += int.Parse(mulDoDontMatch.Groups[2].Captures[0].Value) *
                                      int.Parse(mulDoDontMatch.Groups[3].Captures[0].Value);
                        break;
                    }
                }
            }
        }

        return result;
    }

    private static string[] ParseInput(string input) => input.Split("\n").ToArray();
    
    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex MulRegex();

    [GeneratedRegex(@"(mul\((\d{1,3}),(\d{1,3})\))|(don't\(\))|(do\(\))")]
    private static partial Regex MulDoDontRegex();
}