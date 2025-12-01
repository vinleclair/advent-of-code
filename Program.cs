using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using AdventOfCode;

if (args.Length > 1 && args[1] == "debug")
{
    Console.WriteLine(Process.GetCurrentProcess());
    Thread.Sleep(10000);
}

var tsolutions = Assembly.GetEntryAssembly()!.GetTypes()
    .Where(t => t.GetTypeInfo().IsClass && typeof(ISolution).IsAssignableFrom(t))
    .OrderBy(t => t.FullName)
    .ToArray();

var action =
    Command(args, Args("all"), m => { return () => Runner.RunAll(GetSolutions(tsolutions)); }) ??
    Command(args, Args("[0-9]+"), m =>
    {
        var year = int.Parse(m[0]);
        var tsolutionsSelected = tsolutions.Where(tsolver =>
            SolutionExtensions.Year(tsolver) == year);
        return () => Runner.RunAll(GetSolutions(tsolutionsSelected.ToArray()));
    }) ??
    Command(args, Args("([0-9]+)/([Dd]ay)?([0-9]+)"), m =>
    {
        var year = int.Parse(m[0]);
        var day = int.Parse(m[2]);
        var tsolutionsSelected = tsolutions.First(tsolution =>
            SolutionExtensions.Year(tsolution) == year &&
            SolutionExtensions.Day(tsolution) == day);
        return () => Runner.RunAll(GetSolutions(tsolutionsSelected));
    }) ??
    Command(args,
        Args("today"), m =>
        {
            var dt = DateTime.UtcNow.AddHours(-5);

            if (dt is not { Month: 12, Day: >= 1 and <= 25 })
                throw new Exception("Event is not active. This option works in Dec 1-25 only)");

            var tsolutionsSelected = tsolutions.First(tsolution =>
                SolutionExtensions.Year(tsolution) == dt.Year &&
                SolutionExtensions.Day(tsolution) == dt.Day);

            return () =>
                Runner.RunAll(GetSolutions(tsolutionsSelected));
        }) ??
    (() => { Console.WriteLine(Usage.Get()); });

action?.Invoke();

return;

ISolution[] GetSolutions(params Type[] tsolution) =>
    tsolution.Select(t => Activator.CreateInstance(t) as ISolution)!.ToArray<ISolution>();

Action? Command(string[] args, string[] regexes, Func<string[], Action> parse)
{
    if (args.Length != regexes.Length)
        return null;

    var matches = args.Zip(regexes, (arg, regex) => new Regex("^" + regex + "$").Match(arg));
    var enumerable = matches.ToList();
    if (!enumerable.All(match => match.Success))
        return null;

    try
    {
        return parse(enumerable.SelectMany(m =>
            m.Groups.Count > 1
                ? m.Groups.Cast<Group>().Skip(1).Select(g => g.Value)
                : [m.Value]
        ).ToArray());
    }
    catch
    {
        return null;
    }
}

string[] Args(params string[] regex) => regex;

internal static class Usage
{
    public static string Get()
    {
        return """
               Advent of Code
               Usage: dotnet run [arguments]
                all             Solve everything
                [year]          Solve the whole year
                [year]/[day]    Solve the specified day 
                today           Shortcut to the above
               """;
    }
}