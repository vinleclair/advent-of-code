using System.Collections.Immutable;
using System.Numerics;
using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Vector2, char>;

namespace AdventOfCode._2024.Day08;

public class Solution : ISolution
{
    public object PartOne(string input) => GetAntiNodes(ParseInput(input)).Count;

    public object PartTwo(string input) => GetAntiNodes(ParseInput(input), true).Count;

    private static HashSet<Vector2> GetAntiNodes(Map map, bool isUsingResonantHarmonics = false)
    {
        var antiNodes = new HashSet<Vector2>();
        foreach (var (firstAntenna, frequency) in map.Where(a => char.IsLetterOrDigit(a.Value)))
        {
            foreach (var (secondAntenna, _) in map.Where(a => a.Key != firstAntenna && a.Value == frequency))
            {
                var difference = new Vector2(firstAntenna.X - secondAntenna.X, firstAntenna.Y - secondAntenna.Y);
                var antiNode = firstAntenna;
                if (isUsingResonantHarmonics)
                    antiNodes.Add(antiNode);
                do
                {
                    antiNode += difference;
                    if (map.ContainsKey(antiNode))
                        antiNodes.Add(antiNode);
                } while (map.ContainsKey(antiNode) && isUsingResonantHarmonics);
            }
        }

        return antiNodes;
    }

    private static Map ParseInput(string input) => input.Split("\n")
        .SelectMany((line, y) => line.Select((c, x) => new KeyValuePair<Vector2, char>(new Vector2(x, y), c)))
        .ToImmutableDictionary();
}