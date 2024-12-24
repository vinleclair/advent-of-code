using System.Text.RegularExpressions;

namespace AdventOfCode._2024.Day24;

using Circuit = Dictionary<string, Gate>;

internal record struct Gate(string FirstWire, string LogicGate, string SecondWire);

public partial class Solution : ISolution
{
    public object PartOne(string input)
    {
        var (wires, circuit) = ParseInput(input);

        return Convert.ToInt64(
            circuit.Keys.Where(output => output.StartsWith('z')).OrderByDescending(output => output)
                .Aggregate("", (current, output) => current + Simulate(output, circuit, wires)), 2);
    }

    private static int Simulate(string wire, Circuit circuit, Dictionary<string, int> wires)
    {
        if (wires.TryGetValue(wire, out var res))
            return res;

        return circuit[wire] switch
        {
            (var firstWire, "AND", var secondWire) => Simulate(firstWire, circuit, wires) &
                                                      Simulate(secondWire, circuit, wires),
            (var firstWire, "OR", var secondWire) => Simulate(firstWire, circuit, wires) |
                                                     Simulate(secondWire, circuit, wires),
            (var firstWire, "XOR", var secondWire) => Simulate(firstWire, circuit, wires) ^
                                                      Simulate(secondWire, circuit, wires),
            _ => throw new Exception(circuit[wire].ToString())
        };
    }

    private static (Dictionary<string, int> wires, Circuit circuit) ParseInput(string input)
    {
        var blocks = input.Split("\n\n");

        var wires = blocks[0].Split("\n").Select(line => line.Split(": "))
            .Select(parts => new KeyValuePair<string, int>(parts[0], int.Parse(parts[1]))).ToDictionary();

        var circuit = blocks[1].Split("\n").Select(l => Wire().Matches(l).Select(m => m.Value).ToArray()).Select(
            parts =>
                new KeyValuePair<string, Gate>(parts[3],
                    new Gate(FirstWire: parts[0], LogicGate: parts[1], SecondWire: parts[2]))).ToDictionary();

        return (wires, circuit);
    }

    [GeneratedRegex("[a-zA-z0-9]+")]
    private static partial Regex Wire();
}