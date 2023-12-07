using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day04;

internal class Solution : AdventOfCode.Solution
{
    public override int Year => 2023;

    public override int Day => 4;

    public override string Name => "Scratchcards";

    private record Scratchcard(int CardNumber, int[] WinningNumbers, int[] Numbers);

    protected override long SolvePart1(string input)
    {
        var scratchcards = ParseScratchcards(input);

        var scores = scratchcards.Select(ScoreScratchcard).ToArray();

        return scores.Sum();
    }

    protected override long SolvePart2(string input)
    {
        var scratchcards = ParseScratchcards(input);

        var numberOfCards = TallyNumberOfCards(scratchcards);

        return numberOfCards;
    }

    private Scratchcard[] ParseScratchcards(string input)
    {
        var lines = input.Split(Environment.NewLine);

        return lines.Select(ParseScratchcard).ToArray();
    }

    private Scratchcard ParseScratchcard(string input)
    {
        var parts = input.Split(':', '|');

        var cardNumber = int.Parse(Regex.Match(parts[0], @"\d+").Value);
        var winningNumbers = Regex
            .Matches(parts[1], @"\d+")
            .Select(m => int.Parse(m.Value))
            .ToArray();
        var numbers = Regex
            .Matches(parts[2], @"\d+")
            .Select(m => int.Parse(m.Value))
            .ToArray();

        return new Scratchcard(cardNumber, winningNumbers, numbers);
    }

    private int ScoreScratchcard(Scratchcard scratchcard)
    {
        var numberOfMatches = scratchcard
            .WinningNumbers
            .Intersect(scratchcard.Numbers)
            .Count();

        var score = numberOfMatches == 0 ?
            0 :
            (int)Math.Pow(2, numberOfMatches - 1);

        return score;
    }

    private int TallyNumberOfCards(Scratchcard[] scratchcards)
    {
        var tallies = Enumerable.Repeat(1, scratchcards.Length).ToArray();

        for (var i = 0; i < scratchcards.Length; i++)
        {
            var scratchcard = scratchcards[i];

            var numberOfMatches = scratchcard
                .WinningNumbers
                .Intersect(scratchcard.Numbers)
                .Count();

            var numberOfTallies = tallies[i];

            for (var j = 1; j <= numberOfMatches; j++)
            {
                tallies[i + j] += numberOfTallies;
            }
        }

        return tallies.Sum();
    }
}
