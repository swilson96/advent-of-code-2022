using System.Text.RegularExpressions;
using AdventOfCode2022.Solutions.Cartesian;

namespace AdventOfCode2022.Solutions;

public class Day15 : IAdventSolution
{
    private readonly Regex _inputRegex = new (@"Sensor at x=([\d-]+), y=([\d-]+): closest beacon is at x=([\d-]+), y=([\d-]+)");
    
    public object PartOne(string input) => PotentialBeaconsInRow(input, 2000000);
    
    public int PotentialBeaconsInRow(string input, int rowToCheck)
    {
        var sensors = input.Split(Environment.NewLine)
            .Select(l => _inputRegex.Match(l))
            .Select(m =>
            {
                var location = new Point(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
                var beacon = new Point(int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value));
                return new Tuple<Point, Point, int>(
                    location,
                    beacon,
                    Point.ManhattanDistance(location, beacon)
                );
            }).ToList();
        
        var xMin = sensors.Select(t => t.Item1.X - t.Item3).Min();
        var xMax = sensors.Select(t => t.Item1.X + t.Item3).Max();

        var beaconsInLine = sensors.Select(s => s.Item2).Distinct().Count(b => b.Y == rowToCheck);
        
        return Enumerable.Range(xMin, xMax - xMin + 1)
            .Select(x => new Point(x, rowToCheck))
            .Count(p => sensors.Any(s => Point.ManhattanDistance(s.Item1, p) <= s.Item3)) - beaconsInLine;
    }

    public object PartTwo(string input) => 0;
}