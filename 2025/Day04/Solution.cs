namespace AdventOfCode._2025.Day04;

public class Solution : ISolution
{
    private static readonly (int y, int x)[] Directions =
    [
        (-1, 0), (-1, 1), (0, 1), (1, 1),
        (1, 0), (1, -1), (0, -1), (-1, -1)
    ];

    private const char Roll = '@';

    public object PartOne(string input) => GetRolls(ParseInput(input));

    public object? PartTwo(string input) => GetRolls(ParseInput(input), true);

    private static int GetRolls(char[,] grid, bool removeRolls = false)
    {
        var rolls = 0;
        int removedRolls;

        do
        {
            removedRolls = 0;
            for (var y = 0; y < grid.GetLength(0); y++)
            {
                for (var x = 0; x < grid.GetLength(1); x++)
                {
                    if (!IsAccessibleRoll(grid, y, x)) continue;

                    rolls++;

                    if (!removeRolls) continue;

                    grid[y, x] = '.';
                    removedRolls++;
                }
            }
        } while (removedRolls != 0);

        return rolls;
    }

    private static bool IsAccessibleRoll(char[,] grid, int y, int x)
    {
        if (grid[y, x] != Roll)
            return false;

        var adjacentRolls = 0;

        foreach (var direction in Directions)
        {
            if (IsRoll(grid, y + direction.y, x + direction.x))
                adjacentRolls++;

            if (adjacentRolls >= 4)
                return false;
        }

        return true;
    }

    private static bool IsRoll(char[,] grid, int y, int x) =>
        y >= 0 && y < grid.GetLength(0) && x >= 0 && x < grid.GetLength(1) && grid[y, x] == Roll;

    private static char[,] ParseInput(string input)
    {
        var size = input.IndexOf('\n');
        var grid = new char[size, size];
        int y = 0, x = 0;

        foreach (var c in input)
        {
            if (c == '\n')
            {
                y++;
                x = 0;
            }
            else
            {
                grid[y, x++] = c;
            }
        }

        return grid;
    }
}