using System.Text.RegularExpressions;

Console.WriteLine("Day 2, Part 1, Start");
var gameExample = @"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green";

var gameInput = await File.ReadAllTextAsync("./input.txt");
var gameList = gameInput.Split("\n");
const string gameSetsSeparator = ";";
const string ballSeparator = ",";
const string gameReplaceIdentifier = "Game ";
var regexReplacePattern = @"(?:Game [0-9]+: )";

bool ValidateBall(string ball)
{
    var num = 0;
    var isValid = true;
    if (ball.Contains("red"))
    {
        var extractNumber = ball.Replace(" red", string.Empty);
        num = int.Parse(extractNumber);
        isValid = num <= 12;
    } 
    else if (ball.Contains("green"))
    {
        var extractNumber = ball.Replace(" green", string.Empty);
        num = int.Parse(extractNumber);
        isValid = num <= 13;
    } 
    else if (ball.Contains("blue"))
    {
        var extractNumber = ball.Replace(" blue", string.Empty);
        num = int.Parse(extractNumber);
        isValid = num <= 14;
    }

    Console.WriteLine(num);
    return isValid;
}

bool ValidateGameSet(string set)
{
    var isValid = true;
    var balls = set.Split(ballSeparator);
    
    foreach (var ball in balls)
    {
        isValid = ValidateBall(ball.Trim());
        if (!isValid) break;
        Console.WriteLine(ball.Trim());
    }

    return isValid;
}

var validGamesCounter = 0;
foreach (var game in gameList)
{
    var gameId = game.Replace(gameReplaceIdentifier, string.Empty).Split(":").First();
    var gameSets = Regex.Replace(game, regexReplacePattern, string.Empty).Split(gameSetsSeparator);

    var isValid = true;
    foreach (var set in gameSets)
    {
        Console.WriteLine(set.Trim());
        isValid = ValidateGameSet(set.Trim());

        if (!isValid) break;
    }

    if (isValid)
    {
        int.TryParse(gameId, out var gameNumber);
        validGamesCounter += gameNumber;
    }
}

Console.WriteLine("Day 2, Part 1, End");
Console.WriteLine($"Total Part 1: {validGamesCounter}");