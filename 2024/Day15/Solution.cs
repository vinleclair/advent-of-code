using System.Collections.Immutable;
using System.Numerics;
using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Vector2, char>;

namespace AdventOfCode._2024.Day15;

public class Solution : ISolution
{
    private static readonly Vector2 Up = new(0, -1);
    private static readonly Vector2 Down = new(0, 1);
    private static readonly Vector2 Left = new(-1, 0);
    private static readonly Vector2 Right = new(1, 0);

    public object PartOne(string input)
    {
        var (map, robot, steps) = ParseInput(input);

        foreach (var step in steps)
            if (CanTakeStep(ref map, robot, step))
                robot += step;

        return GetGpsCoordinatesSum(map);
    }

    public object PartTwo(string input)
    {
        var (map, robot, steps) = ParseInput(ScaleInput(input));

        foreach (var step in steps)
            if (CanTakeStep(ref map, robot, step))
                robot += step;

        return GetGpsCoordinatesSum(map);
    }

    private static bool CanTakeStep(ref Map map, Vector2 robot, Vector2 step)
    {
        while (true)
        {
            var originalMap = map;

            switch (map[robot])
            {
                case '.':
                    return true;
                case 'O':
                case '@':
                {
                    if (CanTakeStep(ref map, robot + step, step))
                    {
                        map = map.SetItem(robot + step, map[robot])
                            .SetItem(robot, '.');
                        return true;
                    }

                    break;
                }
                case ']':
                    robot += Left;
                    continue;
                case '[' when step == Left:
                {
                    if (CanTakeStep(ref map, robot + Left, step))
                    {
                        map = map.SetItem(robot + Left, '[')
                            .SetItem(robot, ']')
                            .SetItem(robot + Right, '.');
                        return true;
                    }

                    break;
                }
                case '[' when step == Right:
                {
                    if (CanTakeStep(ref map, robot + 2 * Right, step))
                    {
                        map = map.SetItem(robot, '.')
                            .SetItem(robot + Right, '[')
                            .SetItem(robot + 2 * Right, ']');
                        return true;
                    }

                    break;
                }
                case '[' when CanTakeStep(ref map, robot + step, step) &&
                              CanTakeStep(ref map, robot + Right + step, step):
                    map = map.SetItem(robot, '.')
                        .SetItem(robot + Right, '.')
                        .SetItem(robot + step, '[')
                        .SetItem(robot + step + Right, ']');
                    return true;
            }

            map = originalMap;
            return false;
        }
    }

    private static int GetGpsCoordinatesSum(Map map) =>
        (int)map.Where(l => l.Value is 'O' or '[').Sum(b => b.Key.Y * 100 + b.Key.X);

    private static string ScaleInput(string input) =>
        input.Replace("#", "##").Replace(".", "..").Replace("O", "[]").Replace("@", "@.");

    private static (Map, Vector2, Vector2[]) ParseInput(string input)
    {
        var blocks = input.Split("\n\n");
        var robot = new Vector2();
        var map = blocks[0].Split("\n")
            .SelectMany((line, y) =>
                line.Select((c, x) =>
                {
                    if (c == '@')
                        robot = new Vector2(x, y);

                    return new KeyValuePair<Vector2, char>(new Vector2(x, y), c);
                }))
            .ToImmutableDictionary();

        var steps = blocks[1].ReplaceLineEndings(string.Empty).Select(c =>
            c switch
            {
                '^' => Up,
                '<' => Left,
                '>' => Right,
                'v' => Down,
                _ => throw new InvalidCastException()
            });

        return (map, robot, steps.ToArray());
    }
}