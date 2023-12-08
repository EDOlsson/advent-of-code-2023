using System.Text.RegularExpressions;

const string inputPath = "../../../";
// var puzzleInput = await File.ReadAllLinesAsync(Path.Combine(inputPath, "test-input-part2"));
var puzzleInput = await File.ReadAllLinesAsync(Path.Combine(inputPath, "day08-input"));

var instructions = puzzleInput[0];
var nodes = CreateMap(puzzleInput);

var currentNodes = nodes.Values.Where(n => n.Name.EndsWith("A", StringComparison.OrdinalIgnoreCase));
var allHops = currentNodes.Select(n => CalculateHopsFromNode(GenerateInstructions(instructions), nodes, n));

//
// Least Common Multiple of all the hops should be the answer
//
var lcm = allHops.Aggregate(CalculateLeastCommonMultiple);

/*
var lcm2 = nodes.Values
    .Where(n => n.Name.EndsWith("A", StringComparison.OrdinalIgnoreCase))
    .Select(n => CalculateHopsFromNode(GenerateInstructions(instructions), nodes, n))
    .Aggregate(CalculateLeastCommonMultiple);
    */

Console.WriteLine();
Console.WriteLine($"Part 2 : {lcm}");

return 0;

static Dictionary<string, Node> CreateMap(string[] puzzleInput)
{
    return puzzleInput
        .Skip(2)
        .Select(ParseNodeFromLine)
        .ToDictionary(n => n.Name);
}

static Node ParseNodeFromLine(string line)
{
    var r = MyRegex();

    var m = r.Matches(line);

    return new(m[0].Groups["name"].Value, m[0].Groups["left"].Value, m[0].Groups["right"].Value);
}

static Node ExecuteInstructionForNode(char i, IReadOnlyDictionary<string, Node> map, Node currentNode)
{
    return i switch
    {
        'L' => map[currentNode.Left], // map.Single(n => n.Name == currentNode.Left),
        'R' => map[currentNode.Right], // map.Single(n => n.Name == currentNode.Right),
        _ => throw new InvalidOperationException($"Unknown instruction : {i}"),
    };
}

static long CalculateHopsFromNode(IEnumerable<char> instructions, IReadOnlyDictionary<string, Node> map, Node startingNode)
{
    var count = 0L;
    var currentNode = startingNode;
    foreach(var instruction in instructions)
    {
        if (currentNode.Name.EndsWith("Z", StringComparison.OrdinalIgnoreCase))
            break;

        currentNode = ExecuteInstructionForNode(instruction, map, currentNode);
        count++;
    }

    return count;
}

static IEnumerable<char> GenerateInstructions(string instructions)
{
    for (var i = 0L; i < long.MaxValue; ++i)
    {
        var index = (int)(i % instructions.Length);
        yield return instructions[index];
    }
}

static long CalculateLeastCommonMultiple(long a, long b)
{
    //
    // LCM(a,b) = a / GCD(a,b) * b
    //
    return a / CalculateGcd(a, b) * b;

    //
    // GCD(a,b)
    //
    static long CalculateGcd(long a, long b)
    {
        if (b == 0)
            return a;

        return CalculateGcd(b, a % b);
    }
}

record Node(string Name, string Left, string Right);

partial class Program
{
    [GeneratedRegex(@"^(?<name>\w{3}) = \((?<left>\w{3}), (?<right>\w{3})\)$", RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex MyRegex();
}
