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
    Command(args, Args("[0-9]+"), m =>
    {
        var day = int.Parse(m[0]);
        var tsolutionsSelected = tsolutions.Where(tsolution =>
            SolutionExtensions.Day(tsolution) == day);
        return () => Runner.RunAll(GetSolutions(tsolutionsSelected.ToArray()));
    }) ??
    Command(args, Args("all"), m =>
    {
        return () => Runner.RunAll(GetSolutions(tsolutions));
    });

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