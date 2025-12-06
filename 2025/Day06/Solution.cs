namespace AdventOfCode._2025.Day06;

public class Solution : ISolution
{
    public object PartOne(string input)
    {
        var lines = ParseInput(input);

        var total = 0L;

        for (var i = 0; i < lines[0].Length; i++)
        {
            long sum = int.Parse(lines[0][i]);

            switch (lines[^1][i])
            {
                case "+":
                {
                    for (var j = 1; j < lines.Count - 1; j++)
                        sum += int.Parse(lines[j][i]);
                    break;
                }
                case "*":
                {
                    for (var j = 1; j < lines.Count - 1; j++)
                        sum *= int.Parse(lines[j][i]);
                    break;
                }
            }

            total += sum;
        }

        return total;
    }

    public object PartTwo(string input)
    {
        var lines = input.Split("\n");

        var op = '+';
        long total = 0, sum = 0;

        for (var col = 0; col < lines[0].Length; col++)
        {
            if (lines[^1][col] != ' ')
            {
                total += sum;
                sum = 0;
                op = lines[^1][col];
            }

            var colNum = 0;
            for (var row = 0; row < lines.Length - 1; row++)
                if (char.IsDigit(lines[row][col]))
                    colNum = colNum * 10 + (lines[row][col] - '0');

            if (sum == 0)
                sum = colNum;
            else if (colNum != 0)
                sum = op == '+' ? sum + colNum : sum * colNum;
        }

        return total + sum;
    }

    private static List<string[]> ParseInput(string input) =>
        input.Split("\n").Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToList();
}