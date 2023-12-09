const string path = "";
var puzzleInput = await File.ReadAllLinesAsync(Path.Combine(path, "day09-input"));
// var puzzleInput = await File.ReadAllLinesAsync(Path.Combine(path, "test-input"));

static IEnumerable<int> ParseHistory(string line)
{
    return line.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        .Select(s => Convert.ToInt32(s));
}

/*
var part1 = puzzleInput
    .Select(ParseHistory)
    .Select(PredictFinalReading)
    .Sum();

Console.WriteLine($"Part 1 : {part1}");
*/

var part2 = puzzleInput
    .Select(ParseHistory)
    .Select(PredictFirstReading)
    .Sum();

Console.WriteLine($"Part 2 : {part2}");

return 0;

/*
static int PredictFinalReading(IEnumerable<int> readings)
{
    if (readings.All(r => r is 0))
        return 0;

    else
        return readings.Last() + PredictFinalReading(CalculateSingleDifference(readings));
}
*/

static int PredictFirstReading(IEnumerable<int> readings)
{
    if (readings.All(r => r is 0))
        return 0;

    else
        return readings.First() - PredictFirstReading(CalculateSingleDifference(readings));
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
