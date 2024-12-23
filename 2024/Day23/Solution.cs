using Graph = System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<string>>;

namespace AdventOfCode._2024.Day23;

public class Solution : ISolution
{
    public object PartOne(string input)
    {
        var graph = ParseInput(input);
        var components = graph.Keys.ToHashSet();
        components = Grow(graph, components);
        components = Grow(graph, components);
        return components.Count(component => Members(component).Any(m => m.StartsWith('t')));
    }

    public object PartTwo(string input)
    {
        var graph = ParseInput(input);
        var components = GetSeed(graph);
        while (components.Count > 1)
            components = Grow(graph, components);

        return components.Single();
    }

    private static HashSet<string> Grow(Graph graph, HashSet<string> components) => components
        .AsParallel()
        .Select(component => new { Component = component, Members = Members(component) })
        .SelectMany(
            t => t.Members.SelectMany(member => graph[member]).Distinct(),
            (t, neighbor) => new { t.Component, t.Members, Neighbor = neighbor }
        )
        .Where(t => !t.Members.Contains(t.Neighbor))
        .Where(t => t.Members.All(member => graph[t.Neighbor].Contains(member)))
        .Select(t => Extend(t.Component, t.Neighbor))
        .ToHashSet();

    private static string[] Members(string component) =>
        component.Split(",");

    private static string Extend(string component, string item) =>
        string.Join(",", Members(component).Append(item).OrderBy(x => x));

    private static HashSet<string> GetSeed(Graph g) => g.Keys.ToHashSet();

    private static Graph ParseInput(string input)
    {
        var edges = input
            .Split('\n')
            .SelectMany(line =>
            {
                var nodes = line.Split('-');
                return new[] { (From: nodes[0], To: nodes[1]), (From: nodes[1], To: nodes[0]) };
            });

        var groupedEdges = edges
            .GroupBy(edge => edge.From)
            .ToDictionary(group => group.Key, group => group.Select(edge => edge.To).ToHashSet());

        return new Graph(groupedEdges);
    }
}