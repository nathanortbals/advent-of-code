using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Y2023.Day11;

internal class Solution : AdventOfCode.Solution
{
    public override int Year => 2023;

    public override int Day => 11;

    public override string Name => "Cosmic Expansion";

    private record Position(int X, int Y);

    protected override long SolvePart1(string input)
    {
        return Solve(input, 1);
    }

    protected override long SolvePart2(string input)
    {
        return Solve(input, 999999);
    }

    private long Solve(string input, int expansion)
    {
        var map = ParseMap(input);

        var emptyRows = FindEmptyRows(map);
        var emptyColumns = FindEmptyColumns(map);

        var galaxies = FindGalaxies(map);

        var galaxyPairs = galaxies
            .SelectMany(g1 =>
                galaxies
                    .Where(g2 => g1 != g2)
                    .Select(g2 => (g1, g2)))
            .ToArray();

        var distances = galaxyPairs
            .Select(p => Distance(p.Item1, p.Item2, emptyRows, emptyColumns, expansion))
            .ToArray();

        var sumOfDistances = distances.Sum();

        return sumOfDistances / 2; // Divide by 2 because we're counting each distance twice
    }

    private char[][] ParseMap(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var map = lines
            .Select(l => l.ToCharArray())
            .ToArray();

        return map;
    }

    private int[] FindEmptyRows(char[][] map)
    {
        return Enumerable.Range(0, map.Length)
            .Where(row => IsRowEmpty(map, row))
            .ToArray();
    }

    private int[] FindEmptyColumns(char[][] map)
    {
        return Enumerable.Range(0, map[0].Length)
            .Where(column => IsColumnEmpty(map, column))
            .ToArray();
    }

    private bool IsRowEmpty(char[][] map, int row)
    {
        return map[row].All(c => c == '.');
    }

    private bool IsColumnEmpty(char[][] map, int column)
    {
        return map.All(row => row[column] == '.');
    }

    private Position[] FindGalaxies(char[][] map)
    {
        return Enumerable
            .Range(0, map.Length)
            .SelectMany(y =>
                Enumerable
                    .Range(0, map[y].Length)
                    .Select(x => new Position(x, y)))
            .Where(p => map[p.Y][p.X] == '#')
            .ToArray();
    }

    private long Distance(
        Position p1,
        Position p2,
        int[] emptyRows,
        int[] emptyColumns,
        int expansion)
    {
        var xDistance = Math.Abs(p1.X - p2.X);
        var xMinimum = Math.Min(p1.X, p2.X);
        var xNumOfEmptyRows = Enumerable
            .Range(xMinimum, xDistance)
            .Count(x => emptyColumns.Contains(x));
        var totalXDistance = xDistance + (xNumOfEmptyRows * expansion);

        var yDistance = Math.Abs(p1.Y - p2.Y);
        var yMinimum = Math.Min(p1.Y, p2.Y);
        var yNumOfEmptyRows = Enumerable
            .Range(yMinimum, yDistance)
            .Count(y => emptyRows.Contains(y));
        var totalYDistance = yDistance + (yNumOfEmptyRows * expansion);

        return totalXDistance + totalYDistance;
    }
}
