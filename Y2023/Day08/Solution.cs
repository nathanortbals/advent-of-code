using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day08;

internal class Solution : AdventOfCode.Solution
{
    public override int Year => 2023;

    public override int Day => 8;

    public override string Name => "Haunted Wasteland";

    private record Map(string Instructions, Dictionary<string, NodeElements> Nodes);

    private record NodeElements(string Left, string Right);

    protected override long SolvePart1(string input)
    {
        var map = ParseMap(input);

        var steps = FindNumberOfStepsTillMatch(map, "AAA", l => l.Equals("ZZZ"));

        return steps;
    }

    protected override long SolvePart2(string input)
    {
        var map = ParseMap(input);

        var steps = FindNumberOfStepsAsGhost(map);

        return steps;
    }

    private Map ParseMap(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var instructions = lines[0];

        var nodes = lines.Skip(2)
            .Select(l => Regex.Matches(l, @"([A-Z]{3})"))
            .ToDictionary(m => m[0].Value, m => new NodeElements(m[1].Value, m[2].Value));

        return new Map(instructions, nodes);
    }

    private int FindNumberOfStepsTillMatch(Map map, string start, Func<string, bool> match)
    {
        var currentStep = 0;
        var currentLocation = start;

        while (!match(currentLocation))
        {
            var instructionsIndex = currentStep % map.Instructions.Length;

            var instruction = map.Instructions[instructionsIndex];

            var nodeElements = map.Nodes[currentLocation];

            currentLocation = instruction switch
            {
                'L' => nodeElements.Left,
                'R' => nodeElements.Right,
                _ => throw new Exception("Invalid instruction")
            };

            currentStep++;
        }

        return currentStep;
    }


    private long FindNumberOfStepsAsGhost(Map map)
    {
        var starts = map.Nodes.Keys.Where(k => k.EndsWith('A')).ToList();

        var minimumStepsPerStart = starts
            .Select(s => FindNumberOfStepsTillMatch(map, s, l => l.EndsWith('Z')))
            .Select(s => (long)s)
            .ToArray();

        var leastCommonMultiple = LeastCommonMultiple(minimumStepsPerStart);

        return leastCommonMultiple;
    }

    private long LeastCommonMultiple(long[] numbers)
    {
        return numbers.Aggregate(LeastCommonMultiple);
    }

    private long LeastCommonMultiple(long a, long b)
    {
        return a * b / GreatestCommonDivisor(a, b);
    }

    private long GreatestCommonDivisor(long a, long b)
    {
        return b == 0 ? a : GreatestCommonDivisor(b, a % b);
    }
}
