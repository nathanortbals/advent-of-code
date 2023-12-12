using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day03;

internal partial class Solution : AdventOfCode.Solution
{
    public override int Year => 2023;

    public override int Day => 3;

    public override string Name => "Gear Ratios";

    private record SchematicNumber(int Number, SchematicCharacter[] SurroundingCharacters);

    private record SchematicCharacter(char Char, int RowIndex, int ColumnIndex);

    protected override long SolvePart1(string input)
    {
        var schematic = ParseSchematic(input);

        var schematicNumbers = FindSchematicNumbers(schematic);

        var partNumbers = schematicNumbers.Where(IsPartNumber);

        return partNumbers.Select(n => n.Number).Sum();
    }

    protected override long SolvePart2(string input)
    {
        var schematic = ParseSchematic(input);

        var schematicNumbers = FindSchematicNumbers(schematic);

        var gears = schematicNumbers
            .SelectMany(n => n.SurroundingCharacters)
            .Where(c => c.Char == '*')
            .Distinct()
            .ToArray();

        var numbersAttachedToGears = gears
            .Select(gear => schematicNumbers
                .Where(n => n.SurroundingCharacters
                .Contains(gear)))
            .ToArray();

        var gearRatios = numbersAttachedToGears
            .Where(n => n.Count() > 1)
            .Select(n => n
                .Select(x => x.Number)
                .Aggregate(1, (a,b) => a * b))
            .ToArray();

        return gearRatios.Sum();
    }

    private string[] ParseSchematic(string input)
    {
        return input.Split(Environment.NewLine);
    }

    private SchematicNumber[] FindSchematicNumbers(string[] schematic)
    {
        return schematic
            .SelectMany((_, i) => FindSchematicNumbersInRow(schematic, i))
            .ToArray();
    }

    private SchematicNumber[] FindSchematicNumbersInRow(string[] schematic, int rowIndex)
    {
        var row = schematic[rowIndex];

        var matches = Regex.Matches(row, @"\d+");

        var schematicNumbers = matches
            .Select(match => new SchematicNumber(
                int.Parse(match.Value),
                FindSurroundingSchematicCharacters(
                    schematic,
                    rowIndex,
                    match.Index,
                    match.Length)))
            .ToArray();

        return schematicNumbers;
    }

    private bool IsPartNumber(SchematicNumber schematicNumber)
    {
        return schematicNumber.SurroundingCharacters.Any(c => c.Char != '.' && !char.IsDigit(c.Char));
    }

    private SchematicCharacter[] FindSurroundingSchematicCharacters(
        string[] schematic,
        int rowIndex,
        int startingIndex,
        int length)
    {
        var row = schematic[rowIndex];
        var chars = new List<SchematicCharacter>();

        // Left
        if (startingIndex > 0)
        {
            chars.Add(
                new SchematicCharacter(
                    row[startingIndex - 1],
                    rowIndex,
                    startingIndex - 1));
        }

        // Right
        if (startingIndex + length < row.Length)
        {
            chars.Add(new SchematicCharacter(
                row[startingIndex + length],
                rowIndex,
                startingIndex + length));
        }

        // Top
        if (rowIndex > 0)
        {
            var topRow = schematic[rowIndex - 1];
            
            var topRowChars = topRow
                .Substring(startingIndex, length)
                .Select((c, i) => new SchematicCharacter(
                    c,
                    rowIndex - 1,
                    startingIndex + i));

            chars.AddRange(topRowChars);

            // Top left
            if (startingIndex > 0)
            {
                chars.Add(new SchematicCharacter(
                    topRow[startingIndex - 1],
                    rowIndex - 1,
                    startingIndex - 1));
            }

            // Top right
            if (startingIndex + length < topRow.Length)
            {
                chars.Add(new SchematicCharacter(
                    topRow[startingIndex + length],
                    rowIndex - 1,
                    startingIndex + length));
            }
        }

        // Bottom
        if (rowIndex + 1 < schematic.Length)
        {
            var bottomRow = schematic[rowIndex + 1];

            var bottomRowChars = bottomRow
                .Substring(startingIndex, length)
                .Select((c, i) => new SchematicCharacter(
                    c,
                    rowIndex + 1,
                    startingIndex + i));

            chars.AddRange(bottomRowChars);

            // Bottom left
            if (startingIndex > 0)
            {
                chars.Add(new SchematicCharacter(
                    bottomRow[startingIndex - 1],
                    rowIndex + 1,
                    startingIndex - 1));
            }

            // Bottom right
            if (startingIndex + length < bottomRow.Length)
            {
                chars.Add(new SchematicCharacter(
                    bottomRow[startingIndex + length],
                    rowIndex + 1,
                    startingIndex + length));
            }
        }

        return [.. chars];
    }


}
