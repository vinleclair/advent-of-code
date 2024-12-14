using System.Collections.Immutable;
using System.Numerics;
using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Vector2, int>;

namespace AdventOfCode._2024.Day10;

public class Solution : ISolution
{
    private static readonly Vector2 Up = new(0, 1);
    private static readonly Vector2 Down = new(0, -1);
    private static readonly Vector2 Left = new(-1, 0);
    private static readonly Vector2 Right = new(1, 0);
    private static readonly List<Vector2> Directions = [Up, Down, Left, Right];

    public object PartOne(string input)
    {
        var map = ParseInput(input);

        var trails = map
            .Where(m => m.Value == 0)
            .SelectMany(trailhead => HikeTrails(map, trailhead.Key));

        return trails.Count();
    }

    public object PartTwo(string input)
    {
        var map = ParseInput(input);

        var trails = map
            .Where(m => m.Value == 0)
            .SelectMany(trailhead => HikeTrails(map, trailhead.Key, true));

        return trails.Count();
    }

    private static IEnumerable<Vector2> HikeTrails(Map map, Vector2 trailhead, bool isRating = false)
    {
        var queue = new Queue<Vector2>();
        queue.Enqueue(trailhead);
        var visited = new List<Vector2>();

        while (queue.Count > 0)
        {
            var position = queue.Dequeue();

            if (!isRating && visited.Contains(position)) continue;

            visited.Add(position);

            foreach (var direction in Directions.Where(direction =>
                         map.GetValueOrDefault(position + direction) == map.GetValueOrDefault(position) + 1))
                queue.Enqueue(position + direction);
        }

        return visited.Where(pos => map.GetValueOrDefault(pos) == 9);
    }

    private static Map ParseInput(string input) => input.Split("\n")
        .SelectMany((line, y) =>
            line.Select((c, x) => new KeyValuePair<Vector2, int>(new Vector2(x, y), (int)char.GetNumericValue(c))))
        .ToImmutableDictionary();
}