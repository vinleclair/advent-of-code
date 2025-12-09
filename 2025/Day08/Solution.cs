using System.Numerics;

namespace AdventOfCode._2025.Day08;

public class Solution : ISolution
{
    public object PartOne(string input)
    {
        var (junctionBoxes, distances) = ParseInput(input);

        return ConnectJunctionBoxes(junctionBoxes, distances, out _, 1000)
            .Select(c => c.Count)
            .OrderByDescending(c => c)
            .Take(3)
            .Aggregate((a, b) => a * b);
    }

    public object PartTwo(string input)
    {
        var (junctionBoxes, distances) = ParseInput(input);

        ConnectJunctionBoxes(junctionBoxes, distances, out var lastConnection);

        return (long)lastConnection.firstBox.X * (long)lastConnection.secondBox.X;
    }

    private static List<HashSet<Vector3>> ConnectJunctionBoxes(Vector3[] junctionBoxes,
        Dictionary<float, (Vector3 firstBox, Vector3 secondBox)> distances,
        out (Vector3 firstBox, Vector3 secondBox) lastConnection, int connections = 0)
    {
        lastConnection = (new Vector3(), new Vector3());

        var circuits = junctionBoxes.Select(box => (HashSet<Vector3>)[box]).ToList();

        var pairs = distances.OrderBy(x => x.Key).Select(p => p.Value);

        if (connections > 0)
            pairs = pairs.Take(connections);

        foreach (var (firstBox, secondBox) in pairs)
        {
            var firstCircuit = circuits.First(c => c.Contains(firstBox));
            var secondCircuit = circuits.First(c => c.Contains(secondBox));

            if (firstCircuit == secondCircuit) continue;

            firstCircuit.UnionWith(secondCircuit);
            circuits.Remove(secondCircuit);

            lastConnection = (firstBox, secondBox);

            if (circuits.Count == 1)
                break;
        }

        return circuits;
    }

    private static (Vector3[], Dictionary<float, (Vector3 firstBox, Vector3 secondBox)>) ParseInput(string input)
    {
        var junctionBoxes = input
            .Split("\n")
            .Select(line => line.Split(","))
            .Select(coords => new Vector3(int.Parse(coords[0]), int.Parse(coords[1]), int.Parse(coords[2])))
            .ToArray();

        var distances = new Dictionary<float, (Vector3 firstBox, Vector3 secondBox)>();

        for (var i = 0; i < junctionBoxes.Length - 1; i++)
        {
            var firstBox = junctionBoxes[i];
            for (var j = i + 1; j < junctionBoxes.Length; j++)
            {
                var secondBox = junctionBoxes[j];
                distances.TryAdd(Vector3.Distance(firstBox, secondBox), (firstBox, secondBox));
            }
        }

        return (junctionBoxes, distances);
    }
}