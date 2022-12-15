using AdventOfCode2022.Solutions.Cartesian;

namespace AdventOfCode2022.Solutions;

public class Day14 : IAdventSolution
{
    private const int Width = 1001;
    
    public object PartOne(string input)
    {
        var lines = ParseRockPaths(input);

        var cave = BuildCave(lines);

        var sand = new HashSet<Point>();
        while (LandSand(cave, sand))
        {
            
        }

        return sand.Count;
    }

    private static List<List<Point>> ParseRockPaths(string input) => input.Split(Environment.NewLine)
        .Select(l => l.Split(" -> "))
        .Select(l => l.Select(s => s.Split(",")))
        .Select(l => l.Select(sa => new Point(int.Parse(sa[0]), int.Parse(sa[1]))).ToList()).ToList();

    private bool[][] BuildCave(List<List<Point>> paths)
    {
        var depth = paths.SelectMany(p => p.Select(point => point.Y)).Max() + 1;

        var cave = Enumerable.Range(0, depth)
            .Select(_ => Enumerable.Range(0, Width).Select(_ => false).ToArray())
            .ToArray();

        foreach (var path in paths.Select(p => p.GetEnumerator()))
        {
            path.MoveNext();
            var start = path.Current;
            while (path.MoveNext())
            {
                var end = path.Current;
                Direction direction;
                if (start.X == end.X)
                {
                    direction = start.Y > end.Y ? Direction.D : Direction.U;
                }
                else
                {
                    direction = start.X > end.X ? Direction.L : Direction.R;
                }

                while (!start.Equals(end))
                {
                    cave[start.Y][start.X] = true;
                    start = start.Add(direction, 1);
                }
                
                start = path.Current;
                if (start.X >= Width)
                {
                    throw new Exception($"found outlier {start} in input");
                }
                cave[start.Y][start.X] = true;
            }
        }

        return cave;
    }

    /**
     * returns true if landed
     */
    private bool LandSand(bool[][] cave, HashSet<Point> sand)
    {
        if (sand.Contains(new Point(500, 0)))
        {
            return false;
        }
        return LandSand(cave, sand, new Point(500, 0));
    }
    
    private bool LandSand(bool[][] cave, HashSet<Point> sand, Point start)
    {
        var x = start.X;
        for (int y = start.Y; y < cave.Length - 1; ++y)
        {
            if (sand.Contains(new Point(x, y + 1)) || cave[y + 1][x])
            {
                // try left
                if (x > 0 && !sand.Contains(new Point(x - 1, y + 1)) && !cave[y + 1][x - 1])
                {
                    return LandSand(cave, sand, new Point(x - 1, y + 1));
                }
                if (x < Width - 1 && !sand.Contains(new Point(x + 1, y + 1)) && !cave[y + 1][x + 1])
                {
                    return LandSand(cave, sand, new Point(x + 1, y + 1));
                }
                sand.Add(new Point(x, y));
                return true;
            }
        }

        return false;
    }

    public object PartTwo(string input)
    {
        var paths = ParseRockPaths(input);
        
        var depth = paths.SelectMany(p => p.Select(point => point.Y)).Max() + 1;

        paths.Add(new List<Point> { new (0, depth + 1), new (Width - 1, depth + 1) });

        var cave = BuildCave(paths);
        
        var sand = new HashSet<Point>();
        while (LandSand(cave, sand))
        {
            
        }
        
        // draw the cave!
        for (var y = 0; y < depth + 2; ++y)
        {
            Console.WriteLine(string.Join("",
                cave[y].Select((c, x) => sand.Contains(new Point(x, y)) ? "o" : c ? "#" : ".").ToList()));
        }

        return sand.Count;
    }
}