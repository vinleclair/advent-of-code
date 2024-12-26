using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode._2024.Day18;

public partial class Solution : ISolution
{
    private const int Size = 70;
    private static readonly Vector2 Up = new(0, -1);
    private static readonly Vector2 Down = new(0, 1);
    private static readonly Vector2 Left = new(-1, 0);
    private static readonly Vector2 Right = new(1, 0);

    public object PartOne(string input) => Dijkstra(ParseInput(input).Take(1024)) ?? int.MinValue;

    public object PartTwo(string input) => FirstCutOff(ParseInput(input).ToArray());

    private static int? Dijkstra(IEnumerable<Vector2> bytes)
    {
        var (start, end) = (new Vector2(0, 0), new Vector2(Size, Size));
        var corrupted = bytes.Append(start).ToHashSet();

        var priorityQueue = new PriorityQueue<Vector2, int>();
        priorityQueue.Enqueue(start, 0);

        while (priorityQueue.TryDequeue(out var position, out var distance))
        {
            if (position == end)
                return distance;

            foreach (var direction in new[] { Up, Down, Left, Right })
            {
                var newPosition = position + direction;
                if (corrupted.Contains(newPosition) ||
                    newPosition is not { X: >= 0 and <= Size, Y: >= 0 and <= Size }) continue;
                priorityQueue.Enqueue(newPosition, distance + 1);
                corrupted.Add(newPosition);
            }
        }

        return null;
    }

    private static Vector2 FirstCutOff(Vector2[] bytes)
    {
        var (low, high) = (0, bytes.Length);

        while (low < high - 1)
        {
            var mid = (low + high) / 2;
            if (Dijkstra(bytes.Take(mid)) == null)
            {
                high = mid;
            }
            else
            {
                low = mid;
            }
        }

        return bytes[low];
    }

    private static IEnumerable<Vector2> ParseInput(string input) => input.Split("\n")
        .Select(line =>
            new { line, nums = OneOrMoreDigits().Matches(line).Select(m => int.Parse(m.Value)).ToArray() })
        .Select(line => new Vector2(line.nums[0], line.nums[1]));

    [GeneratedRegex(@"\d+", RegexOptions.Multiline)]
    private static partial Regex OneOrMoreDigits();
}