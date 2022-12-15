using System.Text.RegularExpressions;
using AdventOfCode2022.Solutions.Cartesian;

namespace AdventOfCode2022.Solutions;

public class Day15 : IAdventSolution
{
    private readonly Regex _inputRegex = new (@"Sensor at x=([\d-]+), y=([\d-]+): closest beacon is at x=([\d-]+), y=([\d-]+)");
    
    public object PartOne(string input) => PotentialBeaconsInRow(input, 2000000);
    
    public int PotentialBeaconsInRow(string input, int rowToCheck)
    {
        var sensors = ParseSensors(input);
        
        var xMin = sensors.Select(t => t.Item1.X - t.Item3).Min();
        var xMax = sensors.Select(t => t.Item1.X + t.Item3).Max();

        var beaconsInLine = sensors.Select(s => s.Item2).Distinct().Count(b => b.Y == rowToCheck);

        var x = xMin;
        var pointsInSomeSensorRange = 0;
        while (x <= xMax)
        {
            var sensorData = sensors.FirstOrDefault(s => QuickCheckWithinRange(s.Item1, x, rowToCheck, s.Item3));
            if (sensorData == null)
            {
                ++x;
                continue;
            }
            
            var sensor = sensorData.Item1;
            var range = sensorData.Item3;

            while (QuickCheckWithinRange(sensor, x, rowToCheck, range))
            {
                ++pointsInSomeSensorRange;
                ++x;
            }
        }

        return pointsInSomeSensorRange - beaconsInLine;
    }

    private List<Tuple<Point, Point, int>> ParseSensors(string input) => input.Split(Environment.NewLine)
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

    private bool QuickCheckWithinRange(Point sensor, int x, int y, int distance) =>
        Math.Abs(sensor.X - x) + Math.Abs(sensor.Y - y) <= distance;

    public object PartTwo(string input) => TuningFrequencyInSearchSpace(input, 4000000);

    public long TuningFrequencyInSearchSpace(string input, int width)
    {
        var sensors = ParseSensors(input);

        // Iterating the space is way too much so check round the edge of each sensor range
        foreach (var (sensor, _, range) in sensors)
        {
            Console.WriteLine($"checking edge of sensor {sensor} with range {range}");

            for (var y = Math.Max(0, sensor.Y - range - 1); y <= width && y <= sensor.Y + range + 1; ++y)
            {
                var xDistance = range - Math.Abs(sensor.Y - y) + 1; // just outside range
                if (sensor.X - xDistance >= 0 && !sensors.Any(s => QuickCheckWithinRange(s.Item1, sensor.X - xDistance, y, s.Item3)))
                {
                    return 1L * (sensor.X - xDistance) * 4000000 + y;
                }
                if (sensor.X - xDistance <= width && !sensors.Any(s => QuickCheckWithinRange(s.Item1, sensor.X + xDistance, y, s.Item3)))
                {
                    return 1L * (sensor.X + xDistance) * 4000000 + y;
                }
            }
        }

        return -1;
    }
}