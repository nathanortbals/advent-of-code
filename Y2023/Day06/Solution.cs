
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Xml;

namespace AdventOfCode.Y2023.Day06;

internal class Solution : AdventOfCode.Solution
{
    public override int Year => 2023;

    public override int Day => 6;

    public override string Name => "Wait For It";

    private record Race(long Time, long Record);

    protected override long SolvePart1(string input)
    {
        var races = ParseRaces(input);

        var numWinningDistances = races.Select(FindNumberOfWinningRaces);

        return numWinningDistances.Aggregate(1, (p, r2) => p * r2);
    }

    protected override long SolvePart2(string input)
    {
        var race = ParseAsSingleRace(input);

        var numWinningDistances = FindNumberOfWinningRacesFast(race);

        return numWinningDistances;
    }

    private Race[] ParseRaces(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var times = Regex
            .Matches(lines[0], @"\d+")
            .Select(m => long.Parse(m.Value))
            .ToArray();

        var records = Regex
            .Matches(lines[1], @"\d+")
            .Select(m => long.Parse(m.Value))
            .ToArray();

        return times
            .Select((t, i) => new Race(t, records[i]))
            .ToArray();
    }

    private Race ParseAsSingleRace(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var timeStr = Regex.Replace(lines[0], @"\s+", "");
        var recordStr = Regex.Replace(lines[1], @"\s+", "");

        var time = long.Parse(Regex.Match(timeStr, @"\d+").Value);
        var record = long.Parse(Regex.Match(recordStr, @"\d+").Value);

        return new Race(time, record);
    }

    private int FindNumberOfWinningRaces(Race race)
    {
        var numWinningRaces = 0;

        for (int timeHoldingButton = 0; timeHoldingButton < race.Time; timeHoldingButton++)
        {
            var distance = timeHoldingButton * (race.Time - timeHoldingButton);

            if (distance > race.Record)
            {
                numWinningRaces++;
            }
        }

        return numWinningRaces;
    }

    private long FindNumberOfWinningRacesFast(Race race)
    {
        // x = time holding button, y = distance
        // y = x * (time - x)
        // y = record

        // x * (time - x) = record
        // 0 = x^2 - (time)x + (record)
        // a = 1, b = -time, c = record
 
        // Quadratic formula
        // x = (-b ± √(b^2 - 4ac)) / 2a

        var b = race.Time * -1;
        var c = race.Record;

        var discriminant = Math.Pow(b, 2) - 4 * c;
        var sqrtDiscriminant = Math.Sqrt(discriminant);
        var x1 = ((-1 * b) + sqrtDiscriminant) / 2;
        var x2 = ((-1 * b) - sqrtDiscriminant) / 2;

        var winningDistances = (long)(x1 - x2);

        return winningDistances;
    }
}
