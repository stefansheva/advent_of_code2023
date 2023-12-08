using System.Text.RegularExpressions;

const string CardsSeparator = "\n";
const string CardNumberSeparator = "|";
const string NormalizationRegex = @"(Card \d+:)";

var gameInput = @"
Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11";

gameInput = await File.ReadAllTextAsync("./input.txt");
var normalizeInput = Regex.Replace(gameInput, NormalizationRegex, string.Empty);
var cards = normalizeInput.Trim().Split(CardsSeparator);

Console.WriteLine("Day 4, Part One START");
var winningNumbersCounter = 0.0;
const int expBase = 2;
foreach (var card in cards)
{
    var groupedCards = card.Split(CardNumberSeparator);
    var winningNumbers = groupedCards
        .First()
        .Split(" ")
        .Where(x => !string.IsNullOrWhiteSpace(x));
    var pickedNumbers = groupedCards
        .Last()
        .Split(" ")
        .Where(x => !string.IsNullOrWhiteSpace(x))
        .Count(num => winningNumbers.Contains(num.Trim()));

    winningNumbersCounter += pickedNumbers > 0 ? Math.Pow(expBase, pickedNumbers - 1) : 0;
}

Console.WriteLine($"Total Points: {winningNumbersCounter}");
Console.WriteLine("Day 4, Part One END");

Console.WriteLine("Day 4, Part TWO START");

var scratchcards = normalizeInput.Trim().Split(CardsSeparator).ToList();
var list = scratchcards.Select(x => new ScratchCard(1, x)).ToList();

for (var i = 0; i < list.Count; i++)
{
    for (var j = 0; j < list[i].Count; j++)
    {
        var groupedCards = list[i].Value.Split(CardNumberSeparator);
        var winningNumbers = groupedCards
            .First()
            .Split(" ")
            .Where(x => !string.IsNullOrWhiteSpace(x));
        var totalWinnedCards = groupedCards
            .Last()
            .Split(" ")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Count(num => winningNumbers.Contains(num.Trim()));

        list.Skip(i + 1).Take(totalWinnedCards).ToList().ForEach(x => x.Count += 1);
    }
}

var totalScratchCards = list.Select(x => x.Count).Sum();
Console.WriteLine($"Total cards: {totalScratchCards}");
Console.WriteLine("Day 4, Part TWO END");
class ScratchCard
{
    public int Count { get; set; }
    public string Value { get; set; }

    public ScratchCard(int count, string value)
    {
        Count = count;
        Value = value;
    }
}

