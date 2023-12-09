const string path = "";
var puzzleInput = await File.ReadAllLinesAsync(Path.Combine(path, "day09-input"));
// var puzzleInput = await File.ReadAllLinesAsync(Path.Combine(path, "test-input"));

static IEnumerable<int> ParseHistory(string line)
{
    return line.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        .Select(s => Convert.ToInt32(s));
}

var part1 = puzzleInput
    .Select(ParseHistory)
    .Select(PredictReading)
    .Sum();

Console.WriteLine($"Part 1 : {part1}");

return 0;

static int PredictReading(IEnumerable<int> readings)
{
    if (readings.All(r => r is 0))
        return 0;

    else
        return readings.Last() + PredictReading(CalculateSingleDifference(readings));
}

static IEnumerable<int> CalculateSingleDifference(IEnumerable<int> input)
{
    var previousReading = input.First();
    foreach (var reading in input.Skip(1))
    {
        yield return reading - previousReading;
        previousReading = reading;
    }
}
