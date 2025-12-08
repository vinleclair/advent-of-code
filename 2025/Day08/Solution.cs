using System.Numerics;

namespace AdventOfCode._2025.Day08;

public class Solution : ISolution
{
    public object PartOne(string input)
    {
        input =
            "162,817,812\n57,618,57\n906,360,560\n592,479,940\n352,342,300\n466,668,158\n542,29,236\n431,825,988\n739,650,466\n52,470,668\n216,146,977\n819,987,18\n117,168,530\n805,96,715\n346,949,466\n970,615,88\n941,993,340\n862,61,35\n984,92,344\n425,690,689";

        var junctionBoxes = ParseInput(input);

        var (a, b) = GetTwoClosestBoxes(junctionBoxes);
        
        Console.WriteLine(a);
        Console.WriteLine(b);

        return 0;
    }

    private static (Vector3, Vector3) GetTwoClosestBoxes(Vector3[] vectors)
    {
        var maxDistance = float.MaxValue;
        var closest1 = vectors[0];
        var closest2 = vectors[1];

        for (var i = 0; i < vectors.Length - 1; i++)
        {
            for (var j = i + 1; j < vectors.Length; j++)
            {
                var distance = Vector3.Distance(vectors[i], vectors[j]);

                if (!(distance < maxDistance)) continue;

                maxDistance = distance;
                closest1 = vectors[i];
                closest2 = vectors[j];
            }
        }

        return (closest1, closest2);
    }

    private static Vector3[] ParseInput(string input)
    {
        var junctionBoxes = input.Split("\n").Select(line =>
        {
            var coords = line.Split(",").Select(int.Parse).ToArray();
            return new Vector3(coords[0], coords[1], coords[2]);
        }).ToArray();

        return junctionBoxes;
    }
}