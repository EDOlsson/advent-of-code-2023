using System.Collections.Concurrent;
using System.Runtime.Intrinsics.X86;

var puzzleInput = await File.ReadAllLinesAsync("../../../day03-input");

/*
var allPartNumbers = Parts(puzzleInput).Select(p => (p, FindAdjacentPartNumbers(p, puzzleInput)));

var part1 = allPartNumbers
    .SelectMany(n => n.Item2)
    .Select(n => n.Number)
    .Sum();

Console.WriteLine($"Part 1 : {part1}");
*/

var part2 = Parts(puzzleInput)
    .Where(p => p.Kind == '*')
    .Select(p => (p, FindAdjacentPartNumbers(p, puzzleInput)))
    .Where(p => p.Item2.Count() == 2)
    .Select(p => p.Item2.First().Number * p.Item2.Last().Number)
    .Sum();

Console.WriteLine($"Part 2 : {part2}");

static IEnumerable<PossiblePartNumber> FindAdjacentPartNumbers(Part part, string[] puzzleInput)
{
    return PossibleNumbers(puzzleInput)
        .Where(n => part.Y - 1 <= n.Y && n.Y <= part.Y + 1)
        .Where(n => Enumerable.Range(part.X - 1, 3).Intersect(Enumerable.Range(n.X, n.Length)).Any());
}

static IEnumerable<Part> Parts(string[] schematic)
{
    for (var row = 0; row < schematic.Length; row++)
    {
        var currentLine = schematic[row];
        for (var col = 0; col < currentLine.Length; col++)
        {
            if (Char.IsDigit(currentLine[col]))
                continue;

            if (currentLine[col] == '.')
                continue;

            yield return new(currentLine[col], col, row);
        }
    }
}

static IEnumerable<PossiblePartNumber> PossibleNumbers(string[] schematic)
{
    for (var row = 0; row < schematic.Length; row++)
    {
        var currentLine = schematic[row];
        var accumulatedNumber = new List<char>();
        var accumulatedNumberPosition = Tuple.Create(0, 0);

        for (var col = 0; col < currentLine.Length; col++)
        {
            if (Char.IsDigit(currentLine[col]))
            {
                if (accumulatedNumber.Count == 0)
                    accumulatedNumberPosition = Tuple.Create(col, row);

                accumulatedNumber.Add(currentLine[col]);
            }
            else
            {
                if (accumulatedNumber.Count == 0)
                    continue;

                var numberAsString = new string(accumulatedNumber.ToArray());
                var length = accumulatedNumber.Count;

                yield return new(Convert.ToInt32(numberAsString), accumulatedNumberPosition.Item1, accumulatedNumberPosition.Item2, length);

                accumulatedNumber.Clear();
            }
        }

        //
        // See if a number ran right up to the end of the current row
        //
        if (accumulatedNumber.Count > 0)
        {
            var numberAsString = new string(accumulatedNumber.ToArray());
            var length = accumulatedNumber.Count;

            yield return new(Convert.ToInt32(numberAsString), accumulatedNumberPosition.Item1, accumulatedNumberPosition.Item2, length);

            accumulatedNumber.Clear();
        }
    }
}

record PossiblePartNumber(int Number, int X, int Y, int Length);
record Part(char Kind, int X, int Y);