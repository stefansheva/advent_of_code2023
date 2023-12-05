using System.Text.RegularExpressions;

Console.WriteLine("Day 3, Part 1 START");
var gameExamplePartOne = @"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..";
const string newLineSeparator = "\n";
const string symbolRegex = @"[^.0-9]";
const string numberRegex = @"\d+";
var input = await File.ReadAllTextAsync("./input.txt");
var rows = input.Split(newLineSeparator);

var symbols = new List<(string val, int row, int col)>();
var numbers = new List<(string val, int row, int col)>();

for (var i = 0; i < rows.Length; i++)
{
    var sym = Regex.Matches(rows[i], symbolRegex);
    symbols.AddRange(sym.Select(x => (x.Value, i, x.Index)));
    var nums = Regex.Matches(rows[i], numberRegex);
    numbers.AddRange(nums.Select(x => (x.Value, i, x.Index)));
}

bool IsNeighbour((string val, int row, int col) sym, (string val, int row, int col) num)
{
    return Math.Abs(num.row - sym.row) <= 1 &&
           sym.col <= num.col + num.val.Length &&
           num.col <= sym.col + sym.val.Length;
}

var sum = numbers
    .Where(num => symbols.Any(sym => IsNeighbour(sym, num)))
    .Select(x => int.Parse(x.val))
    .Sum();

Console.WriteLine(sum);
Console.WriteLine("Day 3, Part 1 END");

Console.WriteLine("Day 3, Part 2 START");
const string gearSeparator = @"\*";
var gears = new List<(string val, int row, int col)>();
numbers = new List<(string val, int row, int col)>();

for (var i = 0; i < rows.Length; i++)
{
    var gearsMatches = Regex.Matches(rows[i], gearSeparator);
    gears.AddRange(gearsMatches.Select(x => (x.Value, i, x.Index)));
    var nums = Regex.Matches(rows[i], numberRegex);
    numbers.AddRange(nums.Select(x => (x.Value, i, x.Index)));
}

var part2 = (
    from g in gears
    let neighbours = from n in numbers where IsNeighbour(n, g) select int.Parse(n.val)
    where neighbours.Count() == 2
    select neighbours.First() * neighbours.Last()
).Sum();


Console.WriteLine(part2);
Console.WriteLine("Day 3, Part 2 END");

