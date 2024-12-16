using System.Numerics;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Vector2, char>;

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
        //TODO Get this working properly
        return 0;
    }

    private static bool CanTakeStep(ref Map map, Vector2 robot, Vector2 step)
    {
        if (map[robot + step] == '.')
            return true;

        var boxes = GetBoxes(map, [], robot, step);

        if (boxes.Count == 0)
            return false;

        map[boxes[0]] = '.';
        boxes.RemoveAt(0);
        foreach (var box in boxes)
            map[box] = 'O';

        return true;
    }

    private static List<Vector2> GetBoxes(Map map, List<Vector2> boxes, Vector2 robot, Vector2 step)
    {
        while (true)
        {
            if (map[robot + step] == '#')
            {
                return [];
            }

            boxes.Add(robot + step);
            if (map[robot + step] == '.') return boxes;
            robot += step;
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
        var map = input.Split("\n")
            .SelectMany((line, y) =>
                line.Select((c, x) =>
                {
                    if (c == '@')
                    {
                        robot = new Vector2(x, y);
                        c = '.';
                    }

                    return new KeyValuePair<Vector2, char>(new Vector2(x, y), c);
                }))
            .ToDictionary();

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