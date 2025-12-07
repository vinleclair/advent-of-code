namespace AdventOfCode._2025.Day07;

public class Solution : ISolution
{
    public object PartOne(string input)
    {
        var (beam, splitters) = ParseInput(input);

        var beams = new HashSet<int> { beam };

        var splitCount = 0;

        foreach (var line in splitters)
        foreach (var splitter in line)
        {
            if (!beams.Contains(splitter)) continue;

            beams.Remove(splitter);
            beams.Add(splitter + 1);
            beams.Add(splitter - 1);
            splitCount++;
        }

        return splitCount;
    }

    public object PartTwo(string input)
    {
        var (beam, splitters) = ParseInput(input);

        var timelines = Enumerable.Repeat(1L, splitters.Length).ToArray();

        foreach (var line in splitters.Reverse())
        foreach (var splitter in line)
            timelines[splitter] = timelines[splitter - 1] + timelines[splitter + 1];

        return timelines[beam];
    }

    private static (int beam, int[][] splitters) ParseInput(string input)
    {
        var beam = input.IndexOf('S');
        var lines = input.Split("\n");
        var splitters = new int[lines.Length][];

        foreach (var (line, index) in lines.Select((line, index) => (line, index)))
        {
            var indices = new List<int>();

            for (var i = line.IndexOf('^'); i > -1; i = line.IndexOf('^', i + 1))
                indices.Add(i);

            splitters[index] = indices.ToArray();
        }

        return (beam, splitters);
    }
}