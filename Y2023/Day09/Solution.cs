using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day09;

internal class Solution : AdventOfCode.Solution
{
    public override int Year => 2023;

    public override int Day => 9;

    public override string Name => "Mirage Maintenance";

    protected override long SolvePart1(string input)
    {
        var histories = ParseInput(input);

        var nextValues = histories.Select(FindValueAfterSequence);

        return nextValues.Sum();
    }

    protected override long SolvePart2(string input)
    {
        var histories = ParseInput(input);

        var previousValues = histories.Select(FindValueBeforeSequence);

        return previousValues.Sum();
    }

    protected int[][] ParseInput(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var histories = lines
            .Select(l => Regex.Matches(l, @"-?\d*"))
            .Select(m => m
                .Where(g => !string.IsNullOrEmpty(g.Value))
                .Select(g => int.Parse(g.Value))
                .ToArray())
            .ToArray();

        return histories;
    }

    private int FindValueAfterSequence(int[] sequence)
    {
        if (sequence.All(v => v == 0))
        {
            return 0;
        }
        
        var nextSequence = new int[sequence.Length - 1];
        for (int i = 0; i < sequence.Length - 1; i++)
        {
            nextSequence[i] = sequence[i + 1] - sequence[i];
        }

        return sequence.Last() + FindValueAfterSequence(nextSequence);
    }

    private int FindValueBeforeSequence(int[] sequence)
    {
        if (sequence.All(v => v == 0))
        {
            return 0;
        }

        var nextSequence = new int[sequence.Length - 1];
        for (int i = sequence.Length - 2; i >= 0; i--)
        {
            nextSequence[i] = sequence[i + 1] - sequence[i];
        }

        return sequence.First() - FindValueBeforeSequence(nextSequence);
    }
}
