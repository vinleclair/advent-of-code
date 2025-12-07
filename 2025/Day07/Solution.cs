namespace AdventOfCode._2025.Day07;

public class Solution : ISolution
{
    public object PartOne(string input)
    {
        var (beamIndex, splitters) = ParseInput(input);

        var beamIndices = new List<List<int>>();

        beamIndices.Add([beamIndex]);

        var splitCount = 0;

        foreach (var (splitter, index) in splitters.Select((splitter, index) => (splitter, index)))
        {
            Console.WriteLine(string.Join(',', beamIndices[index]));
            beamIndices.Add([]);
            foreach (var beam in beamIndices[index])
            {
                if (splitter.Contains(beam))
                {
                    splitCount++;
                    beamIndices[index + 1].Add(beam - 1);
                    beamIndices[index + 1].Add(beam + 1);
                }

                if (!splitter.Contains(beam))
                {
                    beamIndices[index + 1].Add(beam);
                }
            }

            beamIndices[index+1] = beamIndices[index+1].Distinct().ToList();
        }

        return splitCount;
    }

    private static (int beamIndex, List<List<int>> splitters) ParseInput(string input)
    {
        var beamIndex = -1;
        var splitters = new List<List<int>>();

        foreach (var line in input.Split("\n"))
        {
            if (line.Contains('S')) beamIndex = line.IndexOf('S');

            if (!line.Contains('^')) continue;

            var indices = new List<int>();

            for (var i = line.IndexOf('^'); i > -1; i = line.IndexOf('^', i + 1))
            {
                indices.Add(i);
            }

            splitters.Add(indices);
        }

        return (beamIndex, splitters);
    }
}