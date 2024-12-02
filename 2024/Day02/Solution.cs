namespace AdventOfCode._2024.Day02;

public class Solution : ISolution
{
    public object PartOne(string input) => ParseInput(input).Count(IsReportSafe);

    public object PartTwo(string input)
    {
        var safeReportsCount = 0;
        foreach (var report in ParseInput(input).ToList())
        {
            if (IsReportSafe(report))
                safeReportsCount++;
            else if (report.Select((_, j) => GetNewReport(report, j)).Any(IsReportSafe))
                safeReportsCount++;
        }

        return safeReportsCount;
    }

    private static bool IsReportSafe(List<int> report)
    {
        var isIncreasing = report.OrderBy(n => n).SequenceEqual(report);
        var isDecreasing = report.OrderByDescending(n => n).SequenceEqual(report);
        if (!isIncreasing && !isDecreasing)
            return false;

        for (var i = 0; i <= report.Count - 2; i++)
        {
            if (Math.Abs(report[i] - report[i + 1]) < 1 || Math.Abs(report[i] - report[i + 1]) > 3)
                return false;

            if (i == report.Count - 2)
                return true;
        }

        return false;
    }

    private static List<int> GetNewReport(List<int> report, int index)
    {
        var newReport = new List<int>(report);
        newReport.RemoveAt(index);
        return newReport;
    }

    private static IEnumerable<List<int>> ParseInput(string input) => input.Split("\n").Select(line => line.Split(" "))
        .Select(split => split.Select(int.Parse).ToList());
}