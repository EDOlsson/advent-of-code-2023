// var allRaces = await ParseInputAsync("../../../test-input");
var allRaces = await ParseInputAsync("../../../day06-input");

/*
var part1 = allRaces
    .Select(FindNumberOfWaysToBeatRecord)
    .Aggregate((a, b) => a * b);

Console.WriteLine($"Part 1 : {part1}");
*/

// var theOneRace = await ParseInputForPart2Async("../../../test-input");
var theOneRace = await ParseInputForPart2Async("../../../day06-input");
var part2 = FindNumberOfWaysToBeatRecordInTheLongRace(theOneRace);

Console.WriteLine($"Part 2 : {part2}");

static long FindNumberOfWaysToBeatRecord(Race race)
{
    return Enumerable.Range(1, race.Time)
        .Select(holdTime => ExceedsBestDistance(race, holdTime))
        .SkipWhile(result => !result)
        .TakeWhile(result => result)
        .LongCount();
}

static bool ExceedsBestDistance(Race race, int holdTime)
{
    //
    // holdTime = speed (e.g., holdTime of 1 = speed of 1 unit/s; holdTime of 2 = speed of 2 unit/s; etc.
    //
    var travelTime = race.Time - holdTime;
    var distanceTraveled = holdTime * travelTime;

    return distanceTraveled > race.BestDistance;
}

static async ValueTask<IEnumerable<Race>> ParseInputAsync(string fileName)
{
    var allLines = await File.ReadAllLinesAsync(fileName);
    var timesSplit = allLines[0].Split(":", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    var allTimes = timesSplit[1].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        .Select(t => Convert.ToInt32(t));

    var distancesSplit = allLines[1].Split(":", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    var allDistances = distancesSplit[1]
        .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        .Select(d => Convert.ToInt32(d));

    return allTimes.Zip(allDistances, (time, distance) => new Race(time, distance));
}

static async ValueTask<LongRace> ParseInputForPart2Async(string fileName)
{
    var allLines = await File.ReadAllLinesAsync(fileName);
    var timesSplit = allLines[0].Split(":", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    var longTime = Convert.ToInt64(new string(FixBadKerning(timesSplit[1]).ToArray()));

    var distancesSplit = allLines[1].Split(":", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    var longDistance = Convert.ToInt64(new string(FixBadKerning(distancesSplit[1]).ToArray()));

    return new(longTime, longDistance);

    static IEnumerable<char> FixBadKerning(string input)
    {
        foreach (var c in input)
        {
            if (Char.IsDigit(c))
                yield return c;
        }
    }
}

static long FindNumberOfWaysToBeatRecordInTheLongRace(LongRace race)
{
    return HoldTimes(race.Time)
        .Select(t => ExceedsBestLongDistance(race, t))
        .SkipWhile(r => !r)
        .TakeWhile(r => r)
        .LongCount();

    static IEnumerable<long> HoldTimes(long maxTime)
    {
        for (var i = 1L; i < maxTime; i++)
        {
            yield return i;
        }
    }
}

static bool ExceedsBestLongDistance(LongRace race, long holdTime)
{
    var travelTime = race.Time - holdTime;
    var distance = holdTime * travelTime;
    return distance > race.BestDistance;
}

record Race(int Time, int BestDistance);

record LongRace(long Time, long BestDistance);
