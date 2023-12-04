var puzzleInput = await File.ReadAllLinesAsync("../../../../day02-input");

Console.WriteLine($"Read {puzzleInput.Length} games.");

const int maxRedCubes = 12;
const int maxGreenCubes = 13;
const int maxBlueCubes = 14;

var part1 = puzzleInput
    .Select(ParseGame)
    .Where(IsGamePossible)
    .Sum(g => g.Id);

Console.WriteLine($"Part 1 : {part1}");

var part2 = puzzleInput
    .Select(ParseGame)
    .Select(FindMinimumCubeSetForGame)
    .Select(CalculatePower)
    .Sum();

Console.WriteLine($"Part 2 : {part2}");

static Game ParseGame(string puzzleInputLine)
{
    //
    // Input line looks like Game 1: n blue, m red; p blue, r green; ...
    //
    var firstSplit = puzzleInputLine.Split(": ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

    var gameId = ParseGameId(firstSplit[0]);

    var allRoundsInput  = firstSplit[1].Split("; ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    var allRounds = allRoundsInput.Select(ParseGameRound);

    return new(gameId, allRounds.ToList());

    //
    // Local functions
    //

    static int ParseGameId(string gameIdentifier)
    {
        const string pattern = @"^Game (?<id>\d+)";
        var r = new System.Text.RegularExpressions.Regex(pattern);

        return Convert.ToInt32(r.Matches(gameIdentifier)[0].Groups["id"].Value);
    }

    static GameRound ParseGameRound(string singleRound)
    {
        //
        // Single round looks something like '3 blue, 4 red'
        //
        var setsOfCubes =
            singleRound.Split(", ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var reds = setsOfCubes.Sum(ParseRedCubesDrawn);
        var blues = setsOfCubes.Sum(ParseBlueCubesDrawn);
        var greens = setsOfCubes.Sum(ParseGreenCubesDrawn);

        return new(reds, greens, blues);

        static int ParseBlueCubesDrawn(string setOfCubes)
            => ParseCubesDrawn(setOfCubes, "blue");

        static int ParseRedCubesDrawn(string setOfCubes)
            => ParseCubesDrawn(setOfCubes, "red");

        static int ParseGreenCubesDrawn(string setOfCubes)
            => ParseCubesDrawn(setOfCubes, "green");

        static int ParseCubesDrawn(string setOfCubes, string color)
        {
            var pattern = @$"^(?<cubes>\d+)\s{color}";

            var r = new System.Text.RegularExpressions.Regex(pattern);

            var matches = r.Matches(setOfCubes);
            if (matches.Count == 0)
                return 0;

            return Convert.ToInt32(matches[0].Groups["cubes"].Value);
        }
    }
}

static bool IsGamePossible(Game g)
{
    return g.Rounds.All(r => r is { BlueCubes: <= maxBlueCubes, GreenCubes: <= maxGreenCubes, RedCubes: <= maxRedCubes });
}

static CubeSet FindMinimumCubeSetForGame(Game g)
{
    var fewestReds = g.Rounds.MaxBy(r => r.RedCubes)?.RedCubes ?? 0;
    var fewestGreens = g.Rounds.MaxBy(r => r.GreenCubes)?.GreenCubes ?? 0;
    var fewestBlues = g.Rounds.MaxBy(r => r.BlueCubes)?.BlueCubes ?? 0;

    return new(fewestReds, fewestGreens, fewestBlues);
}

static long CalculatePower(CubeSet set)
    => set.RedCubes * set.GreenCubes * set.BlueCubes;

record GameRound(int RedCubes, int GreenCubes, int BlueCubes);
record Game(int Id, IReadOnlyCollection<GameRound> Rounds);

record CubeSet(int RedCubes, int GreenCubes, int BlueCubes);
