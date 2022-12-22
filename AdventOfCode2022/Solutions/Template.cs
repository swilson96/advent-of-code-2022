using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solutions;

public class Template : IAdventSolution
{
    private readonly Regex _inputRegex = new (@"Sensor at x=([\d-]+), y=([\d-]+): closest beacon is at x=([\d-]+), y=([\d-]+)");
    
    public object PartOne(string input) => 0;

    public object PartTwo(string input) => 0;
}