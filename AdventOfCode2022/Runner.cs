// See https://aka.ms/new-console-template for more information

using System.Reflection;
using AdventOfCode2022.Advent;
using AdventOfCode2022.Solutions;

var advent = new AdventCountdown(new NowDateProvider());

Console.WriteLine("Advent Of Code 2022");

if (advent.IsAdventNow()) {
    var day = advent.DayOfAdvent();
    if (day == 25)
    {
        Console.WriteLine("Merry Christmas!");
    }
    else
    {
        Console.WriteLine("Only {0} days until Christmas!", 25 - day);
    }
}

try
{
    var dayToRun = DayToRun();
    SolveForDay(dayToRun);
}
catch (ArgumentException e)
{
    Console.Error.WriteLine("Invalid arguments: {0}", e.Message);
}
catch (FormatException e)
{
    Console.Error.WriteLine("Error parsing the requested day: {0}", e.Message);
}


int DayToRun()
{
    if (args.Length > 1)
    {
        throw new ArgumentException("Only one argument required");
    }
    if (args.Length == 0)
    {
        return advent.DayOfAdvent();
    }
    
    var requestedDay = int.Parse(args[0]);
    if (requestedDay is < 1 or > 25)
    {
        throw new ArgumentOutOfRangeException(nameof(args), "First argument must be a day of advent, between 1 and 25");
    }

    Console.WriteLine("Day {0} requested", requestedDay);
    return requestedDay;
}

void SolveForDay(int day)
{
    var assembly = Assembly.GetExecutingAssembly();
    var dayToRunString = day.ToString().PadLeft(2, '0');
    var solverType = assembly.GetType($"AdventOfCode2022.Solutions.Day{dayToRunString}");
    if (solverType == null)
    {
        Console.Error.WriteLine("No solution yet for day {0}", dayToRunString);
        return;
    }
    var inputFilename = $"AdventOfCode2022.Inputs.Day{dayToRunString}.txt";
    using var stream = assembly.GetManifestResourceStream(inputFilename);
    if (stream == null)
    {
        Console.Error.WriteLine("No input found for day {0}", dayToRunString);
        return;
    }
    using var reader = new StreamReader(stream);
    var input = reader.ReadToEnd();
    var solverObject = Activator.CreateInstance(solverType);
    if (solverObject == null)
    {
        Console.Error.WriteLine("Can't create solution class for day {0}", dayToRunString);
        return;
    }

    var solver = (IAdventSolution)solverObject;
    var part1 = solver.PartOne(input);
    Console.WriteLine("Day {0} Part 1: {1}", dayToRunString, part1);
    var part2 = solver.PartTwo(input);
    Console.WriteLine("Day {0} Part 2: {1}", dayToRunString, part2);
}