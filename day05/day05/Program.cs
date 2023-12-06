var puzzleInput = await File.ReadAllLinesAsync("../../../day05-input");
// var puzzleInput = await File.ReadAllLinesAsync("../../../test-input");

var seeds = ParseSeeds(puzzleInput[0]);

var seedToSoillMap = ParseAlmanacMap("seed-to-soil map", puzzleInput);
var soilToFertilizerMap = ParseAlmanacMap("soil-to-fertilizer map", puzzleInput);
var fertilizerToWaterMap = ParseAlmanacMap("fertilizer-to-water map", puzzleInput);
var waterToLightMap = ParseAlmanacMap("water-to-light map", puzzleInput);
var lightToTemperatureMap = ParseAlmanacMap("light-to-temperature map", puzzleInput);
var temperatureToHumidityMap = ParseAlmanacMap("temperature-to-humidity map", puzzleInput);
var humidityToLocationMap = ParseAlmanacMap("humidity-to-location map", puzzleInput);

var part1 = seeds
    .Select(s => MapSeed(seedToSoillMap, s))
    .Select(s => MapSeed(soilToFertilizerMap, s))
    .Select(s => MapSeed(fertilizerToWaterMap, s))
    .Select(s => MapSeed(waterToLightMap, s))
    .Select(s => MapSeed(lightToTemperatureMap, s))
    .Select(s => MapSeed(temperatureToHumidityMap, s))
    .Select(s => MapSeed(humidityToLocationMap, s))
    .Min();

// Console.WriteLine($"Part 1 : {part1}");

static IEnumerable<long> ParseSeeds(string almanacSeeds)
{
    var seedSplit = almanacSeeds.Split(":", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

    var parsedSeeds = seedSplit[1].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

    return parsedSeeds.Select(s => Convert.ToInt64(s));
}

var allSeedRanges = ParseSeedsForPart2(puzzleInput[0]);

var (_, part2) = Locations(humidityToLocationMap)
    .Select(loc => ReverseMapValue(humidityToLocationMap, loc, loc))
    .Select(t => ReverseMapValue(temperatureToHumidityMap, t.Item1, t.Item2))
    .Select(t => ReverseMapValue(lightToTemperatureMap, t.Item1, t.Item2))
    .Select(t => ReverseMapValue(waterToLightMap, t.Item1, t.Item2))
    .Select(t => ReverseMapValue(fertilizerToWaterMap, t.Item1, t.Item2))
    .Select(t => ReverseMapValue(soilToFertilizerMap, t.Item1, t.Item2))
    .Select(t => ReverseMapValue(seedToSoillMap, t.Item1, t.Item2))
    .First(results => SeedExists(allSeedRanges, results.Item1));

Console.WriteLine($"Part 2 {part2}");

static IEnumerable<SeedRange> ParseSeedsForPart2(string almanacSeeds)
{
    var seedValues = ParseSeeds(almanacSeeds);

    return seedValues
        .Chunk(2)
        .Select(c => CreateSeedRange(c[0], c[1]));
}

static AlmanacMap ParseAlmanacMap(string almanacName, string[] puzzleInput)
{
    var rangeMaps = puzzleInput
        .SkipWhile(l => !string.Equals(l, $"{almanacName}:", StringComparison.OrdinalIgnoreCase))
        .Skip(1)
        .TakeWhile(l => l.Length > 0)
        .Select(ParseSingleRangeMap)
        .ToList();

    return new(almanacName, rangeMaps);
}


static RangeMap ParseSingleRangeMap(string rangeMapEntry)
{
    var mapElements = rangeMapEntry.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

    return new(Convert.ToInt64(mapElements[0]), Convert.ToInt64(mapElements[1]), Convert.ToInt64(mapElements[2]));
}

static long MapSeed(AlmanacMap map, long seed)
{
    var rangeMap = map.Maps.SingleOrDefault(m => m.SourceStart <= seed && seed < m.SourceStart + m.Length);

    //
    // No map for this seed--just return the seed value
    //
    if (rangeMap == default)
        return seed;

    var offset = seed - rangeMap.SourceStart;
    return rangeMap.DestinationStart + offset;
}

static SeedRange CreateSeedRange(long seedStart, long rangeLength)
    => new(seedStart, rangeLength);

static bool SeedRangeIntersectsMap(SeedRange seeds, RangeMap map)
    => !(seeds.Start + seeds.Length - 1 < map.SourceStart || map.SourceStart + map.Length < seeds.Start);

static IEnumerable<long> CreateSeedsFromRange(long seedStart, long seedLength)
{
    for (var s = seedStart; s < seedStart + seedLength; s++)
        yield return s;
}

static (long, long) ReverseMapValue(AlmanacMap map, long input, long location)
{
    var rangeMap = map.Maps.SingleOrDefault(m => m.DestinationStart <= input && input < m.DestinationStart + m.Length);

    if (rangeMap == default)
    {
        // Console.WriteLine($"    {map.Name} : {input} -> {input}.");
        return (input, location);
    }

    var offset = input - rangeMap.DestinationStart;
    // Console.WriteLine($"    {map.Name} : {input} -> {rangeMap.SourceStart + offset}");
    return (rangeMap.SourceStart + offset, location);
}

static IEnumerable<long> Locations(AlmanacMap sourceMap)
{
    for (var i = 0L; i < Int64.MaxValue; i++)
        yield return i;
}

static bool SeedExists(IEnumerable<SeedRange> seedRanges, long testSeed)
    => seedRanges.Any(r => r.Start <= testSeed && testSeed < r.Start + r.Length);

record RangeMap(long DestinationStart, long SourceStart, long Length);
record AlmanacMap(string Name, IReadOnlyCollection<RangeMap> Maps);
record SeedRange(long Start, long Length);
