namespace AdventOfCode._2024.Day05;

public class Solution : ISolution
{
    public object PartOne(string input)
    {
        var (comparer, updates) = ParseInput(input);
        return updates.Where(pages => pages.OrderBy(x => x, comparer).SequenceEqual(pages)).Sum(GetMiddlePage);
    }

    public object PartTwo(string input)
    {
        var (comparer, updates) = ParseInput(input);
        return updates.Where(pages => !pages.OrderBy(x => x, comparer).SequenceEqual(pages))
            .Select(pages => pages.OrderBy(p => p, comparer).ToArray()).Sum(GetMiddlePage);
    }

    private static int GetMiddlePage(string[] pages) => int.Parse(pages[pages.Length / 2]);

    private static (Comparer<string>, string[][]) ParseInput(string input)
    {
        var parts = input.Split("\n\n");
        var ordering = new HashSet<string>(parts[0].Split("\n"));
        return (Comparer<string>.Create((p1, p2) => ordering.Contains(p1 + "|" + p2) ? -1 : 1),
            parts[1].Split("\n").Select(line => line.Split(",")).ToArray());
    }
}