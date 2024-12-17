using System.Text.RegularExpressions;

// ReSharper disable InconsistentNaming

namespace AdventOfCode._2024.Day17;

using Computer = (long A, long B, long C);

public partial class Solution : ISolution
{
    private enum Opcode
    {
        adv,
        bxl,
        bst,
        jnz,
        bxc,
        @out,
        bdv,
        cdv
    }

    public object PartOne(string input)
    {
        var (computer, program) = ParseInput(input);

        return Run(computer, program);
    }

    public object PartTwo(string input)
    {
        throw new NotImplementedException();
    }

    private static string Run(Computer computer, long[] program)
    {
        var output = new List<long>();

        for (var ip = 0; ip < program.Length; ip += 2)
            switch ((Opcode)program[ip], (int)program[ip + 1])
            {
                case (Opcode.adv, var op): computer.A >>= (int)Combo(op); break;
                case (Opcode.bdv, var op): computer.B = computer.A >> (int)Combo(op); break;
                case (Opcode.cdv, var op): computer.C = computer.A >> (int)Combo(op); break;
                case (Opcode.bxl, var op): computer.B ^= op; break;
                case (Opcode.bst, var op): computer.B = Combo(op) % 8; break;
                case (Opcode.jnz, var op): ip = computer.A == 0 ? ip : op - 2; break;
                case (Opcode.bxc, _): computer.B ^= computer.C; break;
                case (Opcode.@out, var op): output.Add(Combo(op) % 8); break;
            }

        return string.Join(',', output);

        long Combo(long operand) => operand switch
        {
            0 or 1 or 2 or 3 => operand, 4 => computer.A, 5 => computer.B, 6 => computer.C, _ => operand
        };
    }


    private static (Computer computer, long[] program) ParseInput(string input) =>
        input.Split("\n\n").Select(ParseNums).ToArray() is var blocks
            ? (new Computer(blocks[0][0], blocks[0][1], blocks[0][2]), blocks[1])
            : default;

    private static long[] ParseNums(string st) =>
        OneOrMoreDigits().Matches(st)
            .Select(m => long.Parse(m.Value))
            .ToArray();

    [GeneratedRegex(@"\d+", RegexOptions.Multiline)]
    private static partial Regex OneOrMoreDigits();
}