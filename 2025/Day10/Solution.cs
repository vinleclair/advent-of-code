namespace AdventOfCode._2025.Day10;

using Machine = (int[] lights, int[][] wiring, int[] joltage);

public class Solution : ISolution
{
    private const int NoSolution = 1_000_000_000;

    public object PartOne(string input) => ParseInput(input).Sum(ConfigureLights);

    public object PartTwo(string input) => ParseInput(input).Sum(ConfigureJoltage);

    private static int ConfigureLights(Machine machine) =>
        GetPatterns(machine)[machine.lights].MinBy(p => p.Value).Value;

    private static int ConfigureJoltage(Machine machine)
    {
        var patternCosts = GetPatterns(machine);
        var cache = new Dictionary<int[], int>(new ArrayEqualityComparer());

        return ConfigureJoltageAux(machine.joltage);

        int ConfigureJoltageAux(int[] currentGoal)
        {
            if (currentGoal.All(i => i == 0))
                return 0;

            if (cache.TryGetValue(currentGoal, out var cached))
                return cached;

            var answer = NoSolution;
            var parityPattern = currentGoal.Select(i => i % 2).ToArray();

            if (patternCosts.TryGetValue(parityPattern, out var patternsDict))
            {
                foreach (var (pattern, patternCost) in patternsDict)
                {
                    if (!pattern.Zip(currentGoal, (i, j) => i <= j).All(x => x)) continue;

                    var newGoal = pattern.Zip(currentGoal, (i, j) => (j - i) / 2).ToArray();
                    answer = Math.Min(answer, patternCost + 2 * ConfigureJoltageAux(newGoal));
                }
            }

            cache[currentGoal] = answer;

            return answer;
        }
    }

    private static Dictionary<int[], Dictionary<int[], int>> GetPatterns(Machine machine)
    {
        var numButtons = machine.wiring.Length;
        var numVariables = machine.lights.Length;

        var patterns = new Dictionary<int[], Dictionary<int[], int>>(new ArrayEqualityComparer());

        foreach (var parityPattern in GetAllParityPatterns(numVariables))
        {
            patterns[parityPattern] = new Dictionary<int[], int>();
        }

        for (var numPressedButtons = 0; numPressedButtons <= numButtons; numPressedButtons++)
        {
            foreach (var buttons in GetCombinations(Enumerable.Range(0, numButtons).ToArray(), numPressedButtons))
            {
                var pattern = machine.wiring.Where((_, index) => buttons.Contains(index))
                    .Aggregate(new int[numVariables], (acc, arr) => acc.Zip(arr, (a, b) => a + b).ToArray());

                var parityPattern = pattern.Select(p => p % 2).ToArray();

                if (!patterns[parityPattern].ContainsKey(pattern))
                    patterns[parityPattern][pattern] = numPressedButtons;
            }
        }

        return patterns;
    }

    private static IEnumerable<int[]> GetAllParityPatterns(int numVariables)
    {
        for (var len = 1; len <= numVariables; len++)
        {
            for (var i = 0; i < 1 << len; i++)
            {
                var pattern = new int[len];
                for (var j = 0; j < len; j++)
                {
                    pattern[j] = (i >> j) & 1;
                }

                yield return pattern;
            }
        }
    }


    private static IEnumerable<IEnumerable<T>> GetCombinations<T>(T[] items, int length = 0)
    {
        while (true)
        {
            if (length == 0 && items.Length > 0 || items.Length == 0)
            {
                yield return [];
                yield break;
            }

            if (length > 0 && length == items.Length)
            {
                yield return items;
                yield break;
            }

            var nextLength = length > 0 ? length - 1 : 0;

            foreach (var combo in GetCombinations(items[1..], nextLength))
            {
                yield return combo.Prepend(items[0]);
            }

            items = items[1..];
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
                .Select(x => x == '#' ? 1 : 0)
                .ToArray();

            var wiring =
                parts[1..^1]
                    .Select(p => p.Where(char.IsDigit).Select(c => int.Parse(c.ToString())).ToArray())
                    .Select(r => Enumerable.Range(0, lights.Length)
                        .Select(i => r.Contains(i) ? 1 : 0)
                        .ToArray())
                    .ToArray();

            var joltage = parts[^1].Trim('{', '}').Split(",").Select(int.Parse).ToArray();


            machines[index] = new Machine(lights, wiring, joltage);
        }

        return machines;
    }
}

internal class ArrayEqualityComparer : IEqualityComparer<int[]>
{
    public bool Equals(int[]? x, int[]? y)
    {
        if (x == null || y == null) return x == y;
        return x.SequenceEqual(y);
    }

    public int GetHashCode(int[] obj)
    {
        unchecked
        {
            return obj.Aggregate(17, (current, item) => current * 31 + item);
        }
    }
}