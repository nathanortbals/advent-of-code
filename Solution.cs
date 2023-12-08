using System.Diagnostics;

namespace AdventOfCode;

internal abstract class Solution
{
    abstract public int Year { get; }

    abstract public int Day { get; }

    abstract public string Name { get; }

    public void RunSolution()
    {
        var input = File.ReadAllText($"./Y{Year}/Day{Day.ToString("00")}/input.txt");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Day {Day} {Year}: {Name}");
        Console.WriteLine();

        var stopwatch = new Stopwatch();
        
        stopwatch.Start();
        var result1 = SolvePart1(input);
        stopwatch.Stop();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Result for Part 1: {result1}");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");
        Console.WriteLine();

        stopwatch.Restart();
        var result2 = SolvePart2(input);
        stopwatch.Stop();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Result for Part 2: {result2}");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");
        Console.WriteLine();

        Console.ResetColor();
    }

    protected abstract long SolvePart1(string input);

    protected abstract long SolvePart2(string input);
}