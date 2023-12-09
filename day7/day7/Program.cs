
const string cardBidSeparator = " ";
const string handsSeparator = "\n";

var gameInput = @"32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483
";

gameInput = await File.ReadAllTextAsync("./input.txt");
var hands = gameInput.Trim().Split(handsSeparator);
Console.WriteLine("Day 7, Part One START");

var cardValues = new List<(char, int)>
{
    new('A', 14),
    new('K', 13),
    new('Q', 12),
    new('J', 11),
    new('T', 10),
    new('9', 9),
    new('8', 8),
    new('7', 7),
    new('6', 6),
    new('5', 5),
    new('4', 4),
    new('3', 3),
    new('2', 2),
};

int MapCard(char item)
{
    return cardValues.First(x => x.Item1 == item).Item2;
}

var allHands = new List<CamelCard>();
foreach (var hand in hands)
{
    var orderedCards = new List<int>();
    var cards = hand.Trim().Split(cardBidSeparator).First();
    var bid = hand.Trim().Split(cardBidSeparator).Last();
    foreach (var card in cards)
    {
        var t = MapCard(card);
        orderedCards.Add(t);
    }

    allHands.Add(new(long.Parse(bid), orderedCards.ToList()));
}

var groupedHands = allHands
    .GroupBy(x => x.HandType)
    .OrderBy(x => x.First().HandRank)
    .ToList();

var currentRank = 1;
foreach (var hand in groupedHands)
{
    if (hand.Count() == 1)
    {
        hand.ToList().ForEach(r => r.Rank = currentRank);
        currentRank += 1;
    }
    else
    {
        var sortedHands = hand.OrderBy(card => card.Cards, new CardListComparer()).ToList();

        for (int i = 0; i < sortedHands.Count; i++)
        {
            sortedHands[i].Rank = currentRank + i;
        }
        currentRank += sortedHands.Count;
    }
}

var bidRankValues = groupedHands
    .SelectMany(x => x.ToList().Select(y => (y.Bid, y.Rank)))
    .ToList();

var totalWinnings = bidRankValues.Sum(x => x.Bid * x.Rank);

Console.WriteLine($"Total Points: {totalWinnings}");
Console.WriteLine("Day 7, Part One END");

Console.WriteLine("Day 7, Part TWO START");

var cardMapper = new List<(char, int)>
{
    new('A', 14),
    new('K', 13),
    new('Q', 12),
    new('T', 11),
    new('9', 10),
    new('8', 9),
    new('7', 8),
    new('6', 7),
    new('5', 6),
    new('4', 5),
    new('3', 4),
    new('2', 3),
    new('J', 2),
};

int MapCardWithJokers(char item)
{
    return cardMapper.First(x => x.Item1 == item).Item2;
}

var handsWithJokers = new List<CamelCard>();
foreach (var hand in hands)
{
    var orderedCards = new List<int>();
    var cards = hand.Trim().Split(cardBidSeparator).First();
    var bid = hand.Trim().Split(cardBidSeparator).Last();

    foreach (var card in cards)
    {
        var t = MapCardWithJokers(card);
        orderedCards.Add(t);
    }

    handsWithJokers.Add(new(long.Parse(bid), orderedCards.ToList(), true));
}

var grouped = handsWithJokers
    .GroupBy(x => x.HandType)
    .OrderBy(x => x.First().HandRank)
    .ToList();

var currentRankJokers = 1;
foreach (var hand in grouped)
{
    if (hand.Count() == 1)
    {
        hand.ToList().ForEach(r => r.Rank = currentRankJokers);
        currentRankJokers += 1;
    }
    else
    {
        var sortedHands = hand.OrderBy(card => card.Cards, new CardListComparer()).ToList();

        for (int i = 0; i < sortedHands.Count; i++)
        {
            sortedHands[i].Rank = currentRankJokers + i;
        }
        currentRankJokers += sortedHands.Count;
    }
}

var bidRankTuple = grouped
    .SelectMany(x => x.ToList().Select(y => (y.Bid, y.Rank)))
    .ToList();

var totalWinningsWithJokers = bidRankTuple.Sum(x => x.Bid * x.Rank);

Console.WriteLine($"Total cards: {totalWinningsWithJokers}");
Console.WriteLine("Day 7, Part TWO END");

public class CardListComparer : IComparer<List<int>>
{
    public int Compare(List<int> cards1, List<int> cards2)
    {
        for (var i = 0; i < Math.Min(cards1.Count, cards2.Count); i++)
        {
            if (cards1[i] != cards2[i])
            {
                return cards1[i].CompareTo(cards2[i]);
            }
        }

        return 0; // Lists are equal up to the minimum count
    }
}

enum HandType
{
    FiveOfKind,
    FourOfKind,
    FullHouse,
    ThreeOfKind,
    TwoPairs,
    OnePair,
    HighCard
}

class CamelCard
{
    public long Bid { get; set; }
    public List<int> Cards { get; set; }
    public HandType HandType { get; set; }
    public int HandRank { get; set; }
    public int Rank { get; set; }

    public CamelCard(long bid, List<int> cards, bool calculateJokers = false)
    {
        Bid = bid;
        Cards = cards;

        if (calculateJokers)
        {
            // Part two, checking for jokers
            if (cards.Any(x => x == 2))
            {
                var jokerNumber = 2;
                var mostFrequent = MostFrequentElement(cards);
                var items = cards.Select(num => num == jokerNumber ? mostFrequent : num).ToList();
                CalculateHand(items);
            }
            // part two, ends
            else
            {
                CalculateHand(Cards);
            }
        }
        else
        {
            CalculateHand(Cards);
        }
    }

    private void CalculateHand(List<int> cards)
    {
        if (IsFiveOfKind(cards))
        {
            HandType = HandType.FiveOfKind;
            HandRank = 7;
        } 
        else if (IsFourOfKind(cards))
        {
            HandType = HandType.FourOfKind;
            HandRank = 6;
        }
        else if (IsFullHouse(cards))
        {
            HandType = HandType.FullHouse;
            HandRank = 5;
        }
        else if (IsThreeOfKind(cards))
        {
            HandType = HandType.ThreeOfKind;
            HandRank = 4;
        }
        else if (IsTwoPairs(cards))
        {
            HandType = HandType.TwoPairs;
            HandRank = 3;
        }
        else if (IsOnePair(cards))
        {
            HandType = HandType.OnePair;
            HandRank = 2;
        }
        else if (IsHighCard(cards))
        {
            HandType = HandType.HighCard;
            HandRank = 1;
        }
    }

    private int MostFrequentElement(List<int> numbers)
    {
        // Find the most frequent element
        var excludedNumber = 2;
        var mostFrequentElement = numbers
            .Where(n => n != excludedNumber)
            .GroupBy(n => n)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefault();

        return mostFrequentElement;
    }

    private bool IsFiveOfKind(List<int> cards)
    {
        return cards.Distinct().Count() == 1;
    }
    private bool IsFourOfKind(List<int> cards)
    {
        var groups = cards.GroupBy(x => x);
        return groups.Count() == 2 && (groups.First().Count() == 1 || groups.First().Count() == 4);
    }
    private bool IsFullHouse(List<int> cards)
    {
        var groups = cards.GroupBy(x => x);
        return groups.Count() == 2 && (groups.First().Count() == 2 || groups.First().Count() == 3);
    }
    private bool IsThreeOfKind(List<int> cards)
    {
        var groups = cards.GroupBy(x => x).ToArray();
        var group1Count = groups[0].Count();
        var group2Count = groups[1].Count();
        var group3Count = groups[2].Count();

        return groups.Count() == 3 && ((group1Count == 3 && (group2Count == 1 && group3Count == 1)) ||
                                       (group2Count == 3 && (group1Count == 1 || group3Count == 1)) ||
                                       (group3Count == 3 && (group1Count == 1 || group2Count == 1)));
    }
    
    private bool IsTwoPairs(List<int> cards)
    {
        var groups = cards.GroupBy(x => x).ToArray();
        var group1Count = groups[0].Count();
        var group2Count = groups[1].Count();
        var group3Count = groups[2].Count();

        return groups.Count() == 3 && ((group1Count == 2 && (group2Count == 2 || group3Count == 2)) ||
                                       (group2Count == 2 && (group1Count == 2 || group3Count == 2)));
    }
    
    private bool IsOnePair(List<int> cards)
    {
        var groups = cards.GroupBy(x => x).ToArray();
        var group1Count = groups[0].Count();
        var group2Count = groups[1].Count();
        var group3Count = groups[2].Count();

        if (groups.Count() == 3 && (group1Count == 3 || group2Count == 3 || group3Count == 3))
        {
            return true;
        }

        var group4Count = groups[3].Count();

        var groupCounts = new[] { group1Count, group2Count, group3Count, group4Count };

        // there should be 1 group of 2 and 3 groups of 1
        var countGroups = groupCounts.GroupBy(i => i).ToArray();

        if (groups.Count() == 4 &&
            countGroups.Count() == 2 &&
            (countGroups[0].Count() == 1 || countGroups[0].Count() == 3))
            return true;
        return false;
    }
    
    private bool IsHighCard(List<int> cards)
    {
        var groups = cards.GroupBy(x => x);
        return groups.Count() == 5;
    }
}