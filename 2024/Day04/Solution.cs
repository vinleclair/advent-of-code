using System.Collections.Immutable;
using System.Numerics;
using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Vector2, char>;

namespace AdventOfCode._2024.Day04;

public class Solution : ISolution
{
    private readonly Vector2 _up = new(0, 1);
    private readonly Vector2 _down = new(0, -1);
    private readonly Vector2 _left = new(-1, 0);
    private readonly Vector2 _right = new(1, 0);

    public object PartOne(string input)
    {
        var map = ParseInput(input);

        return map.Keys
            .SelectMany(_ => new[] { _right, _right + _down, _down + _left, _down },
                (coords, dir) => new { coords, dir })
            .Where(x => Match(map, x.coords, x.dir, "XMAS"))
            .Select(_ => 1)
            .Count();
    }

    public object PartTwo(string input)
    {
        var map = ParseInput(input);

        return map.Keys
            .Where(coords =>
                Match(map, coords + _up + _left, _down + _right, "MAS") &&
                Match(map, coords + _down + _left, _up + _right, "MAS"))
            .Select(_ => 1)
            .Count();
    }

    private static bool Match(Map map, Vector2 coordinates, Vector2 direction, string pattern) =>
        pattern.Select((_, index) => map.GetValueOrDefault(coordinates + index * direction))
            .ToArray() is var chars &&
        chars.SequenceEqual(pattern) || chars.SequenceEqual(pattern.Reverse());

    private static Map ParseInput(string input) => input.Split("\n")
        .SelectMany((line, y) => line.Select((c, x) => new KeyValuePair<Vector2, char>(new Vector2(x, y), c)))
        .ToImmutableDictionary();
}