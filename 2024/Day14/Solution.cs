using System.Numerics;
using System.Text.RegularExpressions;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Vector2, int>;

// ReSharper disable PossibleLossOfFraction

namespace AdventOfCode._2024.Day14;

internal record struct Robot(Vector2 Position, Vector2 Velocity);

public partial class Solution : ISolution
{
    private const int Width = 101;
    private const int Height = 103;
    private const int Seconds = 100;
    private static readonly Vector2 Up = new(0, 1);

    public object PartOne(string input) =>
        GetSafetyFactor(Simulate(ParseInput(input).ToArray(), CreateMap(), Seconds, out _));

    public object PartTwo(string input)
    {
        Simulate(ParseInput(input).ToArray(), CreateMap(), 10000, out var easterEggSecond);
        return easterEggSecond;
    }

    private static Map Simulate(Robot[] robots, Map map, int seconds, out int easterEggSecond)
    {
        for (var s = 0; s <= seconds; s++)
        for (var r = 0; r < robots.Length; r++)
        {
            if (s != 0)
            {
                map[robots[r].Position] -= 1;
                robots[r].Position = new Vector2((robots[r].Position.X + robots[r].Velocity.X + Width) % Width,
                    (robots[r].Position.Y + robots[r].Velocity.Y + Height) % Height);
            }

            map[robots[r].Position] += 1;

            if (s <= 100) continue;

            if (!CheckForEasterEgg(map)) continue;

            easterEggSecond = s;
            return map;
        }

        easterEggSecond = 0;

        return map;
    }

    //TODO Optimize this bad boy somehow 
    private static bool CheckForEasterEgg(Map map)
    {
        foreach (var point in map.Where(p => p.Value > 0))
        {
            var currentPosition = point.Key;
            var isEasterEgg = true;

            for (var i = 0; i < 10; i++)
            {
                if (map.GetValueOrDefault(currentPosition) <= 0)
                {
                    isEasterEgg = false;
                    break;
                }

                currentPosition += Up;
            }

            if (isEasterEgg)
                return true;
        }

        return false;
    }

    private static int GetSafetyFactor(Map map) =>
        new[]
            {
                map.Where(p => p.Key is { X: < Width / 2, Y: < Height / 2 }),
                map.Where(p => p.Key is { X: > Width / 2, Y: < Height / 2 }),
                map.Where(p => p.Key is { X: < Width / 2, Y: > Height / 2 }),
                map.Where(p => p.Key is { X: > Width / 2, Y: > Height / 2 })
            }.Select(quadrant => quadrant.Sum(p => p.Value))
            .Aggregate(1, (a, b) => a * b);

    private static Map CreateMap() => Enumerable.Range(0, Width)
        .SelectMany(x => Enumerable.Range(0, Height), (x, y) => new KeyValuePair<Vector2, int>(new Vector2(x, y), 0))
        .ToDictionary();

    private static IEnumerable<Robot> ParseInput(string input) => input.Split("\n")
        .Select(line => PositiveAndNegativeNumbers().Matches(line).Select(m => int.Parse(m.Value)).ToArray())
        .Select(numbers => new Robot(new Vector2(numbers[0], numbers[1]), new Vector2(numbers[2], numbers[3])));

    [GeneratedRegex(@"-?\d+")]
    private static partial Regex PositiveAndNegativeNumbers();
}