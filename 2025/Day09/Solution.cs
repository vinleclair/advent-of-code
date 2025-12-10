using System.Numerics;

namespace AdventOfCode._2025.Day09;

public class Solution : ISolution
{
    public object PartOne(string input)
    {
        var (tiles, _) = ParseInput(input);

        var largestArea = 0L;

        for (var i = 0; i < tiles.Length; i++)
        {
            for (var j = i + 1; j < tiles.Length; j++)
            {
                var area = Area(tiles[i], tiles[j]);
                if (area > largestArea)
                    largestArea = area;
            }
        }

        return largestArea;
    }

    public object PartTwo(string input)
    {
        var (tiles, lines) = ParseInput(input);

        var largestArea = 0L;

        for (var i = 0; i < tiles.Length; i++)
        {
            var localArea = 0L;
            for (var j = i + 1; j < tiles.Length; j++)
            {
                var area = Area(tiles[i], tiles[j]);

                if (area <= localArea) continue;

                var isIntersecting = false;
                for (var lineIndex = 0; lineIndex < lines.Length && !isIntersecting; lineIndex++)
                    isIntersecting = Intersects(lines[lineIndex].A, lines[lineIndex].B, tiles[i], tiles[j]);

                if (isIntersecting) continue;

                localArea = area;
            }

            largestArea = Math.Max(localArea, largestArea);
        }

        return largestArea;
    }

    private static long Area(Vector2 firstTile, Vector2 secondTile) =>
        ((long)Math.Abs(secondTile.X - firstTile.X) + 1) *
        ((long)Math.Abs(secondTile.Y - firstTile.Y) + 1);

    private static bool Intersects(Vector2 firstLineA, Vector2 firstLineB, Vector2 secondLineA, Vector2 secondLineB)
    {
        var (firstLineMinX, firstLineMaxX) = MinMax(firstLineA.X, firstLineB.X);
        var (firstLineMinY, firstLineMaxY) = MinMax(firstLineA.Y, firstLineB.Y);
        var (secondLineMinX, secondLineMaxX) = MinMax(secondLineA.X, secondLineB.X);
        var (secondLineMinY, secondLineMaxY) = MinMax(secondLineA.Y, secondLineB.Y);

        return secondLineMaxX > firstLineMinX &&
               secondLineMinX < firstLineMaxX &&
               secondLineMaxY > firstLineMinY &&
               secondLineMinY < firstLineMaxY;
    }

    private static (float min, float max) MinMax(float a, float b) => a < b ? (a, b) : (b, a);

    private static (Vector2[] tiles, (Vector2 A, Vector2 B)[] lines) ParseInput(string input)
    {
        var tiles = input.Split("\n").Select(line => line.Split(",").Select(int.Parse).ToArray())
            .Select(coords => new Vector2(coords[0], coords[1])).ToArray();

        var lines = new (Vector2, Vector2)[tiles.Length];
        for (var i = 0; i < tiles.Length; i++)
            lines[i] = new ValueTuple<Vector2, Vector2>(tiles[i], tiles[(i + 1) % tiles.Length]);

        return (tiles, lines);
    }
}