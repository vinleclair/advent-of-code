namespace AdventOfCode;

public static class SolutionExtensions
{
    public static IEnumerable<object> Solve(this ISolution solver, string input)
    {
        yield return solver.PartOne(input);
        var res = solver.PartTwo(input);
        if (res != null)
        {
            yield return res;
        }
    }
    
    public static string DayName(this ISolution solution) => $"Day {solution.Day()}";

    public static int Day(Type t) => int.Parse(t.FullName?.Split('.')[2][3..]!);
    
    private static int Day(this ISolution solution) => Day(solution.GetType());
    
    public static int Year(Type t) => int.Parse(t.FullName?.Split('.')[1][1..]!);
    
    private static int Year(this ISolution solution) => Year(solution.GetType());
    
    public static string WorkingDir(this ISolution solution) => WorkingDir(solution.Year(), solution.Day());

    private static string WorkingDir(int year) => Path.Combine(year.ToString());

    private static string WorkingDir(int year, int day) => Path.Combine(WorkingDir(year), "Day" + day.ToString("00"));

}