using System.Collections.Immutable;
using System.Numerics;
using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Vector2, char>;
using Cache = System.Collections.Concurrent.ConcurrentDictionary<(string, int), long>;

namespace AdventOfCode._2024.Day21;

public class Solution : ISolution
{
    private static readonly Vector2 Up = new(0, -1);
    private static readonly Vector2 Down = new(0, 1);
    private static readonly Vector2 Left = new(-1, 0);
    private static readonly Vector2 Right = new(1, 0);

    private static readonly Map Directions = new[]
    {
        new KeyValuePair<Vector2, char>(Up, '^'),
        new KeyValuePair<Vector2, char>(Down, 'v'),
        new KeyValuePair<Vector2, char>(Left, '<'),
        new KeyValuePair<Vector2, char>(Right, '>'),
    }.ToImmutableDictionary();

    private const string Numpad = "789\n456\n123\n 0A";
    private const string Dirpad = " ^A\n<v>";

    public object PartOne(string input) =>
        Solve(input, 2, GetPadMap(ParseKeypad(Numpad)), GetPadMap(ParseKeypad(Dirpad)));

    public object PartTwo(string input) =>
        Solve(input, 25, GetPadMap(ParseKeypad(Numpad)), GetPadMap(ParseKeypad(Dirpad)));

    private static long Solve(
        string input,
        int maxDepth,
        Dictionary<(char, char), List<string>> numpadMap,
        Dictionary<(char, char), List<string>> dirpadMap)
        => input.Split('\n')
            .Select(line => new
            {
                Line = line,
                Number = ParseLeadingNumber(line),
                KeySequences = BuildKeySequence(line, 0, 'A', string.Empty, [], numpadMap)
            })
            .Select(item =>
            {
                var shortestPath = item.KeySequences
                    .Select(sequence => FindShortestKeySequence(sequence, maxDepth, new Cache(), dirpadMap))
                    .Min();
                return item.Number * shortestPath;
            })
            .Sum();

    private static int ParseLeadingNumber(string input) =>
        int.Parse(new string(input.TakeWhile(char.IsDigit).ToArray()));

    private static List<string> BuildKeySequence(string keys, int index, char previousKey, string currentPath,
        List<string> result, Dictionary<(char, char), List<string>> keypad)
    {
        if (index == keys.Length)
        {
            result.Add(currentPath);
            return result;
        }

        var currentKey = keys[index];
        foreach (var path in keypad[(previousKey, currentKey)])
            BuildKeySequence(keys, index + 1, currentKey, currentPath + path + 'A', result, keypad);

        return result;
    }

    private static long FindShortestKeySequence(
        string keys,
        int depth,
        Cache cache,
        Dictionary<(char, char), List<string>> keypad)
    {
        if (depth == 0) return keys.Length;
        if (cache.TryGetValue((keys, depth), out var cachedValue)) return cachedValue;

        var subSequences = SplitOnA(keys);

        var total = subSequences.Sum(subsequence =>
        {
            var possiblePaths = BuildKeySequence(subsequence, 0, 'A', string.Empty, [], keypad);
            return possiblePaths
                .Select(path => FindShortestKeySequence(path, depth - 1, cache, keypad))
                .Min();
        });

        cache[(keys, depth)] = total;
        return total;
    }

    private static List<string> SplitOnA(string input) => input.Split('A')
        .Select((part, index) =>
            index == input.Split('A').Length - 1
                ? part
                : part + "A")
        .Where(s => !string.IsNullOrEmpty(s))
        .ToList();

    private static Dictionary<(char, char), List<string>> GetPadMap(Map pad)
    {
        var padMap = new Dictionary<(char, char), List<string>>();
        foreach (var firstKey in pad)
        foreach (var secondKey in pad)
        foreach (var path in GetShortestKeyPaths(pad, firstKey.Key, secondKey.Key))
        {
            if (!padMap.TryGetValue((firstKey.Value, secondKey.Value), out var list))
            {
                list = [];
                padMap[(firstKey.Value, secondKey.Value)] = list;
            }

            padMap[(firstKey.Value, secondKey.Value)]
                .Add(string.Join(string.Empty, path.Select(key => Directions[key]).ToList()));
        }

        return padMap;
    }

    private static List<List<Vector2>> GetShortestKeyPaths(Map map, Vector2 start, Vector2 end)
    {
        var queue = new Queue<(Vector2 position, List<Vector2> path)>();
        queue.Enqueue((start, []));
        var shortestPaths = new List<List<Vector2>>();
        var visited = new Dictionary<Vector2, int> { { start, 0 } };

        while (queue.Count > 0)
        {
            var (position, path) = queue.Dequeue();

            if (position == end)
            {
                shortestPaths.Add(path);
                continue;
            }

            foreach (var direction in Directions.Keys)
            {
                var neighbor = position + direction;

                if (!map.ContainsKey(neighbor)) continue;

                var newDepth = path.Count;

                if (visited.TryGetValue(neighbor, out var value) && value != newDepth) continue;

                visited[neighbor] = newDepth;
                queue.Enqueue((neighbor, [..path, direction]));
            }
        }

        return shortestPaths;
    }

    private static Map ParseKeypad(string keypad) =>
        keypad.Split('\n')
            .SelectMany((line, y) =>
                line.Select((c, x) => (x, y, c))
                    .Where(t => !char.IsWhiteSpace(t.c) && t.c != '\0')
                    .Select(t => new KeyValuePair<Vector2, char>(new Vector2(t.x, t.y), t.c)))
            .ToImmutableDictionary();
}