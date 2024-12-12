using System.Collections.Immutable;
using System.Numerics;
using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Vector2, char>;

namespace AdventOfCode._2024.Day12;

public class Solution : ISolution
{
    private static readonly Vector2 Up = new(0, 1);
    private static readonly Vector2 Down = new(0, -1);
    private static readonly Vector2 Left = new(-1, 0);
    private static readonly Vector2 Right = new(1, 0);
    private static readonly List<Vector2> Directions = [Up, Right, Down, Left];

    public object PartOne(string input) => CalculateRegions(ParseInput(input)).Sum(r => r.Area * r.Perimeter);

    public object PartTwo(string input) => CalculateRegions(ParseInput(input)).Sum(r => r.Area * r.Sides);

    private static List<(int Area, int Perimeter, int Sides)> CalculateRegions(Map map)
    {
        var visited = new HashSet<Vector2>();
        var regions = new List<(int Area, int Perimeter, int Sides)>();

        foreach (var plot in map)
        {
            var queue = new Queue<Vector2>();
            var perimeter = 0;
            var area = 0;
            var sides = 0;

            queue.Enqueue(plot.Key);

            while (queue.Count > 0)
            {
                var position = queue.Dequeue();

                if (!visited.Add(position)) continue;

                area++;
                sides += CalculateCorners(map, position);

                var currentPlot = map.GetValueOrDefault(position);

                foreach (var direction in Directions)
                    if (map.GetValueOrDefault(position + direction) == currentPlot)
                        queue.Enqueue(position + direction);
                    else
                        perimeter += 1;
            }

            regions.Add((area, perimeter, sides));
        }

        return regions;
    }

    private static int CalculateCorners(Map map, Vector2 position)
    {
        var corners = 0;

        var currentValue = map.GetValueOrDefault(position);

        for (var i = 0; i < Directions.Count; i++)
        {
            var currentDirection = Directions[i];
            var nextDirection = Directions[(i + 1) % Directions.Count];
            var currentNeighbor = map.GetValueOrDefault(position + currentDirection);
            var nextNeighbor = map.GetValueOrDefault(position + nextDirection);
            var diagonalNeighbor = map.GetValueOrDefault(position + currentDirection + nextDirection);

            if (currentValue != currentNeighbor && currentValue != nextNeighbor)
                corners++;

            if (currentValue == currentNeighbor &&
                currentValue == nextNeighbor &&
                map.ContainsKey(position + currentDirection + nextDirection) &&
                currentValue != diagonalNeighbor)
                corners++;
        }

        return corners;
    }

    private static Map ParseInput(string input) => input.Split("\n")
        .SelectMany((line, y) =>
            line.Select((c, x) => new KeyValuePair<Vector2, char>(new Vector2(x, y), c)))
        .ToImmutableDictionary();
}