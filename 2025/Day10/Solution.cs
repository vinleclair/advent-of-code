namespace AdventOfCode._2025.Day10;

using Machine = (HashSet<int> lights, int[][] wiring, int[] joltage);

public class Solution : ISolution
{
    public object PartOne(string input) => ParseInput(input).Sum(machine => PushButtons(machine));

    public object PartTwo(string input)
    {
        return 0;
    }

    private static int PushButtons(Machine machine, int count = 0)
    {
        if (count == machine.wiring.Length)
            return count;

        var combinations = GetCombinations(machine.wiring, count);

        foreach (var combo in combinations)
        {
            var lights = combo.SelectMany(i => i).GroupBy(x => x)
                .Where(g => g.Count() % 2 != 0)
                .Select(g => g.Key)
                .ToHashSet();

            if (lights.SetEquals(machine.lights))
                return count;
        }

        return PushButtons(machine, count + 1);
    }

    private static IEnumerable<IEnumerable<T>> GetCombinations<T>(T[] items, int length)
    {
        if (length == 0)
        {
            yield return [];
            yield break;
        }

        for (var i = 0; i <= items.Length - length; i++)
        {
            foreach (var combo in GetCombinations(items[(i + 1)..], length - 1))
            {
                yield return combo.Prepend(items[i]);
            }
        }
    }

    private static Machine[] ParseInput(string input)
    {
        var lines = input.Split("\n");
        var machines = new Machine[lines.Length];

        foreach (var (line, index) in lines.Select((l, i) => (l, i)))
        {
            var parts = line.Split(' ');

            var lights = parts[0]
                .Trim('[', ']')
                .Index()
                .Where(x => x.Item == '#')
                .Select(x => x.Index)
                .ToHashSet();

            var wiring = parts[1..^1].Select(p => p.Where(char.IsDigit).Select(c => int.Parse(c.ToString())).ToArray())
                .ToArray();

            var joltage = parts[^1].Trim('{', '}').Split(",").Select(int.Parse).ToArray();

            machines[index] = new Machine(lights, wiring, joltage);
        }

        return machines;
    }
}