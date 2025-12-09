using System.Numerics;

namespace AdventOfCode._2025.Day09;

public class Solution : ISolution
{
    public object PartOne(string input)
    {
        var tiles = ParseInput(input);

        var largestArea = 0L;

        for (var i = 0; i < tiles.Length; i++)
        {
            var firstTile = tiles[i];
            for (var j = i + 1; j < tiles.Length; j++)
            {
                var secondTile = tiles[j];
                var area = (long)((Math.Abs(secondTile.X - firstTile.X) + 1) *
                                  (Math.Abs(secondTile.Y - firstTile.Y) + 1));
                if (area > largestArea)
                    largestArea = area;
            }
        }

        return largestArea;
    }

    private static Vector2[] ParseInput(string input) => input.Split("\n").Select(line => line.Split(","))
        .Select(coords => new Vector2(int.Parse(coords[0]), int.Parse(coords[1]))).ToArray();
}