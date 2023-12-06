using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day02;

internal class Solution : AdventOfCode.Solution
{
    public override int Year => 2023;

    public override int Day => 2;

    public override string Name => "Cube Conundrum";

    private record Set(int RedCubes, int GreenCubes, int BlueCubes);

    private record Game(int Id, Set[] Sets);

    protected override int SolvePart1(string input)
    {
        var games = ParseGames(input);

        var maxRedCubes = 12;
        var maxGreenCubes = 13;
        var maxBlueCubes = 14;

        var possibleGames = games.Where(game =>
            game.Sets.All(set =>
                set.RedCubes <= maxRedCubes &&
                set.GreenCubes <= maxGreenCubes &&
                set.BlueCubes <= maxBlueCubes));

        return possibleGames.Select(g => g.Id).Sum();
    }

    protected override int SolvePart2(string input)
    {
        var games = ParseGames(input);

        var powers = games.Select(game =>
        {
            var maxRed = game.Sets.Max(s => s.RedCubes);
            var maxGreen = game.Sets.Max(s => s.GreenCubes);
            var maxBlue = game.Sets.Max(s => s.BlueCubes);
            
            return maxRed * maxGreen * maxBlue;
        });

        return powers.Sum();
    }

    private Game[] ParseGames(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var games = lines.Select(ParseGame);

        return games.ToArray();
    }

    private Game ParseGame(string input)
    {
        var idMatch = Regex.Match(input, @"Game (?<id>\d*):");
        var id = int.Parse(idMatch.Groups["id"].Value);

        var sets = ParseSets(input);

        return new Game(id, sets);
    }

    private Set[] ParseSets(string input)
    {
        var setStrs = input.Split(';');
        return setStrs.Select(ParseSet).ToArray();
    }

    private Set ParseSet(string input)
    {
        var redMatch = Regex.Match(input, @"(?<number>\d*) red");
        var greenMatch = Regex.Match(input, @"(?<number>\d*) green");
        var blueMatch = Regex.Match(input, @"(?<number>\d*) blue");

        var red = redMatch.Groups.TryGetValue("number", out var redValue) ?
            int.Parse(redValue.Value) :
            0;

        var green = greenMatch.Groups.TryGetValue("number", out var greenValue) ?
            int.Parse(greenValue.Value) :
            0;

        var blue = blueMatch.Groups.TryGetValue("number", out var blueValue) ?
            int.Parse(blueValue.Value) :
            0;

        return new Set(red, green, blue);
    }
}
