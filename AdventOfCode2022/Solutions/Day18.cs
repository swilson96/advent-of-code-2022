using AdventOfCode2022.Solutions.Cartesian;

namespace AdventOfCode2022.Solutions;

public class Day18 : IAdventSolution
{
    public object PartOne(string input)
    {
        var points = new HashSet<Point3>(input.Split(Environment.NewLine).Select(ParsePoint));

        return points
            .Select(point => point.Neighbours.Intersect(points))
            .Select(coveredSides => 6 - coveredSides.Count())
            .Sum();
    }

    private Point3 ParsePoint(string inputLine)
    {
        var bits = inputLine.Split(",");
        return new Point3(int.Parse(bits[0]), int.Parse(bits[1]), int.Parse(bits[2]));
    }

    public object PartTwo(string input) => 0;
}