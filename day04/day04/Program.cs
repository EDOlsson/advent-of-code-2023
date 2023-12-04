
var puzzleInput = await File.ReadAllLinesAsync("../../../day04-input");

// Console.WriteLine($"Read {puzzleInput.Length} lines");

/*
var sample = puzzleInput
    .Select(c => ParseSingleCard(c)).Take(10)
    .Select(c => (c, CalculateCardPoints(c)));

foreach (var (card, points) in sample)
{
    Console.WriteLine($"Card {card} : {points} points");
}

var part1 = puzzleInput
    .Select(c => ParseSingleCard(c))
    .Select(c => CalculateCardPoints(c))
    .Sum();

Console.WriteLine($"Part 1 : {part1}");
*/

Dictionary<int, long> part2CardCounts = new();

foreach (var card in puzzleInput.Select(ParseSingleCard))
{
    if (part2CardCounts.ContainsKey(card.Id))
        part2CardCounts[card.Id]++;
    else
        part2CardCounts[card.Id] = 1;

    var cardsWon = GetCardsWon(card, puzzleInput.Select(ParseSingleCard));

    //
    // Accumulate the number of cards based on how many copies of the `card` we have
    //
    foreach (var cardWon in cardsWon)
    {
        //
        // Either add the number of copies won, or set the number to the number of copies of cards won
        //
        if (part2CardCounts.ContainsKey(cardWon.Id))
            part2CardCounts[cardWon.Id] += part2CardCounts[card.Id];
        else
            part2CardCounts[cardWon.Id] = part2CardCounts[card.Id];
    }
}

var part2 = part2CardCounts.Values.Sum();

Console.WriteLine($"Part 2 : {part2}");


static Card ParseSingleCard(string card)
{
    //
    // Card input line looks like the following:
    //  Card 1: 1 2 3 4 5 | 6 7 8 9
    //    winning numbers | numbers on card
    //
    var firstSplit = card.Split(":", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

    var r = new System.Text.RegularExpressions.Regex(@"^Card\s+(?<id>\d+)$");
    var id = Convert.ToInt32(r.Matches(firstSplit[0])[0].Groups["id"].Value);

    var numbersSplit =
        firstSplit[1].Split(" | ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

    var winningNumbers = numbersSplit[0]
        .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        .Select(n => Convert.ToInt32(n))
        .ToList();

    var numbersOnCard = numbersSplit[1]
        .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        .Select(n => Convert.ToInt32(n))
        .ToList();

    return new(id, winningNumbers, numbersOnCard);
}

static long CalculateCardPoints(Card card)
{
    var intersection = card.WinningNumbers.Intersect(card.Numbers);

    if (!intersection.Any())
        return 0;

    return (long)Math.Pow(2, intersection.Count() - 1);
}

static IEnumerable<Card> GetCardsWon(Card card, IEnumerable<Card> allCards)
{
    var numberOfCardsWon = card.WinningNumbers.Intersect(card.Numbers).Count();
    return allCards.SkipWhile(c => c.Id <= card.Id).Take(numberOfCardsWon);
}

record Card(int Id, IReadOnlyCollection<int> WinningNumbers, IReadOnlyCollection<int> Numbers);
