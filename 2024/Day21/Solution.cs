using System.Collections.Immutable;
using System.Numerics;
using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Vector2, char>;
using Cache = System.Collections.Concurrent.ConcurrentDictionary<(string, int), int>;

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

    private static readonly Map Numpad = new[]
    {
        new KeyValuePair<Vector2, char>(new Vector2(0, 0), '7'),
        new KeyValuePair<Vector2, char>(new Vector2(1, 0), '8'),
        new KeyValuePair<Vector2, char>(new Vector2(2, 0), '9'),
        new KeyValuePair<Vector2, char>(new Vector2(0, 1), '4'),
        new KeyValuePair<Vector2, char>(new Vector2(1, 1), '5'),
        new KeyValuePair<Vector2, char>(new Vector2(2, 1), '6'),
        new KeyValuePair<Vector2, char>(new Vector2(0, 2), '1'),
        new KeyValuePair<Vector2, char>(new Vector2(1, 2), '2'),
        new KeyValuePair<Vector2, char>(new Vector2(2, 2), '3'),
        new KeyValuePair<Vector2, char>(new Vector2(1, 3), '0'),
        new KeyValuePair<Vector2, char>(new Vector2(2, 3), 'A')
    }.ToImmutableDictionary();

    private static readonly Map Dirpad = new[]
    {
        new KeyValuePair<Vector2, char>(new Vector2(1, 0), '^'),
        new KeyValuePair<Vector2, char>(new Vector2(2, 0), 'A'),
        new KeyValuePair<Vector2, char>(new Vector2(0, 1), '<'),
        new KeyValuePair<Vector2, char>(new Vector2(1, 1), 'v'),
        new KeyValuePair<Vector2, char>(new Vector2(2, 1), '>'),
    }.ToImmutableDictionary();

    public object PartOne(string input)
    {
        var numpadMap = GetPadMap(Numpad);
        var dirpadMap = GetPadMap(Dirpad);
        input = "029A\n980A\n179A\n456A\n379A";
        
        var total = 0;
        foreach (var keys in input.Split("\n"))
        {
            var numpadKeySequence = BuildKeySequence(keys, 0, 'A', string.Empty, [], numpadMap);
            var min = int.MaxValue;
            
            foreach (var sequence in numpadKeySequence)
            {
                min = Math.Min(min, FindShortestKeySequence(sequence, 2, new Cache(), dirpadMap));
            }

            var numPart = int.Parse(new string(keys.TakeWhile(char.IsDigit).ToArray()));
            Console.WriteLine(min);
            Console.WriteLine(numPart + " x " + min);
            total += numPart * min;
        }

        return total;
    }

    private List<string> BuildKeySequence(string keys, int index, char previousKey, string currentPath,
        List<string> result, Dictionary<(char, char), List<string>> keypad)
    {
        if (index == keys.Length)
        {
            result.Add(currentPath);
            return result;
        }

        var currentKey = keys[index];
        foreach (var path in keypad[(previousKey, currentKey)])
        {
            BuildKeySequence(keys, index + 1, currentKey, currentPath + path + 'A', result, keypad);
        }

        return result;
    }

    private int FindShortestKeySequence(string keys, int depth, Cache cache, Dictionary<(char, char), List<string>> keypad)
    {
        if (depth == 0)
            return keys.Length;
        if (cache.TryGetValue((keys, depth), out var cachedValue))
            return cachedValue;
        var subKeys = keys.Split('A');
        var total = 0;
        foreach (var subKey in subKeys)
        {
            var min = int.MaxValue;
            foreach (var keySequence in BuildKeySequence(subKey, 0, 'A', string.Empty, [], keypad))
            {
                min = Math.Min(min, FindShortestKeySequence(keySequence, depth - 1, cache, keypad));
            }
            total += min;
        }

        cache[(keys, depth)] = total;
        return total;
    }

    private static Dictionary<(char, char), List<string>> GetPadMap(Map pad)
    {
        var padMap = new Dictionary<(char, char), List<string>>();
        foreach (var firstKey in pad)
        {
            foreach (var secondKey in pad)
            {
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
            }
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

    private static Map ParseKeypad(string keypad) => keypad.Split("\n")
        .SelectMany((line, y) => line.Select((ch, x) => new KeyValuePair<Vector2, char>(new Vector2(x, y), ch)))
        .ToImmutableDictionary();
}