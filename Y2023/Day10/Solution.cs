using System.Data;

namespace AdventOfCode.Y2023.Day10;

using Maze = Dictionary<int, Pipe>;

internal record Pipe(char Type, int[] ConnectedTo);

internal class Solution : AdventOfCode.Solution
{
    public override int Year => 2023;

    public override int Day => 10;

    public override string Name => "Pipe Maze";

    private static readonly int
        Up = -140,
        Down = 140,
        Right = 1,
        Left = -1;

    private static readonly Dictionary<char, int[]> PipeDirections = new()
    {
        { '|', new int[]{ Up, Down } },
        { '-', new int[]{ Left, Right } },
        { 'L', new int[]{ Right, Up } },
        { 'J', new int[]{ Left, Up } },
        { 'F', new int[]{ Right, Down } },
        { '7', new int[]{ Left, Down } },
        { 'S', Array.Empty<int>() },
        { '.', Array.Empty<int>() },
    };

    protected override long SolvePart1(string input)
    {
        var maze = ParseMaze(input);

        var loop = FindLoop(maze);

        return (int)Math.Ceiling(decimal.Divide(loop.Count, 2));
    }

    protected override long SolvePart2(string input)
    {
        var maze = ParseMaze(input);

        var loop = FindLoop(maze);

        var numLocationsInsideLoop = CountLocationsInsideLoop(maze, loop);

        return numLocationsInsideLoop;
    }

    private Maze ParseMaze(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var chars = lines
            .SelectMany(l => l)
            .ToArray();

        var maze = chars
            .Select((c, i) =>
            {
                var connectedTo = PipeDirections[c]
                    .Select(direction => i + direction)
                    .Where(index => index >= 0 && index < chars.Length)
                    .ToArray();

                var pipe = new Pipe(c, connectedTo);

                return new KeyValuePair<int, Pipe>(i, pipe);
            })
            .ToDictionary();

        return maze;
    }

    private HashSet<int> FindLoop(Maze maze)
    {
        var startLocation = maze.First(p => p.Value.Type == 'S').Key;

        var visitedLocations = new HashSet<int>();

        var currentLocation = maze
            .First(p => p.Value.ConnectedTo.Contains(startLocation))
            .Key;

        while (maze[currentLocation].Type != 'S')
        {
            visitedLocations.Add(currentLocation);

            currentLocation = maze[currentLocation]
                .ConnectedTo
                .Except(visitedLocations)
                .First();
        }

        visitedLocations.Add(startLocation);

        return visitedLocations;
    }

    private int CountLocationsInsideLoop(
        Maze maze,
        HashSet<int> loop)
    {
        var totalCount = 0;

        for (int row = 0; row < maze.Count() / 140; row++)
        {
            var rowCount = 0;
            var insideLoop = false;

            for (int column = 0; column < 140; column++)
            {
                var i = row * 140 + column;
                var c = maze[i].Type;

                var isApartOfLoop = loop.Contains(i);
                var isFacingUp = PipeDirections[c].Contains(Up);

                if (isApartOfLoop && isFacingUp)
                {
                    insideLoop = !insideLoop;
                }
                else if (!isApartOfLoop && insideLoop)
                {
                    rowCount++;
                }
            }

            totalCount += rowCount;
        }

        return totalCount;
    }
}
