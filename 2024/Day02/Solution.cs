namespace AdventOfCode._2024.Day02;

public class Solution : ISolution
{
    public object PartOne(string input) => ParseInput(input).Count(IsReportSafe);

    public object PartTwo(string input) => ParseInput(input)
        .Count(report => IsReportSafe(report) ||
                         report.Select((_, j) => GetNewReport(report, j)).Any(IsReportSafe));

    private static bool IsReportSafe(List<int> report) =>
        (report.OrderBy(n => n).SequenceEqual(report) || report.OrderByDescending(n => n).SequenceEqual(report)) &&
        Enumerable.Range(0, report.Count - 1).All(i =>
            Math.Abs(report[i] - report[i + 1]) >= 1 && Math.Abs(report[i] - report[i + 1]) <= 3);

    private static List<int> GetNewReport(List<int> report, int index)
    {
        var newReport = new List<int>(report);
        newReport.RemoveAt(index);
        return newReport;
    }

    private static IEnumerable<List<int>> ParseInput(string input) => input.Split("\n").Select(line => line.Split(" "))
        .Select(split => split.Select(int.Parse).ToList());
}