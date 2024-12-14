using Equation = System.Collections.Generic.KeyValuePair<long, System.Collections.Generic.IEnumerable<long>>;

namespace AdventOfCode._2024.Day07;

public class Solution : ISolution
{
    private readonly Func<long, long, long> _add = (x, y) => x + y;
    private readonly Func<long, long, long> _mult = (x, y) => x * y;
    private readonly Func<long, long, long> _conc = (x, y) => long.Parse(string.Concat(x, y));

    public object PartOne(string input) => GetTrueCalibrationResults(ParseInput(input), [_add, _mult]).Sum();

    public object PartTwo(string input) => GetTrueCalibrationResults(ParseInput(input), [_add, _mult, _conc]).Sum();

    private static IEnumerable<long> GetTrueCalibrationResults(IEnumerable<Equation> equations,
        List<Func<long, long, long>> operators) =>
        from equation in equations
        where IsEquationTrue(equation.Key, operators, equation.Value.First(), equation.Value.Skip(1).ToArray())
        select equation.Key;

    private static bool IsEquationTrue(long target, List<Func<long, long, long>> operators, long acc,
        long[] testNumbers) => testNumbers.Length == 0 || acc > target
        ? target == acc
        : operators.Any(op => IsEquationTrue(target, operators, op(acc, testNumbers[0]), testNumbers[1..]));

    private static IEnumerable<Equation> ParseInput(string input) => input.Split('\n').Select(line =>
        new Equation(long.Parse(line.Split(':', StringSplitOptions.RemoveEmptyEntries)[0]),
            line.Split(':', StringSplitOptions.RemoveEmptyEntries)[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)));
}