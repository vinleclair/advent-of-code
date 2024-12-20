using System.Collections.Immutable;
using System.Numerics;

namespace AdventOfCode._2024.Day20;

public class Solution : ISolution
{
    public object PartOne(string input) => DisableCollision(ParseInput(input), 2);
    public object PartTwo(string input) => DisableCollision(ParseInput(input), 20);

    private static int DisableCollision(Vector2[] path, int seconds)
    {
        return Enumerable.Range(0, path.Length).AsParallel().Select(Cheat).Sum();

        int Cheat(int i) =>
            Enumerable
                .Range(0, i)
                .Select(j => new
                {
                    Distance = ManhattanDistance(path[i], path[j]),
                    Saving = i - (j + ManhattanDistance(path[i], path[j]))
                })
                .Count(t => t.Distance <= seconds && t.Saving >= 100);
    }

    private static int ManhattanDistance(Vector2 a, Vector2 b) => (int)(Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y));

    private static Vector2[] ParseInput(string input)
    {
        var map = input.Split('\n')
            .SelectMany((line, y) => line.Select((c, x) => (Position: new Vector2(x, y), Char: c)))
            .ToImmutableDictionary(item => item.Position, item => item.Char);

        var directions = new[] { new Vector2(0, -1), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(1, 0) };

        var (start, end) = (map.Single(pair => pair.Value == 'S').Key, map.Single(pair => pair.Value == 'E').Key);

        var path = new List<Vector2> { start };
        var (previous, current) = ((Vector2?)null, start);

        while (current != end)
        {
            current = directions
                .Select(direction => current + direction)
                .Single(next => map.GetValueOrDefault(next) != '#' && next != previous);

            path.Add(current);
            previous = path[^2];
        }

        return path.ToArray();
    }
}