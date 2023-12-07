using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day01;

internal class Solution : AdventOfCode.Solution
{
    public override int Year => 2023;

    public override int Day => 1;

    public override string Name => "Trebuchet?!";

    protected override long SolvePart1(string input)
    {
        return SolveGeneric(input, @"\d");
    }

    protected override long SolvePart2(string input)
    {
        return SolveGeneric(input, @"\d|one|two|three|four|five|six|seven|eight|nine");
    }

    private int SolveGeneric(string input, string regex)
    {
        var lines = input.Split(Environment.NewLine);

        var numbers = lines.Select(l =>
        {
            var firstMatch = Regex.Match(l, regex).Value;
            var lastMatch = Regex.Match(l, regex, RegexOptions.RightToLeft).Value;
            var firstNumber = Parse(firstMatch);
            var lastNumber = Parse(lastMatch);
            var number = int.Parse($"{firstNumber}{lastNumber}");

            return number;
        });

        return numbers.Sum();
    }

    private int Parse(string match)
    {
        return match switch
        {
            "one" => 1,
            "two" => 2,
            "three" => 3,
            "four" => 4,
            "five" => 5,
            "six" => 6,
            "seven" => 7,
            "eight" => 8,
            "nine" => 9,
            var number => int.Parse(number),
        };
    }
}
