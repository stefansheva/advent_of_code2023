using System.Text.RegularExpressions;

const string RaceSeparator = " ";
const string TimeDistanceSeparator = "\n";
const string TimeRegex = @"(Time:\s+)";
const string DistanceRegex = @"(Distance:\s+)";

var gameInput = @"
Time:      7  15   30
Distance:  9  40  200";

gameInput = await File.ReadAllTextAsync("./input.txt");
var normalizeInput = Regex.Replace(gameInput.Trim(), TimeRegex, string.Empty);
var inputSeparation = normalizeInput.Trim().Split(TimeDistanceSeparator);
var times = Regex.Replace(inputSeparation.First().Trim(), TimeRegex, string.Empty)
    .Split(RaceSeparator)
    .Where(x => !string.IsNullOrWhiteSpace(x))
    .ToList();
var distances = Regex.Replace(inputSeparation.Last().Trim(), DistanceRegex, string.Empty)
    .Split(RaceSeparator)
    .Where(x => !string.IsNullOrWhiteSpace(x))
    .ToList();

var races = (from idx in Enumerable.Range(0, times.Count)
    select (int.Parse(times[idx]), int.Parse(distances[idx]))).ToList();

Console.WriteLine("Day 6, Part One START");

var waysToBeatAllRace = 1;
foreach (var race in races)
{
    var targetDistance = race.Item2;
    var waysToBeatRaceRecord = 0;
    for (int holdBtn = 0; holdBtn < race.Item1; holdBtn++)
    {
        // var holdButtonTime = targetDistance - i;
        var remainingTime = race.Item1 - holdBtn;
        var passedDistance = holdBtn * remainingTime;

        if (passedDistance > targetDistance)
        {
            waysToBeatRaceRecord += 1;
        }
    }

    waysToBeatAllRace *= waysToBeatRaceRecord;
}

Console.WriteLine($"Total Points: {waysToBeatAllRace}");
Console.WriteLine("Day 6, Part One END");

Console.WriteLine("Day 6, Part TWO START");

Console.WriteLine($"Total cards: {0}");
Console.WriteLine("Day 6, Part TWO END");