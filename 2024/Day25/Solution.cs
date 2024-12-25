namespace AdventOfCode._2024.Day25;

public class Solution : ISolution
{
    public object PartOne(string input)
    {
        var (locks, keys) = ParseInput(input);

        return locks
            .Join(keys,
                _ => true,
                _ => true,
                (@lock, key) => new { @lock, key })
            .Count(t => !HasOverlap(t.@lock, t.key));
    }

    public object PartTwo(string input) => 0;

    private static bool HasOverlap(int[] @lock, int[] key) => @lock.Where((t, i) => t + key[i] > 5).Any();

    private static (List<int[]> locks, List<int[]> keys) ParseInput(string input)
    {
        var keys = new List<int[]>();
        var locks = new List<int[]>();

        var lines = input.Split("\n\n").Select(g => g.Split("\n")).ToArray();

        foreach (var grid in lines)
        {
            if (grid[0] == "#####")
                locks.Add(CountHashes(grid));
            else if (grid[6] == "#####")
                keys.Add(CountHashes(grid));
        }

        return (locks, keys);
    }

    private static int[] CountHashes(string[] grid)
    {
        var cols = grid[0].Length;
        var counts = new int[cols];
        Array.Fill(counts, -1);

        for (var row = grid.Length - 1; row >= 0; row--)
        for (var col = 0; col < cols; col++)
            if (grid[row][col] == '#')
                counts[col]++;

        return counts;
    }
}