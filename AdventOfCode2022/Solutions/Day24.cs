using AdventOfCode2022.Solutions.Cartesian;

namespace AdventOfCode2022.Solutions;

public class Day24 : IAdventSolution
{
    public object PartOne(string input)
    {
        var valley = ParseValley(input);

        var start = valley.Start;
        var destination = valley.End;

        return valley.Travel(start, destination);
    }

    private Valley ParseValley(string input)
    {
        var lines = input.Split(Environment.NewLine).ToList();
        var width = lines[0].Length;
        var length = lines.Count;
        var blizzards = lines.Skip(1).Take(length - 2).SelectMany(
                (l, y) => l.Skip(1).Take(width - 2).Select((c, x) => c == '.' ? null : Blizzard.Parse(c, x + 1, y + 1)))
            .Where(b => b != null)
            .Select(b => b!)
            .ToList();

        return new Valley(length, width, blizzards);
    }

    public object PartTwo(string input)
    {
        var valley = ParseValley(input);

        var start = valley.Start;
        var destination = valley.End;

        var toEnd = valley.Travel(start, destination);
        var backToStart = valley.Travel(destination, start);
        var backToEnd = valley.Travel(start, destination);

        return toEnd + backToStart + backToEnd;
    }

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
    
    private class Valley
    {
        private int _length;
        private int _width;
        private List<Blizzard> _blizzards;
        
        public Valley(int length, int width, List<Blizzard> blizzards)
        {
            _length = length;
            _width = width;
            _blizzards = blizzards;
        }

        public Point Start => new (1, 0);

        public Point End => new (_width - 1, _length - 2);

        public int Travel(Point start, Point destination)
        {
            var unvisited = new HashSet<Point>();
            unvisited.Add(start);

            var time = 1;
            while (unvisited.Count > 0)
            {
                var unvisitedNextRound = new HashSet<Point>();
                _blizzards = _blizzards.Select(b => b.Move(_length, _width)).ToList();
                var blizzardPositions = new HashSet<Point>(_blizzards.Select(b => b.Position));

                foreach (var current in unvisited)
                {
                    foreach (var neighbour in current.Neighbours)
                    {
                        if (neighbour.Equals(destination))
                        {
                            return time;
                        }
                    
                        if (neighbour.Y < 1 || neighbour.Y >= _length - 1 || neighbour.X < 1 ||
                            neighbour.X >= _width - 1)
                        {
                            continue;
                        }

                        if (blizzardPositions.Contains(neighbour))
                        {
                            continue;
                        }

                        unvisitedNextRound.Add(neighbour);
                    }
                
                    // waiting is always an option, if there's no blizzard colliding with us this minute
                    if (!blizzardPositions.Contains(current))
                    {
                        unvisitedNextRound.Add(current);
                    }
                }

                unvisited = unvisitedNextRound;
                ++time;
            }

            throw new Exception($"can't get from {start} to {destination} in this valley at this time");
        }
    }
}

