namespace AdventOfCode._2025.Day01;

public class Solution : ISolution
{
    private const int TrackSize = 100;
    private const int StartPosition = 50;

    public object PartOne(string input)
    {
        var sum = 0;
        var position = StartPosition;
        var rotations = ParseInput(input);

        foreach (var rotation in rotations)
        {
            var direction = rotation.direction == 'R' ? 1 : -1;

            position = (position + direction * rotation.distance) % TrackSize;

            if (position == 0)
                sum++;
        }

        return sum;
    }

    public object PartTwo(string input)
    {
        var sum = 0;
        var position = StartPosition;
        var rotations = ParseInput(input);

        foreach (var rotation in rotations)
        {
            var direction = rotation.direction == 'R' ? 1 : -1;

            for (var i = 0; i < rotation.distance; i++)
            {
                position += direction;
                position %= TrackSize;
                if (position == 0) sum++;
            }
        }

        return sum;
    }

    private static IEnumerable<(char direction, int distance)> ParseInput(string input) => input.Split("\n")
        .Select(line => (line[0], int.Parse(line[1..])));
}