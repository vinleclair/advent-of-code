namespace AdventOfCode._2024.Day11;

public class Solution : ISolution
{
    public object PartOne(string input) => Blink(ParseInput(input), 25).Sum(s => s.Value);
    public object PartTwo(string input) => Blink(ParseInput(input), 75).Sum(s => s.Value);

    private static Dictionary<long, long> Blink(Dictionary<long, long> stones, int blinkCount)
    {
        var updates = new Dictionary<long, long>();

        for (var b = 1; b <= blinkCount; b++)
        {
            updates.Clear();

            foreach (var key in stones.Keys.ToList())
            {
                var count = stones[key];
                if (count <= 0) continue;

                stones[key] = 0;

                if (key == 0)
                    updates[1] = updates.GetValueOrDefault(1) + count;
                else if (key.ToString().Length % 2 == 0)
                {
                    var (firstNumber, secondNumber) = SplitNumber(key.ToString());
                    updates[firstNumber] = updates.GetValueOrDefault(firstNumber) + count;
                    updates[secondNumber] = updates.GetValueOrDefault(secondNumber) + count;
                }
                else
                    updates[key * 2024] = updates.GetValueOrDefault(key * 2024) + count;
            }

            foreach (var update in updates)
                stones[update.Key] = stones.GetValueOrDefault(update.Key) + update.Value;
        }

        return stones;
    }

    private static (long first, long second) SplitNumber(string number) => (long.Parse(number[..(number.Length / 2)]),
        long.Parse(number[(number.Length / 2)..]));

    private static Dictionary<long, long> ParseInput(string input) =>
        input.Split(" ").ToDictionary(long.Parse, _ => (long)1);
}