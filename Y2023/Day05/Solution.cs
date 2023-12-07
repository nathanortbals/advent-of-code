using System.Data;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day05;

internal class Solution : AdventOfCode.Solution
{
    public override int Year => 2023;

    public override int Day => 5;

    public override string Name => "If You Give A Seed A Fertilizer";

    private record Almanac(long[] Seeds, Map[] Maps);

    private record Map(string Name, MappingRange[] MappingRanges);

    private record MappingRange(Range Source, Range Destination);

    private record Range(long Start, long End);

    protected override long SolvePart1(string input)
    {
        var almanac = ParseAlmanac(input);

        var destinationValues = MapSeedsToLocation(almanac);

        return destinationValues.Min();
    }

    protected override long SolvePart2(string input)
    {
        var almanac = ParseAlmanac(input);

        var seedRanges = ConvertToSeedRanges(almanac.Seeds);

        var locationRanges = MapSeedRangeToLocationRanges(seedRanges, almanac);

        var minimumLocation = locationRanges.Min(r => r.Start);

        return minimumLocation;
    }

    private Almanac ParseAlmanac(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var sections = input.Split(Environment.NewLine + Environment.NewLine);

        var seeds = ParseSeeds(sections[0]);

        var maps = sections.Skip(1).Select(ParseMap).ToArray();

        return new Almanac(seeds, maps);
    }

    private long[] ParseSeeds(string input)
    {
        return Regex
            .Matches(input, @"\d+")
            .Select(m => long.Parse(m.Value))
            .ToArray();
    }

    private Map ParseMap(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var name = lines[0];

        var mappingRanges = lines
            .Skip(1)
            .Select(ParseMappingRange)
            .ToArray();

        return new Map(name, mappingRanges);
    }

    private MappingRange ParseMappingRange(string input)
    {
        var rangeNumbers = input
            .Split(" ");

        var destinationStart = long.Parse(rangeNumbers[0]);
        var sourceStart = long.Parse(rangeNumbers[1]);
        var rangeLength = long.Parse(rangeNumbers[2]);

        var destinationRange = new Range(destinationStart, destinationStart + rangeLength);
        var sourceRange = new Range(sourceStart, sourceStart + rangeLength);

        return new MappingRange(sourceRange, destinationRange);
    }

    private long[] MapSeedsToLocation(Almanac almanac)
    {
        return almanac.Seeds
            .Select(seed => MapSeedToLocation(seed, almanac.Maps))
            .ToArray();
    }

    private long MapSeedToLocation(long seed, Map[] maps)
    {
        return maps.Aggregate(seed, MapToDestination);
    }

    private long MapToDestination(long sourceValue, Map map)
    {
        var mappingRange = map.MappingRanges
            .FirstOrDefault(r =>
                sourceValue >= r.Source.Start &&
                sourceValue < r.Source.End);

        if (mappingRange is null)
        {
            return sourceValue;
        }

        var offset = sourceValue - mappingRange.Source.Start;

        return mappingRange.Destination.Start + offset;
    }

    private Range[] ConvertToSeedRanges(long[] seeds)
    {
        var seedRanges = new List<Range>();

        for (int i = 0; i < seeds.Length; i += 2)
        {
            var start = seeds[i];
            var length = seeds[i + 1];

            var seedRange = new Range(start, start + length);

            seedRanges.Add(seedRange);
        }

        return seedRanges.ToArray();
    }

    private Range[] MapSeedRangeToLocationRanges(Range[] seedRanges, Almanac almanac)
    {
        return almanac.Maps
            .Aggregate(seedRanges, ApplyMapToRanges);
    }

    private Range[] ApplyMapToRanges(Range[] ranges, Map map)
    {
        var rangeQueue = new Queue<Range>(ranges);

        var outputRanges = new List<Range>();

        while (rangeQueue.Any())
        {
            var range = rangeQueue.Dequeue();
            
            // Find overlapping mapping range
            var mappingRange = map.MappingRanges.FirstOrDefault(mr =>
                range.Start <= mr.Source.End &&
                range.End >= mr.Source.Start);

            if (mappingRange is null)
            {
                // No overlap, just add the range
                outputRanges.Add(range);
            }
            else if (
                range.Start >= mappingRange.Source.Start &&
                range.End <= mappingRange.Source.End)
            {
                // Range is completely inside MappingRange
                var startOffset = range.Start - mappingRange.Source.Start;
                var endOffset = range.End - mappingRange.Source.Start;
                var destinationRange = new Range(
                    mappingRange.Destination.Start + startOffset,
                    mappingRange.Destination.Start + endOffset);
                outputRanges.Add(destinationRange);
            }
            else if (
                range.Start >= mappingRange.Source.Start && 
                range.Start < mappingRange.Source.End)
            {
                // Range starts inside MappingRange
                var startOffset = range.Start - mappingRange.Source.Start;

                var destinationRange = new Range(
                    mappingRange.Destination.Start + startOffset,
                    mappingRange.Destination.End);

                var remainingSourceRange = new Range(
                    mappingRange.Source.End + 1,
                    range.End);

                outputRanges.Add(destinationRange);
                rangeQueue.Enqueue(remainingSourceRange);
            }
            else if (
                range.End > mappingRange.Source.Start &&
                range.End <= mappingRange.Source.End)
            {
                // Range ends inside MappingRange
                var endOffset = range.End - mappingRange.Source.Start;

                var destinationRange = new Range(
                    mappingRange.Destination.Start,
                    mappingRange.Destination.Start + endOffset);

                var remainingSourceRange = new Range(
                    range.Start,
                    mappingRange.Source.Start - 1);

                outputRanges.Add(destinationRange);
                rangeQueue.Enqueue(remainingSourceRange);
            }
            else if (
                range.Start < mappingRange.Source.Start &&
                range.End > mappingRange.Source.End)
            {
                // Range completely contains MappingRange
                var destinationRange = new Range(
                    mappingRange.Destination.Start,
                    mappingRange.Destination.End);

                var remainingSourceRangeBefore = new Range(
                    range.Start,
                    mappingRange.Source.Start - 1);

                var remainingSourceRangeAfter = new Range(
                    mappingRange.Source.End + 1,
                    range.End);

                outputRanges.Add(destinationRange);
                rangeQueue.Enqueue(remainingSourceRangeBefore);
                rangeQueue.Enqueue(remainingSourceRangeAfter);
            }
        }

        return outputRanges.ToArray();
    }
}

