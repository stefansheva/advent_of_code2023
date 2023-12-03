using System.Text.RegularExpressions;

int StringToNumberMapper(string element)
{
    if (!int.TryParse(element, out var parsedItem))
    {
        return element switch
        {
            "one" => 1,
            "two" => 2,
            "three" => 3,
            "four" => 4,
            "five" => 5,
            "six" => 6,
            "seven" => 7,
            "eight" => 8,
            "nine" => 9,
            _ => 0,
        };
    }

    return parsedItem;
}

int ConcatenateValue(string firstElement, string secondElement)
{
    var concatenatedValue = $"{StringToNumberMapper(firstElement)}{StringToNumberMapper(secondElement)}";
    Console.WriteLine(concatenatedValue);
    return int.Parse(concatenatedValue);
}

const string separator = "\n";
Console.WriteLine($"---- Advent of Code 2023 - Part 1 Start. ----");
var examplePartOne = @"1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet";

const string partOneRegex = "[0-9]";
var inputDataPartOne = await File.ReadAllTextAsync("./input.txt");
var inputDataList = inputDataPartOne.Split(separator);

var sumPartOne = 0;
foreach (var line in inputDataList)
{
    Console.WriteLine(line);
    var matches = Regex.Matches(line, partOneRegex);
    sumPartOne += matches.Count switch
    {
        0 => 0,
        1 => ConcatenateValue(matches.First().Value, matches.First().Value),
        _ => ConcatenateValue(matches.First().Value, matches.Last().Value),
    };
}

Console.WriteLine($"---- Advent of Code 2023 - Part 1 End. ----");


Console.WriteLine($"---- Advent of Code 2023 - Part 2 Start. ----");
var examplePartTwo = @"two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen";


const string partTwoRegex = @"(?:zero|one|two|three|four|five|six|seven|eight|nine|[0-9])";
var inputDataPartTwo = await File.ReadAllTextAsync("./input_part2.txt");
var trebuchetList = inputDataPartTwo.Split(separator);

static List<string> GetMatches(string input)
{
    var results = new List<string>();
    var regex = new Regex(partTwoRegex);
    var startIndex = 0;

    while (startIndex < input.Length)
    {
        var match = regex.Match(input, startIndex);

        if (match.Success)
        {
            results.Add(match.Value);
            startIndex = match.Index + 1;
        }
        else
        {
            break;
        }
    }

    return results;
}

var sumPartTwo = 0;
foreach (var line in trebuchetList)
{
    Console.WriteLine(line);
    var matches = GetMatches(line);
    sumPartTwo += matches.Count switch
    {
        0 => 0,
        1 => ConcatenateValue(matches.First(), matches.First()),
        _ => ConcatenateValue(matches.First(), matches.Last()),
    };
}

Console.WriteLine($"---- Advent of Code 2023 - Part 2 End. ----");
Console.WriteLine($"Part 1 Result: {sumPartOne}");
Console.WriteLine($"Part 1 Result: {sumPartTwo}");

