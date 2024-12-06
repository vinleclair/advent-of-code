using System.Collections.Immutable;
using System.Numerics;
using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Vector2, char>;

namespace AdventOfCode._2024.Day06;

public class Solution : ISolution
{
    private readonly Vector2 _forward = new(0, -1);

    public object PartOne(string input)
    {
        var (map, start) = ParseInput(input);

        return Patrol(map, start).steps.Count();
    }

    public object PartTwo(string input)
    {
        var (map, start) = ParseInput(input);

        return Patrol(map, start).steps.Where(s => map[s] == '.').Count(step => Patrol(map, start, step).isLoop);
    }

    private (IEnumerable<Vector2> steps, bool isLoop) Patrol(Map map, Vector2 position, Vector2? positionToUpdate = null)
    {
        var steps = new HashSet<(Vector2 position, Vector2 direction)>();
        var direction = _forward;
        
        while (map.ContainsKey(position) && !steps.Contains((position, direction)))
        {
            steps.Add((position, direction));
            if (map.GetValueOrDefault(position + direction) == '#' || position + direction == positionToUpdate)
                direction = RotateRight(direction);
            else
                position += direction;
        }

        return (steps.Select(s => s.position).Distinct(), steps.Contains((position, direction)));
    }

    private static Vector2 RotateRight(Vector2 v) => new(-v.Y, v.X);

    private static (Map map, Vector2 start) ParseInput(string input)
    {
        var map = input.Split("\n")
            .SelectMany((line, y) => line.Select((c, x) => new KeyValuePair<Vector2, char>(new Vector2(x, y), c)))
            .ToImmutableDictionary();

        var start = map.First(m => m.Value == '^').Key;

        return (map, start);
    }
}