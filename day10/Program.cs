const string inputPath = "./";
var puzzleInput = await File.ReadAllLinesAsync(Path.Combine(inputPath, "day10-input"));
// var puzzleInput = await File.ReadAllLinesAsync(Path.Combine(inputPath, "test-input"));

Console.WriteLine($"Read {puzzleInput.Length} lines.");

var mainLoop = TraverseMainLoop(puzzleInput);

Console.WriteLine($"Total loop distance is {mainLoop.Count()} steps. Furthest point is {mainLoop.Count() / 2}");

return 0;



static IEnumerable<Position> TraverseMainLoop(string[] puzzleInput)
{
    var start = FindStartingPosition(puzzleInput);
    yield return start;

    var nextPositions = FindNextPositionsFromStart(start, puzzleInput);

    //
    // pick the first "next" position
    //
    var current = nextPositions.First();
    yield return current;

    var previous = start;
    while (current != start)
    {
        var next = GetNextPosition(previous, current, puzzleInput);
        yield return next;

        previous = current;
        current = next;
    }

    static Position FindStartingPosition(string[] puzzleInput)
    {
        for (int i = 0; i < puzzleInput.Length; i++)
        {
            if (puzzleInput[i].Contains('S'))
                return new(i, puzzleInput[i].IndexOf('S'));
        }

        throw new InvalidOperationException("Unable to find the starting position");
    }
}

static IEnumerable<Position> FindNextPositionsFromStart(Position startPosition, string[] puzzleInput)
{
    //
    // Check north of 'S'
    //
    if (0 < startPosition.Row)
    {
        var northPipe = puzzleInput[startPosition.Row - 1][startPosition.Col];
        if (northPipe is '|' or 'F' or '7')
            yield return startPosition with { Row = startPosition.Row - 1 };
    }

    //
    // Check east of 'S'
    //
    if (startPosition.Col < puzzleInput[0].Length - 1)
    {
        var eastPipe = puzzleInput[startPosition.Row][startPosition.Col + 1];
        if (eastPipe is '-' or 'J' or '7')
            yield return startPosition with { Col = startPosition.Col + 1 };
    }

    //
    // Check south of 'S'
    //
    if (startPosition.Row < puzzleInput.Length - 1)
    {
        var southPipe = puzzleInput[startPosition.Row + 1][startPosition.Col];
        if (southPipe is '|' or 'J' or 'L')
            yield return startPosition with { Row = startPosition.Row + 1 };
    }

    //
    // check west of 'S'
    //
    if (0 < startPosition.Col)
    {
        var westPipe = puzzleInput[startPosition.Row][startPosition.Col - 1];
        if (westPipe is '-' or '7' or 'F')
            yield return startPosition with { Col = startPosition.Col - 1 };
    }
}

static Position GetNextPosition(Position previous, Position current, string[] puzzleInput)
{
    var pipe = puzzleInput[current.Row][current.Col];

    return pipe switch
    {
        '|' => previous.Row < current.Row ? current with { Row = current.Row + 1 } : current with { Row = current.Row - 1 },
        '-' => previous.Col < current.Col ? current with { Col = current.Col + 1 } : current with { Col = current.Col - 1 },
        'F' => previous.Row > current.Row ? current with { Col = current.Col + 1 } : current with { Row = current.Row + 1 },
        '7' => previous.Col < current.Col ? current with { Row = current.Row + 1 } : current with { Col = current.Col - 1 },
        'J' => previous.Row < current.Row ? current with { Col = current.Col - 1 } : current with { Row = current.Row - 1 },
        'L' => previous.Row < current.Row ? current with { Col = current.Col + 1 } : current with { Row = current.Row - 1 },
        _ => throw new InvalidOperationException($"Invalid pipe found : {pipe}"),
    };
}

record Position(int Row, int Col);
