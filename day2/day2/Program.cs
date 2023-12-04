using System.Text.RegularExpressions;
// ReSharper disable InconsistentNaming

const string gameSetsSeparator = ";";
const string ballSeparator = ",";
const string GAME_REPLACE_IDENTIFIER = "Game ";
const string RED_REPLACE_IDENTIFIER = " red";
const string GREEN_REPLACE_IDENTIFIER = " green";
const string BLUE_REPLACE_IDENTIFIER = " blue";
const string RED = "red";
const string GREEN = "green";
const string BLUE = "blue";
const string regexReplacePattern = @"(?:Game [0-9]+: )";

Console.WriteLine("Day 2, Part 1, Start");
var gameExample = @"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green";

var gameInput = await File.ReadAllTextAsync("./input.txt");
var gameList = gameInput.Split("\n");

bool isBallValid(string ball, int validationMarker, string identifier)
{
    var extractNumber = ball.Replace(identifier, string.Empty);
    var num = int.Parse(extractNumber);
    return num <= validationMarker;
}

bool ValidateGameSet(string set)
{
    var isValid = true;
    var cubes = set.Split(ballSeparator);
    
    foreach (var cube in cubes)
    {
        var ball = cube.Trim();
        if (ball.Contains(RED))
        {
            isValid = isBallValid(ball, 12, RED_REPLACE_IDENTIFIER);
        } 
        else if (ball.Contains(GREEN))
        {
            isValid = isBallValid(ball, 13, GREEN_REPLACE_IDENTIFIER);
        } 
        else if (ball.Contains(BLUE))
        {
            isValid = isBallValid(ball, 14, BLUE_REPLACE_IDENTIFIER);
        }
        
        if (!isValid) break;
    }

    return isValid;
}

var validGamesCounter = 0;
foreach (var game in gameList)
{
    var gameId = game.Replace(GAME_REPLACE_IDENTIFIER, string.Empty).Split(":").First();
    var gameSets = Regex.Replace(game, regexReplacePattern, string.Empty).Split(gameSetsSeparator);
    var isValid = true;

    foreach (var set in gameSets)
    {
        isValid = ValidateGameSet(set.Trim());
        if (!isValid) break;
    }

    if (isValid)
    {
        int.TryParse(gameId, out var gameNumber);
        validGamesCounter += gameNumber;
    }
}

Console.WriteLine($"Day 2, Part 1, END Total: {validGamesCounter}");

Console.WriteLine("Day 2, Part 2, Start");
var gameExamplePartTwo = @"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green";

int FindMax(IEnumerable<string> sets, string identifier) => sets
    .Select(set => set.Trim().Replace(identifier, string.Empty))
    .Select(extractNumber => int.Parse(extractNumber))
    .Prepend(0).Max();

(int, int, int) FindMaxBallValue(string gameSets)
{
    var sets = gameSets.Replace(gameSetsSeparator, ballSeparator).Split(ballSeparator);
    (var red, var green, var blue) =  (0, 0, 0);

    red = FindMax(sets.Where(x => x.Contains(RED)), RED_REPLACE_IDENTIFIER);
    green = FindMax(sets.Where(x => x.Contains(GREEN)), GREEN_REPLACE_IDENTIFIER);
    blue = FindMax(sets.Where(x => x.Contains(BLUE)), BLUE_REPLACE_IDENTIFIER);
    
    return (red, green, blue);
}

var powerSetsSum = 0;
var gameInputPartTwo = gameInput.Split("\n");
foreach (var game in gameInputPartTwo)
{
    var gameSets = Regex.Replace(game, regexReplacePattern, string.Empty);
    (var red, var green, var blue) = FindMaxBallValue(gameSets.Trim());
    powerSetsSum += red * green * blue;
}

Console.WriteLine($"Day 2, Part 2 END, Total: {powerSetsSum}");
