// See https://aka.ms/new-console-template for more information

using AdventOfCode2022.Advent;

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
    Console.WriteLine("No solutions implemented yet");
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
        throw new ArgumentOutOfRangeException(nameof(args), "First argument must be between 1 and 25");
    }

    Console.WriteLine("Day {0} requested", requestedDay);
    return requestedDay;
}