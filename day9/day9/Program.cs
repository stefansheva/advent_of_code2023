var gameInput = @"0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45";

gameInput = await File.ReadAllTextAsync("./input.txt");
const string HistoryLineSeparator = "\n";
const string HistorySequenceSeparator = " ";
Console.WriteLine("DAY 9 PART 1, START");

var normalizeInput = gameInput.Split(HistoryLineSeparator).ToList();

List<long> FindSequenceDiff(List<long> sequence)
{
    var newSequence = new List<long>();
    for (var i = 1; i < sequence.Count; i++)
    {
        var item = sequence[i] - sequence[i - 1];
        newSequence.Add(item);
    }

    return newSequence;
}

long ExtrapolateSequence(List<long> sequence)
{
    if (!sequence.Any()) return 0;

    return ExtrapolateSequence(FindSequenceDiff(sequence)) + sequence.Last();
}

long ExtrapolateSequenceReverse(List<long> sequence)
{
    var reversed = sequence.ToArray().Reverse().ToList();
    return ExtrapolateSequence(reversed);
}

long extrapolatedSum = 0;
long extrapolatedSumReversed = 0;
foreach (var input in normalizeInput)
{
    var historySequence = input.Trim().Split(HistorySequenceSeparator).Select(long.Parse).ToList();
    extrapolatedSum += ExtrapolateSequence(historySequence);
    extrapolatedSumReversed += ExtrapolateSequenceReverse(historySequence);
}

Console.WriteLine($"TOTAL: {extrapolatedSum}");
Console.WriteLine($"TOTAL REVERSED: {extrapolatedSumReversed}");