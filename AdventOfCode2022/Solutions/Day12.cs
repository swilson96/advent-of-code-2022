using AdventOfCode2022.Solutions.Cartesian;

namespace AdventOfCode2022.Solutions;

public class Day12 : IAdventSolution
{
    public object PartOne(string input)
    {
        var map = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToList();
        var unvisited = new HashSet<Point>();
        var distance = new int[map.Count][];
        var current = Point.Origin;
        var destination = Point.Origin;
        for (var y = 0; y < map.Count; ++y)
        {
            distance[y] = new int[map[y].Length];
            for (var x = 0; x < map[y].Length; ++x)
            {
                unvisited.Add(new Point(x, y));
                distance[y][x] = int.MaxValue;
                if (map[y][x] == 'S')
                {
                    current = new Point(x, y);
                    distance[y][x] = 0;
                    map[y][x] = 'a';
                }
                else if (map[y][x] == 'E')
                {
                    destination = new Point(x, y);
                    map[y][x] = 'z';
                }
            }
        }

        while (unvisited.Count > 0)
        {
            foreach (var neighbour in current.Neighbours)
            {
                if (neighbour.Y < 0 || neighbour.Y >= map.Count || neighbour.X < 0 || neighbour.X >= map[neighbour.Y].Length)
                {
                    continue;
                }

                if (!unvisited.Contains(neighbour))
                {
                    continue;
                }

                if (map[neighbour.Y][neighbour.X] - map[current.Y][current.X] > 1)
                {
                    continue;
                }

                distance[neighbour.Y][neighbour.X] = Math.Min(distance[neighbour.Y][neighbour.X], distance[current.Y][current.X] + 1);
            }
            
            unvisited.Remove(current);
            
            if (current.Equals(destination))
            {
                return distance[destination.Y][destination.X];
            }

            current = unvisited.OrderBy(u => distance[u.Y][u.X]).First();
        }

        return 0;
    }

    public object PartTwo(string input) => 0;
}