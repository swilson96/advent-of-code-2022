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

    public object PartTwo(string input)
    {
        var obsidian = new HashSet<Point3>(input.Split(Environment.NewLine).Select(ParsePoint));
        var neighbours = new HashSet<Point3>(obsidian.SelectMany(o => o.Neighbours).Where(n => !obsidian.Contains(n)));

        var shell = new HashSet<Point3>(neighbours
            .SelectMany(n => n.Neighbours.Where(n2 => !obsidian.Contains(n2)).Prepend(n)));
        
        // The enemy's gate is down
        var start = shell.OrderByDescending(p => p.Y).First();
        
        // Flow down and around to find the outer shell
        var outerShell = new HashSet<Point3> { start };
        var unvisited = new Queue<Point3>();
        unvisited.Enqueue(start);
        while (unvisited.Any())
        {
            var current = unvisited.Dequeue();
            foreach (var neighbour in current.Neighbours)
            {
                if (shell.Contains(neighbour) && !outerShell.Contains(neighbour))
                {
                    outerShell.Add(neighbour);
                    unvisited.Enqueue(neighbour);
                }
            }
        }

        var innerCoating = new HashSet<Point3>(neighbours.Except(outerShell));
        
        return obsidian
            .Select(point => point.Neighbours.Count(p => !obsidian.Contains(p) && !innerCoating.Contains(p)))
            .Sum();
    }
}