using AdventOfCode2022.Solutions.Cartesian;

namespace AdventOfCode2022.Solutions;

public class Day24 : IAdventSolution
{
    public object PartOne(string input)
    {
        var lines = input.Split(Environment.NewLine).ToList();
        var width = lines[0].Length;
        var length = lines.Count;
        var blizzards = lines.Skip(1).Take(length - 2).SelectMany(
                (l, y) => l.Skip(1).Take(width - 2).Where(c => c != '.').Select((c, x) => Blizzard.Parse(c, x, y)))
            .ToList();

        var unvisited = new HashSet<Point>();
        var start = new Point(1, 0);
        var destination = new Point(width - 2, length - 1);

        unvisited.Add(start);

        var time = 1;
        while (unvisited.Count > 0)
        {
            var unvisitedNextRound = new HashSet<Point>();
            blizzards = blizzards.Select(b => b.Move(length, width)).ToList();
            var blizzardPositions = new HashSet<Point>(blizzards.Select(b => b.Position));

            foreach (var current in unvisited)
            {
                foreach (var neighbour in current.Neighbours)
                {
                    if (neighbour.Y < 1 || neighbour.Y >= length - 1 || neighbour.X < 1 ||
                        neighbour.X >= width - 1)
                    {
                        if (!neighbour.Equals(destination))
                        {
                            continue;
                        }
                    }

                    if (blizzardPositions.Contains(neighbour))
                    {
                        continue;
                    }

                    unvisitedNextRound.Add(neighbour);
                }
                
                if (current.Equals(destination))
                {
                    return time + 1;
                }

                // waiting is always an option, if there's no blizzard colliding with us this minute
                if (!blizzardPositions.Contains(current))
                {
                    unvisitedNextRound.Add(current);
                }
            }

            unvisited.UnionWith(unvisitedNextRound);
            ++time;
        }

        return -1;
    }

    public object PartTwo(string input) => 0;

    private class Blizzard
    {
        public Point Position { get; }
        public Direction Direction { get; }

        public Blizzard(Point position, Direction direction)
        {
            Position = position;
            Direction = direction;
        }

        public Blizzard Move(int length, int width)
        {
            var newPos = Direction switch
            {
                Direction.U => Position.Y == length - 2 ? new Point(Position.X, 1) : Position.Add(Direction, 1),
                Direction.R => Position.X == width - 2 ? new Point(1, Position.Y) : Position.Add(Direction, 1),
                Direction.D => Position.Y == 1 ? new Point(Position.X, length - 2) : Position.Add(Direction, 1),
                Direction.L => Position.X == 1 ? new Point(width - 2, Position.Y) : Position.Add(Direction, 1),
                _ => throw new ArgumentOutOfRangeException($"can't move blizzard in direction {Direction}")
            };
            return new Blizzard(newPos, Direction);
        }

        public static Blizzard Parse(char input, int x, int y)
        {
            var pos = new Point(x, y);
            var dir = input switch
            {
                '>' => Direction.R,
                '<' => Direction.L,
                'v' => Direction.U,
                '^' => Direction.D,
                _ => throw new ArgumentOutOfRangeException(nameof(input), input, "no such blizzard symbol " + input)
            };
            return new Blizzard(pos, dir);
        }

        public override string ToString() => $"Day24.Blizzard{{{Position}, {Direction}}}";
    }
}