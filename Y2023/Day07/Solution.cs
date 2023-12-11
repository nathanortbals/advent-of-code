using System.Numerics;

namespace AdventOfCode.Y2023.Day07;

internal class Solution : AdventOfCode.Solution
{
    public override int Year => 2023;

    public override int Day => 7;

    public override string Name => "Camel Cards";

    private record Hand(string Cards, int Bid); 

    protected override long SolvePart1(string input)
    {
        var hands = ParseHands(input);

        var rankedHands = hands.OrderBy(h => GetHandValue(h.Cards, false));

        var totalWinnings = rankedHands
            .Select((h, i) => h.Bid * (i + 1))
            .Sum();

        return totalWinnings;
    }

    protected override long SolvePart2(string input)
    {
        return 0;
    }

    private Hand[] ParseHands(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var cards = lines.Select(l =>
        {
            var sections = l.Split(" ");

            var cards = sections[0];
            var bid = int.Parse(sections[1]);

            return new Hand(cards, bid);
        }).ToArray();

        return cards;
    }

    private long GetHandValue(string hand, bool jIsWildCard)
    {
        var cardOrder = jIsWildCard ? "J123456789TQKA" : "123456789TJQKA";

        var patternValue = GetPatternValue(hand);
        var cardValue = GetCardValue(hand, cardOrder);

        if (patternValue.ToString().Length != 5)
        {
            throw new Exception("Pattern value is not 5 digits long.");
        }

        var value = long.Parse(patternValue.ToString("D5") + cardValue.ToString("D12"));

        return value;
    }

    private int GetPatternValue(string cards)
    {
        // Convert the cards into the number of occurrences of the card in the hand,
        // then sort them from highest to lowest. Then turn it into a number.
        //
        // Ex: 22222 -> 55555, 22223 -> 44441, AKAA2 -> 33311, Q6KQK -> 22221, 

        var digits = cards
            .Select(c => cards.Count(c2 => c == c2))
            .OrderDescending()
            .Aggregate("", (s, c) => s + c.ToString());

        var value = int.Parse(digits);

        return value;
    }

    private long GetCardValue(string cards, string cardOrder)
    {
        // Convert the cards into their corresponding value (by index of card order)
        // then turn it into a number. Adding a leading zero to single digit cards 
        // to make sure they are at the right place in the number.
        //
        // Ex for card order "23456789TJQKA":
        // Ex: 22222 -> (00)(00)(00)(00)(00),
        // 22223 -> (00)(00)(00)(00)(02),
        // AKAA2 -> (12)(11)(12)(12)(00),
        // Q6KQK -> (10)(06)(11)(10)(11)

        var digits = cards
            .Select(c => (byte)cardOrder.IndexOf(c))
            .Reverse()
            .ToArray();

        var value = new BigInteger(digits);

        return (long)value;
    }
}
