using System.Numerics;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Vector2, char>;
using State = (System.Numerics.Vector2 Position, System.Numerics.Vector2 Direction);

namespace AdventOfCode._2024.Day16;

public class Solution : ISolution
{
    private static readonly Vector2 North = new(0, -1);
    private static readonly Vector2 South = new(0, 1);
    private static readonly Vector2 West = new(-1, 0);
    private static readonly Vector2 East = new(1, 0);

    public object PartOne(string input)
    {
        var map = ParseInput(input);
        var start = new State(GetTile(map, 'S'), East);
        var end = GetTile(map, 'E');

        return Dijkstra(map, end)[start];
    }

    public object PartTwo(string input)
    {
        var map = ParseInput(input);
        var start = new State(GetTile(map, 'S'), East);
        var end = GetTile(map, 'E');

        var distances = Dijkstra(map, end);

        var priorityQueue = new PriorityQueue<State, int>();
        priorityQueue.Enqueue(start, distances[start]);

        var bestSpots = new HashSet<State> { start };

        while (priorityQueue.TryDequeue(out var current, out var remainingScore))
            foreach (var (next, score) in Steps(map, current, true))
            {
                var nextRemainingScore = remainingScore - score;

                if (bestSpots.Contains(next) || distances[next] != nextRemainingScore) continue;

                bestSpots.Add(next);
                priorityQueue.Enqueue(next, nextRemainingScore);
            }

        return bestSpots.DistinctBy(state => state.Position).Count();
    }

    private static Dictionary<State, int> Dijkstra(Map map, Vector2 endTile)
    {
        var distance = new Dictionary<State, int>();

        var priorityQueue = new PriorityQueue<State, int>();
        foreach (var direction in new[] { North, East, West, South })
        {
            priorityQueue.Enqueue((endTile, direction), 0);
            distance[(endTile, direction)] = 0;
        }

        while (priorityQueue.TryDequeue(out var current, out var totalDistance))
            foreach (var (next, score) in Steps(map, current, false))
            {
                var nextCost = totalDistance + score;

                if (nextCost >= distance.GetValueOrDefault(next, int.MaxValue)) continue;

                priorityQueue.Remove(next, out _, out _);
                distance[next] = nextCost;
                priorityQueue.Enqueue(next, nextCost);
            }

        return distance;
    }

    private static IEnumerable<(State, int cost)> Steps(Map map, State state, bool forward)
    {
        foreach (var direction in new[] { North, East, West, South })
            if (direction == state.Direction)
            {
                var position = forward ? state.Position + direction : state.Position - direction;
                if (map.GetValueOrDefault(position) != '#')
                    yield return ((position, direction), 1);
            }
            else if (direction != -state.Direction)
                yield return ((state.Position, direction), 1000);
    }

    private static Map ParseInput(string input) => input.Split("\n")
        .SelectMany((line, y) =>
            line.Select((c, x) =>
                new KeyValuePair<Vector2, char>(new Vector2(x, y), c)
            )).ToDictionary();

    private static Vector2 GetTile(Map map, char tile) => map.Keys.Single(k => map[k] == tile);
}